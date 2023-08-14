using Microsoft.Extensions.Logging;
using RockPapSci.Service.Common;
using RockPapSci.Service.Https;
using System.Text.Json;

namespace RockPapSci.Service
{
    public class RandomGeneratorService: IRandomGeneratorService
    {
        protected ConfigReaderSettings _settings;
        protected HttpClient _httpClient;
        private readonly ILogger<RandomGeneratorService> _logger;

        public RandomGeneratorService(ConfigReaderSettings settings, HttpClient httpClient, ILogger<RandomGeneratorService> logger)
        {
            _settings = settings;
            _httpClient = httpClient;
            _logger = logger;
        }

        /// <summary>
        /// Gets the random from the online source in interval 0..N.
        /// </summary>
        /// <param name="N">The number of items</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>0..N when successful. Uses the remainder for division by N.
        ///         -1 when fails.</returns>
        /// <exception cref="Exception"></exception>
        public async Task<int> GetRandom(int N, CancellationToken cancellationToken)
        {
            if (N < 2) throw new ArgumentOutOfRangeException(nameof(N), N, "GetRandom works with count > 2.");

            try
            {
                var url = _settings.RandomUrl;

                var responseMessage = await _httpClient.GetAsync(url);

                responseMessage.EnsureSuccessStatusCode();
                _logger.LogInformation($"Reading json data for random succeeded.");

                var stream = await responseMessage.Content.ReadAsStreamAsync(cancellationToken);

                var result = await JsonSerializer.DeserializeAsync<RandomResponseObject>(stream, (JsonSerializerOptions)null, cancellationToken);

                if (result != null && result.Random.HasValue)
                {
                    _logger.LogInformation($"Parsing json data succeeded.");
                    var convertedRandom = Math.DivRem(result.Random.Value,  N).Remainder;
                    return convertedRandom;
                }
                else
                {
                    // TODO: Think over whether is needed to add content to the logger, because it is vulnerability
                    // var text = responseMessage.Content.ReadAsStringAsync(cancellationToken);
                    // _logger.LogInformation($"Parsing json data failed.", text);
                    _logger.LogError("Parsing json data failed. May be invalid format or no data.");
                    throw new ThirdServiceException("Parsing json data failed. May be invalid format or no data.");
                }
            }
            // TODO: Implementing Polly policies, you need to implement specific exceptions or wrap such methods with Polly-specific handling of exceptions.
            //  This we do to handle better the cases where third party service does not work well.
            catch (Exception ex) when (!(ex is ThirdServiceException))
            {
                var msg = $"Failed reading json data.";
                _logger.LogError(ex, msg);
                throw new ThirdServiceException(msg, ex);
            }
        }


    }
}