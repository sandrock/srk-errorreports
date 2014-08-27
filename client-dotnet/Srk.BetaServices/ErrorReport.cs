
namespace Srk.BetaServices
{
    using System;
    using System.Text;

    public class ErrorReport
    {
        public ErrorReport()
        {
            this.Culture = System.Threading.Thread.CurrentThread.CurrentUICulture.Name;
            this.OSPlatform = Environment.OSVersion.Platform.ToString();
            this.OSVersion = Environment.OSVersion.Version.ToString();
        }

        public void SetException(Exception exception)
        {
            if (exception == null)
                throw new ArgumentNullException("exception");

            this.ExceptionType = exception.GetType().FullName;
            this.ExceptionMessage = exception.Message;
            this.ExceptionTrace = exception.StackTrace;

            Exception ex = exception;
            var s = new StringBuilder();
            int i = 0;
            do
            {
                s.Append("# " + i + ": ");
                s.AppendLine(ex.GetType().FullName);
                s.AppendLine(ex.Message);

                i++;
                ex = ex.InnerException;

                if (ex != null)
                    s.AppendLine();
            } while (ex != null);

            this.FullException = s.ToString();
        }

        public void SetNonException(string message)
        {
            this.ExceptionType = "not an exception";
            this.ExceptionMessage = message;

            try
            {
                throw new Exception();
            }
            catch (Exception ex)
            {
                this.ExceptionTrace = ex.StackTrace;
            }
        }

        public string ExceptionType { get; set; }
        public string ExceptionMessage { get; set; }
        public string ExceptionTrace { get; set; }
        public string FullException { get; set; }

        public DateTime? AppStartTime { get; set; }
        public DateTime AppErrorTime { get; set; }
        public DateTime? AppExitTime { get; set; }

        public string AssemblyName { get; set; }
        public string AssemblyVersion { get; set; }

        public string Culture { get; set; }

        public string DeploymentKind { get; set; }
        public string DeploymentComment { get; set; }

        public string OSPlatform { get; set; }
        public string OSVersion { get; set; }

        public string UserId { get; set; }

        public string DeviceId { get; set; }
        public string DeviceManufacturer { get; set; }
        public string DeviceName { get; set; }

        public long DeviceTotalMemory { get; set; }
        public long AppCurrentMemoryUsage { get; set; }
        public long AppPeakMemoryUsage { get; set; }

        public string HttpRequest { get; set; }
        public string HttpReferer { get; set; }
        public string HttpMethod { get; set; }
        public string HttpHost { get; set; }

        public string Comment { get; set; }

        public void AppendComment(string comment)
        {
            if (string.IsNullOrEmpty(comment))
                return;

            if (this.Comment != null && this.Comment.Length > 0)
            {
                this.Comment = string.Concat(this.Comment, Environment.NewLine, comment);
            }
            else
            {
                this.Comment = comment;
            }
        }
    }
}
