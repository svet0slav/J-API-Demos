using TaxCalc.Domain.Common.Config;

namespace TaxCalc.Api.Extensions
{
    public static class ConfigExtensions
    {
        public static void AddTaxJurisdictionConfig(this WebApplicationBuilder builder, string sectionName)
        {
            var configSection = builder.Configuration.GetSection(sectionName);
            var configSettings = configSection.Get<TaxJurisdictionConfiguration>();

            if (configSettings == null)
                throw new InvalidDataException($"The application is not configured properly with section {sectionName}.");

            builder.Services.AddScoped<ITaxJurisdictionConfiguration>((provider) => configSettings);

        }
    }
}
