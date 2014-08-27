
namespace Srk.BetaServices
{
    using System.Collections.Generic;
    using System.IO;

    public interface IHttpRequestWrapper
    {
        /// <summary>
        /// Download a response string.
        /// </summary>
        /// <param name="action">Service action</param>
        /// <param name="parameters">Query string parameters</param>
        /// <returns>HTTP response body as a string.</returns>
        Stream ExecuteQuery(string controller, string action, Dictionary<string, string> parameters);

        Stream ExecuteQuery(string controller, string action, Dictionary<string, string> parameters, Stream postData);
    }
}
