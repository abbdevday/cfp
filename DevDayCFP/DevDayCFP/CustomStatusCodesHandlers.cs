using System;
using Nancy;
using Nancy.ErrorHandling;
using Nancy.Responses;

namespace DevDayCFP
{
    public class CustomStatusCodesHandlers : IStatusCodeHandler
    {
        public bool HandlesStatusCode(HttpStatusCode statusCode, NancyContext context)
        {
            return statusCode == HttpStatusCode.NotFound
                || statusCode == HttpStatusCode.InternalServerError
                || statusCode == HttpStatusCode.Forbidden
                || statusCode == HttpStatusCode.Unauthorized;
        }

        public void Handle(HttpStatusCode statusCode, NancyContext context)
        {
            try
            {
                if (statusCode == HttpStatusCode.Unauthorized)
                {
                    context.Response = new RedirectResponse("/account/login");
                }
                else
                {
                    var response = new RedirectResponse("/errorcodes/" + (int)statusCode);
                    context.Response = response;
                }
            }
            catch (Exception)
            {
                context.Response.StatusCode = statusCode;
            }
        }
    }
}