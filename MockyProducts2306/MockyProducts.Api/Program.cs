using MockyProducts.Repository.Readers;
using MockyProducts.Service;
using MockyProducts.Service.Filters;
using MockyProducts.Service.Processors;
using MockyProducts.Shared.Services;
using MockyProducts.Shared.Settings;
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc;
using MockyProducts.Api.ErrorHandling;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Setup the services, controllers, Json reading options.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.AllowTrailingCommas = true;
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        options.JsonSerializerOptions.WriteIndented = true;
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });

builder.Services.AddApiVersioning();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// Register the Swagger generator, defining 1 or more Swagger documents
// builder.Services.AddSwaggerGen();
// used from https://stackoverflow.com/questions/56859604/swagger-not-loading-failed-to-load-api-definition-fetch-error-undefined
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API MockyProducts", Version = "v1" });
    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First()); //This line
});

var configSettings = builder.Configuration.GetSection("ConfigReaderSettings").Get<ConfigReaderSettings>();
if (configSettings == null) throw new Exception("The application is not configured properly with ConfigReaderSettings");
builder.Services.AddScoped<ConfigReaderSettings>((provider) => configSettings);

builder.Services.AddHttpClient<IMockyJsonReader>(client =>
{
    client.BaseAddress = new Uri(configSettings?.Url ?? string.Empty);
    client.Timeout = TimeSpan.FromSeconds(configSettings?.TimeoutSeconds ?? 120);
    client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue
    {
        MaxAge = TimeSpan.FromSeconds(360),
        NoTransform = true
    };
});

// Set the JSON serializer options.
builder.Services.Configure<JsonSerializerOptions>(options =>
{
    options.PropertyNameCaseInsensitive = true;
    options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.WriteIndented = true;
    options.AllowTrailingCommas = true;
});

// Register the interfaces.
builder.Services.AddScoped(typeof(IMockyJsonReader), typeof(MockyJsonReader));
builder.Services.AddTransient<IProductServiceFilter, ProductServiceFilter>();
builder.Services.AddTransient<IProductsDtoHighlightWordsProcessor, ProductsDtoHighlightWordsProcessor>();
builder.Services.AddTransient<IProductsDtoProcessor, ProductsDtoHighlightWordsProcessor>();
builder.Services.AddScoped<IMockyProductsService, MockyProductsService>();

builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
       new HeaderApiVersionReader("X-Api-Version"),
       new MediaTypeApiVersionReader("v"),
       new QueryStringApiVersionReader("version"));
});

// The Microsoft.Extensions.Logging package provides this one-liner to add logging services.
builder.Services.AddLogging();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Enable middleware to serve generated Swagger as a JSON endpoint.
    app.UseSwagger();
    // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
    // used from https://stackoverflow.com/questions/56859604/swagger-not-loading-failed-to-load-api-definition-fetch-error-undefined
    app.UseSwaggerUI(c =>
    {
        //c.SwaggerEndpoint("v1/swagger.json", "My API V1");
        //c.RoutePrefix = string.Empty;
    });
    app.UseDeveloperExceptionPage();
}
app.ConfigureCustomExceptionMiddleware();
app.UseApiVersioning();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseSwagger();

app.Run();
