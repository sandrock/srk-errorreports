using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Srk.BetaServices {
    public interface IHttpRequestWrapper {

        /// <summary>
        /// Download a response string.
        /// </summary>
        /// <param name="callback">delegate called when the query is succesful</param>
        /// <param name="errorCallback">delegate called when the query encoutered an error</param>
        /// <param name="controller">Service controller</param>
        /// <param name="action">Service action</param>
        /// <param name="parameters">Query string parameters</param>
        void ExecuteQuery(AsyncCallback callback, Action<Exception> errorCallback, string controller, string action, Dictionary<string, string> parameters);

        /// <summary>
        /// Download a response string with POST data.
        /// </summary>
        /// <param name="callback">delegate called when the query is succesful</param>
        /// <param name="errorCallback">delegate called when the query encoutered an error</param>
        /// <param name="controller">Service controller</param>
        /// <param name="action">Service action</param>
        /// <param name="parameters">Query string parameters</param>
        /// <param name="postData">POST data as a stream</param>
        void ExecuteQuery(AsyncCallback callback, Action<Exception> errorCallback, string controller, string action, Dictionary<string, string> parameters, Stream postData);

    }
}
