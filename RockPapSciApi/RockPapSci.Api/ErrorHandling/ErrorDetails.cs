using System.Net;
using System.Text.Json;

//used from https://stackoverflow.com/questions/38630076/asp-net-core-web-api-exception-handling

namespace RockPapSci.Api.ErrorHandling
{
    /// <summary>
    /// General API error response details. Share the reason, but hide details.
    /// </summary>
    public class ErrorDetails
    {
        /// <summary>
        /// The status code (HTTP status codes).
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Basic message for the kind of error.
        /// </summary>
        public string? Message { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize<ErrorDetails>(this);
        }

        internal static ErrorDetails Create(int statusCode, string? message)
        {
            return new ErrorDetails { StatusCode = statusCode, Message = message };
        }

        internal static ErrorDetails Bad(string? message)
        {
            return new ErrorDetails { StatusCode = (int)HttpStatusCode.BadRequest, Message = message };
        }

        internal static object? NotFound(string name)
        {
            return new ErrorDetails { StatusCode = (int)HttpStatusCode.NotFound, Message = $"{name} not found." };
        }
    }
}
