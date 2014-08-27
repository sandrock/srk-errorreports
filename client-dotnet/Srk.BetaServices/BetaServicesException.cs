
namespace Srk.BetaServices
{
    using System;

    public partial class BetaServicesException : Exception
    {
        private readonly ServiceError serviceError;

        public BetaServicesException(ServiceError error)
            : base(error.Message)
        {
            if (error == null)
                throw new ArgumentNullException("error");

            this.serviceError = error;
        }

        public ServiceError Error
        {
            get { return this.serviceError; }
        }
    }
}
