namespace TaxCalc.Api.Mappers
{
    public static class MyMapperExtensions
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            var mapper = new MyMapperConfiguration().CreateMapperConfiguration();
            services.AddSingleton(mapper);
        }
    }
}
