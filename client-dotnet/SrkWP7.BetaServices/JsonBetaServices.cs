using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Runtime.Serialization.Json;

namespace Srk.BetaServices {
    public partial class JsonBetaServices : IBetaServices, IDisposable {

        #region HTTP Query

        /// <summary>
        /// Download a response string.
        /// </summary>
        /// <param name="action">Service action</param>
        /// <param name="keyValues">Pairs of query string parameters (key1, value1, key2, value2...)</param>
        /// <returns>HTTP response body as a string.</returns>
        protected virtual void ExecuteQuery(AsyncCallback callback, Action<Exception> errorCallback, string controller, string action, params string[] keyValues) {
            ExecuteQuery(callback, errorCallback, controller, action, ConvertKeysValusToDictionary(keyValues));
        }

        /// <summary>
        /// Download a response string.
        /// </summary>
        /// <param name="action">Service action</param>
        /// <param name="parameters">Query string parameters</param>
        /// <returns>HTTP response body as a string.</returns>
        protected virtual void ExecuteQuery(AsyncCallback callback, Action<Exception> errorCallback, string controller, string action, Dictionary<string, string> parameters) {
            parameters = parameters ?? new Dictionary<string, string>();
            parameters["key"] = ApiKey;

            http.ExecuteQuery(callback, errorCallback, controller, action, parameters);
        }

        /// <summary>
        /// Download a response string.
        /// </summary>
        /// <param name="action">Service action</param>
        /// <param name="parameters">Query string parameters</param>
        /// <returns>HTTP response body as a string.</returns>
        protected virtual void ExecuteQuery(AsyncCallback callback, Action<Exception> errorCallback, string controller, string action, Dictionary<string, string> parameters, Stream postData) {
            parameters = parameters ?? new Dictionary<string, string>();
            parameters["key"] = ApiKey;

            http.ExecuteQuery(callback, errorCallback, controller, action, parameters, postData);
        }

        #region Document parsing

        /// <summary>
        /// Returns a <see cref="ServiceResult`T"/> from a HTTP body.
        /// </summary>
        /// <param name="content">the HTTP response content</param>
        /// <returns>the root XML element</returns>
        private static ServiceResult<T> ParseResponse<T>(Stream stream) {
            ServiceResult<T> o = null;
            try {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(ServiceResult<T>));
                o = (ServiceResult<T>)serializer.ReadObject(stream);
            } catch {
                throw;
                //throw new ClientException(ParseErrorMessage, ex);
            } finally {
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
        private static ServiceResult ParseResponse(Stream stream) {
            ServiceResult o = null;
            try {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(ServiceResult));
                o = (ServiceResult)serializer.ReadObject(stream);
            } catch (Exception ex) {
                throw;
                //throw new ClientException(ParseErrorMessage, ex);
            } finally {
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
        private static ServiceResult<T> ParseResponse<T>(string jsonContent) {
            try {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(ServiceResult<T>));
                using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonContent))) {
                    return (ServiceResult<T>)serializer.ReadObject(stream);
                }
            } catch (Exception ex) {
                throw;
                //throw new ClientException(ParseErrorMessage, ex);
            }
        }

        /// <summary>
        /// Returns a <see cref="ServiceResult`T"/> from a HTTP body.
        /// </summary>
        /// <param name="content">the HTTP response content</param>
        /// <returns>the root XML element</returns>
        private static ServiceResult ParseResponse(string jsonContent) {
            try {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(ServiceResult));
                using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonContent))) {
                    return (ServiceResult)serializer.ReadObject(stream);
                }
            } catch (Exception ex) {
                throw;
                //throw new ClientException(ParseErrorMessage, ex);
            }
        }

        #endregion

        #region Execution helpers

        private void ExecuteAndReturn<T>(Action<T> callback, Action<Exception> errorCallback, string controller, string action, params string[] keyValues) {
            ExecuteAndReturn<T>(callback, errorCallback, controller, action, ConvertKeysValusToDictionary(keyValues));
        }

        private void ExecuteAndReturn<T>(Action<T> callback, Action<Exception> errorCallback, string controller, string action, Dictionary<string, string> parameters) {
            ExecuteQuery(
                new AsyncCallback(stream => {
                    try {
                        var response = ParseResponse<T>(stream);
                        HandleError(response.Errors);
                        callback(response.Data);
                    } catch (Exception ex) {
                        errorCallback(ex);
                    }
                }),
                new Action<Exception>(ex => errorCallback(ex)),
                controller, action, parameters);
        }

        private void Execute(Action callback, Action<Exception> errorCallback, string controller, string action, params string[] keyValues) {
            Execute(callback, errorCallback, controller, action, ConvertKeysValusToDictionary(keyValues));
        }

        private void Execute(Action callback, Action<Exception> errorCallback, string controller, string action, Dictionary<string, string> parameters) {
            ExecuteQuery(
                new AsyncCallback(stream => {
                    try {
                        var response = ParseResponse(stream);
                        HandleError(response.Errors);
                        callback();
                    } catch (Exception ex) {
                        errorCallback(ex);
                    }
                }),
                new Action<Exception>(ex => errorCallback(ex)),
                controller, action, parameters);
        }

        private void Execute(Action callback, Action<Exception> errorCallback, string controller, string action, Stream postData, params string[] keyValues) {
            Execute(callback, errorCallback, controller, action, postData, ConvertKeysValusToDictionary(keyValues));
        }

        private void Execute(Action callback, Action<Exception> errorCallback, string controller, string action, Stream postData, Dictionary<string, string> parameters) {
            ExecuteQuery(
                new AsyncCallback(stream => {
                    try {
                        var response = ParseResponse(stream);
                        HandleError(response.Errors);
                        callback();
                    } catch (Exception ex) {
                        errorCallback(ex);
                    }
                }),
                new Action<Exception>(ex => errorCallback(ex)),
                controller, action, parameters, postData);
        }

        #endregion

        #endregion

        #region IBetaServices Members

        public string Language { get; set; }

        public void GetAnnouncementSectionsAsync() {
            ExecuteAndReturn<string[]>(
                r  => GetAnnouncementSectionsEnded(this, new AsyncResponseArgs<string[]>(r)),
                ex  => GetAnnouncementSectionsEnded(this, new AsyncResponseArgs<string[]>(ex)),
                "Announcements", "GetSections");
        }

        public event AsyncResponseHandler<string[]> GetAnnouncementSectionsEnded;

        public void GetAnnouncementsAsync(string section) {
            var dic = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(Language))
                dic["lang"] = Language;

            ExecuteAndReturn<Announcement[]>(
                r => GetAnnouncementsEnded(this, new AsyncResponseArgs<Announcement[]>(r)),
                ex => GetAnnouncementsEnded(this, new AsyncResponseArgs<Announcement[]>(ex)),
                "Announcements", "GetAll", dic);
        }

        public void GetAnnouncementsAsync(string section, uint limit) {
            var dic = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(Language))
                dic["lang"] = Language;
            dic["limit"] = limit.ToString();

            ExecuteAndReturn<Announcement[]>(
                r => GetAnnouncementsEnded(this, new AsyncResponseArgs<Announcement[]>(r)),
                ex => GetAnnouncementsEnded(this, new AsyncResponseArgs<Announcement[]>(ex)),
                "Announcements", "GetAll", dic);
        }

        public event AsyncResponseHandler<Announcement[]> GetAnnouncementsEnded;

        public void GetLatestAnnouncementAsync() {
            GetLatestAnnouncementAsync(null);
        }

        public void GetLatestAnnouncementAsync(string section) {
            var dic = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(Language))
                dic["lang"] = Language;

            ExecuteAndReturn<Announcement>(
                r => GetLatestAnnouncementEnded(this, new AsyncResponseArgs<Announcement>(r)),
                ex => GetLatestAnnouncementEnded(this, new AsyncResponseArgs<Announcement>(ex)),
                "Announcements", "GetLatest", dic);
        }

        public event AsyncResponseHandler<Announcement> GetLatestAnnouncementEnded;

        public void ReportCrashAsync(ErrorReport report) {
            //HACK: DataContractJsonSerializer does not escape url chars
            if (report.ExceptionTrace != null)
                report.ExceptionTrace = report.ExceptionTrace.Replace("&", "%26");

            var stream = new MemoryStream();
            var key = UTF8Encoding.UTF8.GetBytes("data=");
            stream.Write(key, 0, key.Length);
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(ErrorReport));
            serializer.WriteObject(stream, report);
            stream.Seek(0L, SeekOrigin.Begin);

            ExecuteQuery(
                s => {
                    try {
                        var response = ParseResponse(s);
                        HandleError(response.Errors);
                        ReportCrashEnded(this, new AsyncResponseArgs(true));
                    } catch (Exception ex) {
                        ReportCrashEnded(this, new AsyncResponseArgs(ex));
                    }
                },
                ex => ReportCrashEnded(this, new AsyncResponseArgs(ex)),
                "Reports", "PostErrorReport", null, stream);
        }

        public event AsyncResponseHandler ReportCrashEnded;

        #endregion

        #region IDisposable Members

        public void Dispose() {

        }

        protected virtual void Dispose(bool isDisposing) {
            if (isDisposing) {
                this.ContactEnded = null;
                this.GetAnnouncementSectionsEnded = null;
                this.GetAnnouncementsEnded = null;
                this.GetLatestAnnouncementEnded = null;
                this.GetShowsByCategoryEnded = null;
                this.ReportCrashEnded = null;
                this.ReportShowEnded = null;
                this.ReportUsageEnded = null;
                this.ReportVariousEnded = null;
            }
        }

        #endregion

    }
}
