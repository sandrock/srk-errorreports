
namespace Srk.BetaServices
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.Serialization.Json;
    using System.Text;

    /// <summary>
    /// Implementation of the <see cref="IBetaServices"/> interface using JSON format and HTTP transport.
    /// </summary>
    public partial class JsonBetaServices : IBetaServices
    {
        #region HTTP Query

        /// <summary>
        /// Download a response string.
        /// </summary>
        /// <param name="action">Service action</param>
        /// <param name="keyValues">Pairs of query string parameters (key1, value1, key2, value2...)</param>
        /// <returns>HTTP response body as a string.</returns>
        protected virtual Stream ExecuteQuery(string controller, string action, params string[] keyValues)
        {
            return ExecuteQuery(controller, action, ConvertKeysValusToDictionary(keyValues));
        }

        /// <summary>
        /// Download a response string.
        /// </summary>
        /// <param name="action">Service action</param>
        /// <param name="parameters">Query string parameters</param>
        /// <returns>HTTP response body as a string.</returns>
        protected virtual Stream ExecuteQuery(string controller, string action, Dictionary<string, string> parameters)
        {
            parameters = parameters ?? new Dictionary<string, string>();
            parameters["key"] = ApiKey;

            return http.ExecuteQuery(controller, action, parameters);
        }

        /// <summary>
        /// Download a response string.
        /// </summary>
        /// <param name="action">Service action</param>
        /// <param name="parameters">Query string parameters</param>
        /// <returns>HTTP response body as a string.</returns>
        protected virtual Stream ExecuteQuery(string controller, string action, Dictionary<string, string> parameters, Stream postData)
        {
            parameters = parameters ?? new Dictionary<string, string>();
            parameters["key"] = ApiKey;

            return http.ExecuteQuery(controller, action, parameters, postData);
        }

        /// <summary>
        /// Returns a <see cref="ServiceResult`T"/> from a HTTP body.
        /// </summary>
        /// <param name="content">the HTTP response content</param>
        /// <returns>the root XML element</returns>
        private static ServiceResult<T> ParseResponse<T>(Stream stream)
        {
            ServiceResult<T> o = null;
            try
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(ServiceResult<T>));
                o = (ServiceResult<T>)serializer.ReadObject(stream);
            }
            catch
            {
                throw;
                //throw new ClientException(ParseErrorMessage, ex);
            }
            finally
            {
                stream.Close();
                stream.Dispose();
            }

            return o;
        }

        /// <summary>
        /// Returns a <see cref="ServiceResult`T"/> from a HTTP body.
        /// </summary>
        /// <param name="content">the HTTP response content</param>
        /// <returns>the root XML element</returns>
        private static ServiceResult ParseResponse(Stream stream)
        {
            ServiceResult o = null;
            try
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(ServiceResult));
                o = (ServiceResult)serializer.ReadObject(stream);
            }
            catch
            {
                throw;
                //throw new ClientException(ParseErrorMessage, ex);
            }
            finally
            {
                stream.Close();
                stream.Dispose();
            }

            return o;
        }

        /// <summary>
        /// Returns a <see cref="ServiceResult`T"/> from a HTTP body.
        /// </summary>
        /// <param name="content">the HTTP response content</param>
        /// <returns>the root XML element</returns>
        private static ServiceResult<T> ParseResponse<T>(string jsonContent)
        {
            try
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(ServiceResult<T>));
                using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonContent)))
                {
                    return (ServiceResult<T>)serializer.ReadObject(stream);
                }
            }
            catch
            {
                throw;
                //throw new ClientException(ParseErrorMessage, ex);
            }
        }

        /// <summary>
        /// Returns a <see cref="ServiceResult`T"/> from a HTTP body.
        /// </summary>
        /// <param name="content">the HTTP response content</param>
        /// <returns>the root XML element</returns>
        private static ServiceResult ParseResponse(string jsonContent)
        {
            try
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(ServiceResult));
                using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonContent)))
                {
                    return (ServiceResult)serializer.ReadObject(stream);
                }
            }
            catch
            {
                throw;
                //throw new ClientException(ParseErrorMessage, ex);
            }
        }

        private T ExecuteAndReturn<T>(string controller, string action, params string[] keyValues)
        {
            return ExecuteAndReturn<T>(controller, action, ConvertKeysValusToDictionary(keyValues));
        }

        private T ExecuteAndReturn<T>(string controller, string action, Dictionary<string, string> parameters)
        {
            var rawResponse = ExecuteQuery(controller, action, parameters);
            var response = ParseResponse<T>(rawResponse);
            HandleError(response.Errors);
            return response.Data;
        }

        private void Execute(string controller, string action, params string[] keyValues)
        {
            Execute(controller, action, ConvertKeysValusToDictionary(keyValues));
        }

        private void Execute(string controller, string action, Dictionary<string, string> parameters)
        {
            var rawResponse = ExecuteQuery(controller, action, parameters);
            var response = ParseResponse(rawResponse);
            HandleError(response.Errors);
        }

        #endregion

        public string Language { get; set; }

        public string[] GetAnnouncementSections()
        {
            return ExecuteAndReturn<string[]>("Announcements", "GetSections");
        }

        public Announcement[] GetAnnouncements(string section)
        {
            return ExecuteAndReturn<Announcement[]>("Announcements", "GetAll");
        }

        public Announcement[] GetAnnouncements(string section, int limit)
        {
            return ExecuteAndReturn<Announcement[]>("Announcements", "GetAll", "limit", limit.ToString());
        }

        public Announcement GetLatestAnnouncement()
        {
            return GetLatestAnnouncement(null);
        }

        public Announcement GetLatestAnnouncement(string section)
        {
            return ExecuteAndReturn<Announcement>("Announcements", "GetLatest");
        }

        public void ReportCrash(ErrorReport report)
        {
            if (report == null)
                throw new ArgumentNullException("report");

            // HACK: DataContractJsonSerializer does not escape url chars
            if (report.ExceptionTrace != null)
                report.ExceptionTrace = report.ExceptionTrace.Replace("&", "%26");
            if (report.Comment != null)
                report.Comment = report.Comment.Replace("&", "%26");
            if (report.DeploymentComment != null)
                report.DeploymentComment = report.DeploymentComment.Replace("&", "%26");

            using (var stream = new MemoryStream())
            {
                var key = UTF8Encoding.UTF8.GetBytes("data=");
                stream.Write(key, 0, key.Length);
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(ErrorReport));
                serializer.WriteObject(stream, report);
                stream.Seek(0L, SeekOrigin.Begin);

                var result = ExecuteQuery("Reports", "PostErrorReport", null, stream);
                var response = ParseResponse(result);
                HandleError(response.Errors);
            }
        }
    }
}
