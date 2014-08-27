
namespace Srk.BetaServices
{
    using System;

    [Serializable]
    partial class BetaServicesException
    {
        protected BetaServicesException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
