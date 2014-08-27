using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Web;

namespace Srk.BetaServices {
    public partial class HttpRequestWrapper : IHttpRequestWrapper {

        public void ExecuteQuery(AsyncCallback callback, Action<Exception> errorCallback, string controller, string action, Dictionary<string, string> parameters) {
            string queryString = GetQueryString(controller, action, parameters);

            // prepare the web page we will be asking for
            HttpWebRequest request = (HttpWebRequest)
                WebRequest.Create(queryString);

            request.AllowAutoRedirect = false;
            request.UserAgent = UserAgent;

            // execute the request
            try {
                var r = request.BeginGetResponse(delegate(IAsyncResult @async) {
                    try {
                        HttpWebRequest request2 = (HttpWebRequest)@async.AsyncState;
                        HttpWebResponse response = null;

                        response = (HttpWebResponse)request2.EndGetResponse(@async);
                        HandleHttpCodes(response.StatusCode);

                        callback(response.GetResponseStream());
                    } catch (Exception ex) {
                        errorCallback(ex);
                    }
                }, request);
            } catch (Exception ex) {
                errorCallback(ex);
            }
        }

        public void ExecuteQuery(AsyncCallback callback, Action<Exception> errorCallback, string controller, string action, Dictionary<string, string> parameters, Stream postData) {
            string queryString = GetQueryString(controller, action, parameters);


            //using (var reader = new StreamReader(postData)) {
            //    var response = reader.ReadToEnd();
            //}
            //postData.Seek(0, SeekOrigin.Begin);

            // prepare the web page we will be asking for
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(queryString);

            request.AllowAutoRedirect = false;
            request.UserAgent = UserAgent;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            // get request and post data
            request.BeginGetRequestStream(getRequestResult => {
                try {
                    using (var post = request.EndGetRequestStream(getRequestResult)) {
                        byte[] buffer = new byte[4096];
                        int bytesRead = 0;
                        while ((bytesRead = postData.Read(buffer, 0, buffer.Length)) != 0) {
                            post.Write(buffer, 0, bytesRead);
                            post.Flush();
                        }
                        post.Close();
                        postData.Close();
                        postData.Dispose();
                    }
                } catch (Exception ex) {
                    errorCallback(ex);
                    return;
                }

                // get response 
                request.BeginGetResponse(getResponseResult => {
                    try {
                        HttpWebResponse response = null;

                        response = (HttpWebResponse)request.EndGetResponse(getResponseResult);
                        HandleHttpCodes(response.StatusCode);

                        callback(response.GetResponseStream());
                    } catch (Exception ex) {
                        errorCallback(ex);
                    }
                }, null);
            }, null);
        }

        /// <summary>
        /// Create a HTTP query string from a dictionary of parameters.
        /// </summary>
        /// <param name="action"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private string GetQueryString(string controller, string action, Dictionary<string, string> parameters) {
            parameters = parameters ?? new Dictionary<string, string>();

            var querystring = parameters.GetQueryString();

            string str = string.Format(UrlFormat, BaseUrl, controller, action, querystring);
            return str;
        }

    }
}
