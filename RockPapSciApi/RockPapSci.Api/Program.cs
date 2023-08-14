using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using RockPapSci.Api.Polly;
using RockPapSci.Api.ErrorHandling;
using RockPapSci.Service.Common;
using System.Net.Http.Headers;
using System.Text.Json;
using RockPapSci.Service;
using RockPapSci.Data.Interfaces;
using RockPapSci.Data;

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
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API RockPaperScissor", Version = "v1" });
    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First()); //This line
});

var configSettings = builder.Configuration.GetSection("ConfigReaderSettings").Get<ConfigReaderSettings>();
if (configSettings == null) throw new Exception("The application is not configured properly with ConfigReaderSettings");
builder.Services.AddScoped<ConfigReaderSettings>((provider) => configSettings);

// Define the Http Handler and the Polly Policies

using var pollyLoggerFactory = LoggerFactory.Create(builder =>
{
    builder.SetMinimumLevel(LogLevel.Information);
    builder.AddEventSourceLogger();
});
var loggerPolicies = pollyLoggerFactory.CreateLogger<PollyPolicies>();

var pollyPolicies = new PollyPolicies(loggerPolicies);

builder.Services.AddHttpClient<IRandomGeneratorService>(client =>
{
    client.BaseAddress = new Uri(configSettings?.RandomUrl ?? string.Empty);
    client.Timeout = TimeSpan.FromSeconds(configSettings?.TimeoutSeconds ?? 120);
    client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue
    {
        MaxAge = TimeSpan.FromSeconds(360),
        NoTransform = true
    };
}).AddPolicyHandler(pollyPolicies.RetryPolicy())
.AddPolicyHandler(pollyPolicies.CircuitBreakerPolicy());

// Set the JSON serializer options.
builder.Services.Configure<JsonSerializerOptions>(options =>
{
    options.PropertyNameCaseInsensitive = true;
    options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.WriteIndented = true;
    options.AllowTrailingCommas = true;
});

// Register the interfaces.
builder.Services.AddScoped(typeof(IRandomGeneratorService), typeof(RandomGeneratorService));
builder.Services.AddScoped<IGameModel, GameModel>(provider =>
{
    var model = new GameModel();
    model.Initialize();
    return model;
});

builder.Services.AddScoped<IGameModelRules, GameModelRules>();
builder.Services.AddTransient<IGameService, GameService>();

builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
       new QueryStringApiVersionReader("version"),
       new HeaderApiVersionReader("X-Api-Version"),
       new MediaTypeApiVersionReader("v"));
});

// The Microsoft.Extensions.Logging package provides this one-liner to add logging services.
builder.Services.AddLogging();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
    app.UseSwaggerUI();
}

app.ConfigureCustomExceptionMiddleware();
app.UseApiVersioning();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseSwagger();

app.Run();
