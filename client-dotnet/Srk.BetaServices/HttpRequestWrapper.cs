
namespace Srk.BetaServices
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Text;

    public partial class HttpRequestWrapper : IHttpRequestWrapper
    {
        /// <summary>
        /// Use a <see cref="HttpWebRequest"/> to download a response string.
        /// </summary>
        /// <param name="action">Service action</param>
        /// <param name="parameters">Query string parameters</param>
        /// <returns>HTTP response body as a string.</returns>
        public Stream ExecuteQuery(string controller, string action, Dictionary<string, string> parameters)
        {
            string queryString = GetQueryString(controller, action, parameters);

            StringBuilder sb = new StringBuilder();

            // prepare the web page we will be asking for
            HttpWebRequest request = (HttpWebRequest)
                WebRequest.Create(new Uri(queryString, UriKind.Absolute));

            request.AllowAutoRedirect = false;
            request.UserAgent = UserAgent;

            // execute the request
            HttpWebResponse response = (HttpWebResponse)
                request.GetResponse();

            HandleHttpCodes(response.StatusCode);

            // we will read data via the response stream
            Stream resStream = response.GetResponseStream();

            return resStream;
#if DEBUG
            string tempString = null;
            int count = 0;
            byte[] buf = new byte[8192];

            do
            {
                // fill the buffer with data
                count = resStream.Read(buf, 0, buf.Length);

                // make sure we read some data
                if (count != 0)
                {
                    // translate from bytes to ASCII text
                    tempString = Encoding.UTF8.GetString(buf, 0, count);

                    // continue building the string
                    sb.Append(tempString);
                }
            }
            while (count > 0); // any more data to read?

            var stringResponse = sb.ToString();
            //return stringResponse;
#endif
        }

        public Stream ExecuteQuery(string controller, string action, Dictionary<string, string> parameters, Stream postData)
        {
            if (postData == null)
                throw new ArgumentNullException("postData");

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
            ////var getRequestResult = request.GetRequestStream();
            try
            {
                using (var post = request.GetRequestStream())
                {
                    byte[] buffer = new byte[4096];
                    int bytesRead = 0;
                    while ((bytesRead = postData.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        post.Write(buffer, 0, bytesRead);
                        post.Flush();
                    }
                    post.Close();
                    postData.Close();
                    postData.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            // get response 
            try
            {
                HttpWebResponse response = null;

                response = (HttpWebResponse)request.GetResponse();
                HandleHttpCodes(response.StatusCode);

                return response.GetResponseStream();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Create a HTTP query string from a dictionary of parameters.
        /// </summary>
        /// <param name="action"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private string GetQueryString(string controller, string action, Dictionary<string, string> parameters)
        {
            parameters = parameters ?? new Dictionary<string, string>();

            var querystring = parameters.GetQueryString();

            string str = string.Format(UrlFormat, BaseUrl, controller, action, querystring);
            return str;
        }
    }
}
