
namespace Srk.BetaServices
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Web;

    partial class HttpRequestWrapper
    {
        /// <summary>
        /// Base HTTP url for queries. 
        /// This will permit to use a different base adresse (for HTTPS, different port or domain name...).
        /// </summary>
        /// <remarks>
        /// Value must be setted from .ctor.
        /// </remarks>
        private readonly string BaseUrl;

        /// <summary>
        /// Formating string for query string.
        /// Must be set from sub-class.
        /// </summary>
        private readonly string UrlFormat;

        /// <summary>
        /// Formating string for query string.
        /// Must be set from sub-class.
        /// </summary>
        private readonly string UserAgent;

        public HttpRequestWrapper(string urlFormat, string baseUrl, string userAgent)
        {
            UrlFormat = urlFormat;
            BaseUrl = baseUrl;
            UserAgent = userAgent;
        }

        private static void HandleHttpCodes(HttpStatusCode code)
        {
            switch ((int)code)
            {
                // Good statuses
                case (int)HttpStatusCode.OK:
                    break;

                // Redirections (3xx)
                case (int)HttpStatusCode.MultipleChoices:
                case (int)HttpStatusCode.MovedPermanently:
                case (int)HttpStatusCode.Redirect:
                case (int)HttpStatusCode.SeeOther:
                case (int)HttpStatusCode.TemporaryRedirect:
                    //TODO: Choose a right exception type here
                    throw new Exception(
                        "Service did not respond correctly (redirection) (HTTP code: " + (int)code + "). ");

                // Dev errors
                case (int)HttpStatusCode.NotModified:
                case (int)HttpStatusCode.BadRequest:
                case (int)HttpStatusCode.Forbidden:
                case (int)HttpStatusCode.NotFound:
                case (int)HttpStatusCode.MethodNotAllowed:
                case (int)HttpStatusCode.NotAcceptable:
                case (int)HttpStatusCode.RequestTimeout:
                case (int)HttpStatusCode.Gone:
                case (int)HttpStatusCode.LengthRequired:
                case (int)HttpStatusCode.PreconditionFailed:
                case (int)HttpStatusCode.RequestEntityTooLarge:
                case (int)HttpStatusCode.RequestUriTooLong:
                case (int)HttpStatusCode.UnsupportedMediaType:
                case (int)HttpStatusCode.ExpectationFailed:
                case (int)HttpStatusCode.HttpVersionNotSupported:
                    throw new InvalidOperationException(
                        "Service returned an error. The cause seems to be a bad request. " +
                        "Update your application or contact support. ");

                // LOL
                case 418:
                    //TODO: Choose a right exception type here
                    throw new Exception(
                        "Service made a joke (HTTP code: " + (int)code +
                        "). You might want to try again. ");

                // Computer/LAN issue
                case (int)HttpStatusCode.UseProxy:
                case 306:
                case 450:
                    throw new InvalidOperationException(
                        "Something on your computer or network prevents the website from being contacted " +
                        "(HTTP code: " + (int)code + "). ");

                // Server errors 
                case (int)HttpStatusCode.InternalServerError:
                case (int)HttpStatusCode.NotImplemented:
                case (int)HttpStatusCode.BadGateway:
                case (int)HttpStatusCode.ServiceUnavailable:
                case (int)HttpStatusCode.GatewayTimeout:
                    //TODO: Choose a right exception type here
                    throw new Exception(
                        "Service seems to be unavailable (maintenance?), please try again later " +
                        "(HTTP code: " + (int)code + "). ");

                // Other error
                default:
                    //TODO: Choose a right exception type here
                    throw new Exception(
                        "Service did not respond correctly (HTTP code: " + (int)code + "). ");
            }
        }
    }
}
