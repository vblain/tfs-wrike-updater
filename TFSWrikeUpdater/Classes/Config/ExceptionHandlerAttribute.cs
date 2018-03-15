using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

namespace Rocklan.WrikeUpdater
{
    public class ExceptionHandlerAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            context.Response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            System.Diagnostics.Debug.WriteLine("Error: " + context.Exception);
        }
    }
}