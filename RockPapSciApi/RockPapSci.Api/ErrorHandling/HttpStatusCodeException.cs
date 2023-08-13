using System.Net;

//used from https://stackoverflow.com/questions/38630076/asp-net-core-web-api-exception-handling

namespace RockPapSci.Api.ErrorHandling
{
    public class HttpStatusCodeException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }
        public string ContentType { get; set; } = @"text/plain";

        public HttpStatusCodeException(HttpStatusCode statusCode)
        {
            this.StatusCode = statusCode;
        }

        public HttpStatusCodeException(HttpStatusCode statusCode, string message)
            : base(message)
        {
            this.StatusCode = statusCode;
        }

        public HttpStatusCodeException(HttpStatusCode statusCode, Exception inner)
            : this(statusCode, inner.ToString()) { }

        public HttpStatusCodeException(HttpStatusCode statusCode, Object? errorObject)
            : this(statusCode, errorObject?.ToString())
        {
            this.ContentType = @"application/json";
        }

    }
}
