using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace TaxCalc.Api.Extensions
{
    public static class ApiExtensions
    {
        public static void SetupApiVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
                options.ApiVersionReader = ApiVersionReader.Combine(
                   new QueryStringApiVersionReader("version"),
                   new HeaderApiVersionReader("X-Api-Version"),
                   new MediaTypeApiVersionReader("v"));
            });
        }
    }
}
