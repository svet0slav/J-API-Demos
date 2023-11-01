using System.Reflection;
using System.Text.Json;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using StackExchange.Redis;
using StackExchange.Redis.Configuration;
using TaxCalc.Api.ErrorHandling;
using TaxCalc.Api.Extensions;
using TaxCalc.Api.Validators;
using TaxCalc.Domain.Calculate;
using TaxCalc.Interfaces.Requests;
using TaxCalc.Service;
using TaxCalc.Services.Common.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Add Idempotency.
// Code reused from https://github.com/ikyriak/IdempotentAPI and samples.
// Register an implementation of IDistributedCache such as Memory Cache, SQL Server cache, Redis cache, etc.).
// For this example, we are using a Memory Cache.
builder.Services.AddDistributedMemoryCache(options =>
{
    options.ExpirationScanFrequency = new TimeSpan(0, 30, 0);
});
// Using Redis caching
//builder.Services.AddStackExchangeRedisCache(options =>
//{
//    options.Configuration = builder.Configuration.GetConnectionString("MyRedisConStr");
//    options.InstanceName = "MyInstance";
//});

// Setup the services, controllers, Json reading options.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.AllowTrailingCommas = true;
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        options.JsonSerializerOptions.WriteIndented = true;
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });

// Configure the validaiton.
builder.Services.AddFluentValidationAutoValidation();
// Manually setup your validators
builder.Services.AddTransient<IValidator<TaxPayerRequest>, TaxPayerRequestValidator>();

builder.Services.AddApiVersioning();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.SwaggerConfiguration();

// Configurations.

builder.AddTaxJurisdictionConfig("TaxJurisdictionConfiguration");

// Set the JSON serializer options.
builder.Services.Configure<JsonSerializerOptions>(options =>
{
    options.PropertyNameCaseInsensitive = true;
    options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.WriteIndented = true;
    options.AllowTrailingCommas = true;
});

// Register the interfaces.
builder.Services.AddTransient<ITaxesCalculator, TaxesCalculator>();
builder.Services.AddTransient<ITaxesCalculator, TaxesCalculator>();
builder.Services.AddTransient<ICalculatorService, CalculatorService>();
builder.Services.ConfigureMapperServices();

builder.Services.SetupApiVersioning();

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
        // c.SwaggerEndpoint("v1/swagger.json", "My API V1");
        // c.RoutePrefix = string.Empty;
    });
    app.UseSwaggerUI();
}

// Exception handling
app.ConfigureCustomExceptionMiddleware();

app.UseApiVersioning();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
