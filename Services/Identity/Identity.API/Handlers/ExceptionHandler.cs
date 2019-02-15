using System.Net;
using System.Threading.Tasks;
using Identity.API.Responses;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Identity.API.Handlers
{
    public class ExceptionHandler
    {
        public async Task Invoke(HttpContext context)
        {
            HttpStatusCode httpStatus = HttpStatusCode.InternalServerError;

            var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
            if (exception != null)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)httpStatus;
                
                await context.Response.WriteAsync(JsonConvert.SerializeObject(
                    new ExceptionResponse((int)httpStatus, exception.Message, exception.StackTrace)));
            }
        }
    }
}