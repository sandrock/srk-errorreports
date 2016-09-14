
namespace Srk.BetaServices
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// This class helps instanciating client classes.
    /// It also permits to share session tokens between clients.
    /// </summary>
    public class BetaServicesClientFactory
    {
        private string ApiKey;
        private string ApiUserAgent;
        private string BaseUrl;
        private string UrlFormat;

        /// <summary>
        /// This is the main factory instance.
        /// It's recommended to fill this property on application startup.
        /// </summary>
        public static BetaServicesClientFactory Default { get; set; }

        /// <summary>
        /// Class .ctor to create a factory.
        /// </summary>
        /// <param name="apiKey">your API key (ask it on the website, don't use someone else's)</param>
        /// <param name="apiUserAgent">anything like MyBetaseriesApp/1.0.0.0 (name/version)</param>
        /// <param name="shareSessionToken">true will activate session token sharing for all clients created from this factory</param>
        public BetaServicesClientFactory(string apiKey, string apiUserAgent, string baseUrl, string urlFormat)
        {
            if (string.IsNullOrEmpty(apiKey))
                throw new ArgumentException("Missing API key", "apiKey");
            if (string.IsNullOrEmpty(apiUserAgent))
                throw new ArgumentException("Missing UserAgent", "apiUserAgent");

            this.BaseUrl = baseUrl;
            this.UrlFormat = urlFormat;
            this.ApiKey = apiKey;
            this.ApiUserAgent = apiUserAgent;
        }

        /// <summary>
        /// Class .ctor to create a factory.
        /// </summary>
        /// <param name="apiKey">your API key (ask it on the website, don't use someone else's)</param>
        /// <param name="apiUserAgent">anything like MyBetaseriesApp/1.0.0.0 (name/version)</param>
        /// <param name="shareSessionToken">true will activate session token sharing for all clients created from this factory</param>
        public BetaServicesClientFactory(string apiKey, string apiUserAgent)
            : this(apiKey, apiUserAgent, null, null)
        {
        }

        /// <summary>
        /// Create a new client with the factory's configuration.
        /// Object type is <see cref="BetaseriesXmlClient"/>.
        /// </summary>
        /// <returns></returns>
        public IBetaServices CreateDefaultClient()
        {
            var o = new JsonBetaServices(ApiKey, ApiUserAgent, BaseUrl, UrlFormat);
            return o;
        }
    }
}
