
using System.IO;
namespace Srk.BetaServices {

    /// <summary>
    /// Internal delegate for service response handling.
    /// Public for alternate implementations.
    /// </summary>
    /// <param name="result">HTTP response content</param>
    public delegate void AsyncCallback(Stream resultStream);

    /// <summary>
    /// Internal delegate for service response handling.
    /// Public for alternate implementations.
    /// </summary>
    /// <param name="result">HTTP response content</param>
    public delegate void AsyncCallback<T>(T result);

}
