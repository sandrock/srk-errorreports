
namespace Srk.BetaServices
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    partial class JsonBetaServices
    {
        private readonly string BaseUrl;
        private readonly string UrlFormat;

        private readonly string ApiKey;
        private readonly string UserAgent;
        private readonly string BaseUserAgent = Version.LibraryUserAgent;
        private string _realUserAgent;

        private readonly IHttpRequestWrapper http;

        /// <summary>
        /// Auxiliary class .ctor for customization and unit testing.
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="userAgent"></param>
        /// <param name="baseUrl"></param>
        /// <param name="urlFormat"></param>
        public JsonBetaServices(string apiKey, string userAgent, string baseUrl, string urlFormat, IHttpRequestWrapper httpWrapper)
        {
            BaseUrl = baseUrl;
            UrlFormat = urlFormat ?? "{0}{1}.php?action={2}&{3}";
            ApiKey = apiKey;
            UserAgent = userAgent ?? "unknown-agent";

            http = httpWrapper ?? new HttpRequestWrapper(UrlFormat, BaseUrl, RealUserAgent);
        }

        /// <summary>
        /// Auxiliary class .ctor for customization.
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="userAgent"></param>
        /// <param name="baseUrl"></param>
        /// <param name="urlFormat"></param>
        public JsonBetaServices(string apiKey, string userAgent, string baseUrl, string urlFormat)
            : this(apiKey, userAgent, baseUrl, urlFormat, null) { }

        /// <summary>
        /// Main class .ctor.
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="userAgent"></param>
        public JsonBetaServices(string apiKey, string userAgent)
            : this(apiKey, userAgent, null, null) { }

        private string RealUserAgent
        {
            get
            {
                return _realUserAgent ?? (_realUserAgent = string.Format("{0} {1}", BaseUserAgent, UserAgent));
            }
        }

        private static Dictionary<string, string> ConvertKeysValusToDictionary(string[] keyValues)
        {
            if (keyValues.Length % 2 != 0)
                throw new ArgumentException("Invalid parameters count", "keyValues");
            Dictionary<string, string> parameters = null;
            if (keyValues.Length > 0)
            {
                parameters = new Dictionary<string, string>();
                bool isKey = true;
                string key = null;
                foreach (var item in keyValues)
                {
                    if (isKey)
                    {
                        key = item;
                    }
                    else
                    {
                        parameters.Add(key, item);
                    }

                    isKey = !isKey;
                }
            }

            return parameters;
        }

        /// <summary>
        /// Handle a custom error via <see cref="HandleCustomError"/>.
        /// The throws an exception if the parameter is not null.
        /// </summary>
        /// <param name="exception"></param>
        protected void HandleError(Exception exception)
        {
            HandleCustomError(exception);
            if (exception is WebException)
            {
                //TODO: find a good exception type here
                throw new Exception("The service does not seem available. Check your Internet connection and the service status. ");
            }

            throw exception;
        }

        /// <summary>
        /// Empty overridable method to handle custom errors.
        /// </summary>
        /// <param name="exception"></param>
        protected virtual void HandleCustomError(Exception exception)
        {
        }

        /// <summary>
        /// Encapsulates a <see cref="BetaError"/> into an exception.
        /// Do nothing if error is null.
        /// </summary>
        /// <param name="error"></param>
        protected static void HandleError(ServiceError error)
        {
            if (error == null)
                return;

            throw new BetaServicesException(error);
        }

        /// <summary>
        /// Encapsulates 1 or many <see cref="BetaError"/>s into an exception.
        /// Do nothing if errors is null.
        /// </summary>
        /// <param name="errors"></param>
        protected static void HandleError(IEnumerable<ServiceError> errors)
        {
            if (errors == null)
                return;

            //TODO: this will save only 1 error if there are many
            HandleError(errors.FirstOrDefault());
        }
    }
}
