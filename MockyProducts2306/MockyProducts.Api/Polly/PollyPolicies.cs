using Polly;
using Polly.CircuitBreaker;
using Polly.Extensions.Http;
using Polly.Retry;

namespace MockyProducts.Api.Polly
{
    // Code adapted from https://github.com/App-vNext/Polly/wiki/Polly-and-HttpClientFactory

    // Make it a class that returns policies, because it could be unit tested. I did this in a real project.

    public class PollyPolicies
    {

        private ILogger<PollyPolicies> _logger;

        public PollyPolicies(/* TODO: Configuration settings */ ILogger<PollyPolicies> logger)
        {
            _logger = logger;
        }

        public AsyncRetryPolicy<HttpResponseMessage> RetryPolicy()
        {
            // May generate durations from configurations settings
            var durations = new List<TimeSpan>();
            var count = 3;
            var initial = 5; //seconds
            var increase = 5; // seconds
            durations.Add(TimeSpan.FromSeconds(initial));
            for (int i = 1; i < count; i++)
            {
                durations.Add(TimeSpan.FromSeconds(initial + i * increase));
            }

            var policy = HttpPolicyExtensions
                .HandleTransientHttpError() // HttpRequestException, 5XX and 408
                .OrResult(response => (int)response.StatusCode == 429) // RetryAfter
                .WaitAndRetryAsync(durations,
                (result, time) =>
                {
                    _logger.LogInformation($"Fafiled with {result.Result.StatusCode} retrying ");
                });

            return policy;
        }

        public AsyncCircuitBreakerPolicy<HttpResponseMessage> CircuitBreakerPolicy()
        {
            // May generate durations from configurations settings
            var failureCount = 5;
            var waitDuration = TimeSpan.FromSeconds(30);

            var policy = HttpPolicyExtensions
                .HandleTransientHttpError() // HttpRequestException, 5XX and 408
                .OrResult(response => (int)response.StatusCode == 408) // May do it with a list of status codes.
                .CircuitBreakerAsync(failureCount, waitDuration,
                (result, time) =>
                {
                    _logger.LogWarning("Breaking request ...");
                },
                () =>
                {
                    _logger.LogInformation($"Recovered service.");
                });

            return policy;
        }
    }
}