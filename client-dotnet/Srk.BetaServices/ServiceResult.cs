
namespace Srk.BetaServices
{
    using System;

    public class ServiceResult
    {
        public ServiceError[] Errors { get; set; }
    }

    public class ServiceResult<T> : ServiceResult
    {
        public T Data { get; set; }
    }
}
