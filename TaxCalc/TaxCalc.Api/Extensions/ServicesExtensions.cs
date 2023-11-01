using Microsoft.OpenApi.Models;

namespace TaxCalc.Api.Extensions
{
    public static class ServicesExtensions
    {
        /// <summary>
        /// Setup Swagger Configuration. Register the Swagger generator, defining 1 or more Swagger documents.
        /// </summary>
        /// <param name="services"></param>
        public static void SwaggerConfiguration(this IServiceCollection services)
        {
            // used from https://stackoverflow.com/questions/56859604/swagger-not-loading-failed-to-load-api-definition-fetch-error-undefined
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API TaxCalc", Version = "v1" });
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            });
        }
    }
}
