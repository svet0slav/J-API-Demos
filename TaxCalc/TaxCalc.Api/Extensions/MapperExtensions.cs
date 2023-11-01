using TaxCalc.Api.Mappers;

namespace TaxCalc.Api.Extensions
{
    public static class MapperExtensions
    {
        public static void ConfigureMapperServices(this IServiceCollection services)
        {
            var mapper = new MyMapperConfiguration().CreateMapperConfiguration();
            services.AddSingleton(mapper);
        }
    }
}
