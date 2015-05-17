using System;
using Nancy;
using Nancy.ErrorHandling;
using NLog;

namespace DevDayCFP.Services
{
    public class LoggingErrorHandler : IStatusCodeHandler
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public bool HandlesStatusCode(HttpStatusCode statusCode, NancyContext context)
        {
            return statusCode == HttpStatusCode.InternalServerError;
        }

        public void Handle(HttpStatusCode statusCode, NancyContext context)
        {
            object errorObject;
            context.Items.TryGetValue(NancyEngine.ERROR_EXCEPTION, out errorObject);
            var error = errorObject as Exception;

            _logger.Error("Unhandled error", error);
        }
    }
}