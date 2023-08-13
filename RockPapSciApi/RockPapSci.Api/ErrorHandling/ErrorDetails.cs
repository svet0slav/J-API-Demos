using System.Text.Json;

//used from https://stackoverflow.com/questions/38630076/asp-net-core-web-api-exception-handling

namespace RockPapSci.Api.ErrorHandling
{
    /// <summary>
    /// General API error response details.
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
    }
}
