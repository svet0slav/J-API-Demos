using Microsoft.Extensions.Logging;
using MockyProducts.Repository.Data;
using MockyProducts.Repository.Requests;
using MockyProducts.Shared.Settings;
using System.Text.Json;

namespace MockyProducts.Repository.Readers
{
    /// <summary>
    /// Class to read date from the given data source
    /// </summary>
    public class MockyJsonReader : IMockyJsonReader
    {
        protected ConfigReaderSettings _settings;
        protected HttpClient _httpClient;
        protected JsonSerializerOptions _options;
        private readonly ILogger<MockyJsonReader> _logger;

        public MockyJsonReader(ConfigReaderSettings settings, HttpClient httpClient, JsonSerializerOptions options, ILogger<MockyJsonReader> logger)
        {
            _settings = settings;
            _httpClient = httpClient;
            _options = options;
            _logger = logger;
        }

        public async Task<ProductsSource?> GetRawDataFromSource(MockyRawDataParams param)
        {
            try
            {
                var jsonProductData = string.Empty;

                var url = _settings.Url;

                // TODO: Enable url with addtional params from MockyRawDataParams param

                var responseMessage = await _httpClient.GetAsync(_settings.Url);

                responseMessage.EnsureSuccessStatusCode();
                _logger.LogInformation($"Reading json data succeeded.");

                var stream = await responseMessage.Content.ReadAsStreamAsync();

                var result = await JsonSerializer.DeserializeAsync<ProductsSource>(stream, _options);

                if (result != null)
                {
                    _logger.LogInformation($"Parsing json data succeeded {result?.Products?.Count}.");
                    return result;
                }
                else
                {
                    _logger.LogInformation($"Parsing json data failed.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                var msg = $"Failed reading json data.";
                _logger.LogError(ex, msg);
                throw new Exception(msg, ex);
            }
        }
    }
}
