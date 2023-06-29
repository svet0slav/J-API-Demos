using System.Text.Json;

//used from https://stackoverflow.com/questions/38630076/asp-net-core-web-api-exception-handling

namespace MockyProducts.Api.ErrorHandling
{
    public class ErrorDetails
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize<ErrorDetails>(this);
        }
    }
}
