using System.Net;

//used from https://stackoverflow.com/questions/38630076/asp-net-core-web-api-exception-handling

namespace RockPapSci.Api.ErrorHandling
{
    public class CustomExceptionMiddleware
    {
        private readonly RequestDelegate next;

        public CustomExceptionMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context /* other dependencies */)
        {
            try
            {
                await next(context);
            }
            catch (HttpStatusCodeException ex)
            {
                await HandleExceptionAsync(context, ex);
            }
            catch (Exception exceptionObj)
            {
                await HandleExceptionAsync(context, exceptionObj);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, HttpStatusCodeException exception)
        {
            string result = null;
            context.Response.ContentType = "application/json";
            if (exception is HttpStatusCodeException)
            {
                result = new ErrorDetails()
                {
                    // TODO: Could write own message and hide the original one to not share detailed information or make app vulnerable.
                    Message = exception.Message,
                    StatusCode = (int)exception.StatusCode
                }.ToString();
                context.Response.StatusCode = (int)exception.StatusCode;
            }
            //TODO: May develop other verifications for Polly-specific or library specific exceptions how to translate them to a general error response.
            // I have done this in the past, but because it is a test task I would not invest time.
            else
            {
                result = new ErrorDetails()
                {
                    Message = "Runtime Error",
                    StatusCode = (int)HttpStatusCode.BadRequest
                }.ToString();
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
            return context.Response.WriteAsync(result);
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            //TODO: May develop other verifications for Polly-specific or library specific exceptions how to translate them to a general error response.
            // I have done this in the past, but because it is a test task I would not invest time.

            //TODO: Log the original error
            //TODO: Revial the message to the log, not to the client of the API exception.Message

            string result = new ErrorDetails()
            {
                // it is not a good practice to share the details about the error. Better log the source.
                Message = "Error happened during execution",
                StatusCode = (int)HttpStatusCode.InternalServerError
            }.ToString();


            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return context.Response.WriteAsync(result);
        }
    }
}
