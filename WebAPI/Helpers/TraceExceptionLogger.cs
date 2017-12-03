using System.Diagnostics;
using System.Web.Http.ExceptionHandling;

namespace WebAPI.Helpers
{
    public class TraceExceptionLogger : ExceptionLogger
    {
        public override void Log(ExceptionLoggerContext context)
        {
            Trace.TraceError(context.ExceptionContext.Exception.ToString());
            // Log your error here, Db or Service.
        }
    }
}