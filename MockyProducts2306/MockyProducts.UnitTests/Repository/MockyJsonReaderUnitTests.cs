using MockyProducts.Repository.Readers;
using MockyProducts.Shared.Settings;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RichardSzalay.MockHttp;
using System.Net.Http.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text.Json;
using MockyProducts.UnitTests.Common;

namespace MockyProducts.UnitTests.Repository
{
    /// <summary>
    /// Test the reader.
    /// Moq handler example from https://github.com/richardszalay/mockhttp
    /// </summary>
    [TestClass]
    public class MockyJsonReaderUnitTests
    {
        private MockyJsonReader _reader;
        private MockHttpMessageHandler _mockHandler;
        private ConfigReaderSettings _settings;
        private JsonSerializerOptions _options;
        private Mock<ILogger<MockyJsonReader>> _logger;

        public MockyJsonReaderUnitTests()
        {
            _settings = FakeSettings();
            _mockHandler = new MockHttpMessageHandler();
            _logger = new Mock<ILogger<MockyJsonReader>>();
            _options = new JsonSerializerOptions();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _mockHandler = new MockHttpMessageHandler();

            //// Setup a respond for the user api (including a wildcard in the URL)
            //mockHttp.When(settings.Url)
            //        .Respond("application/json", jsonContent); // Respond with JSON

            // Inject the handler or client into your application code
            var httpClient = _mockHandler.ToHttpClient();

            _reader = new MockyJsonReader(_settings, httpClient, _options, _logger.Object);
        }

        [TestMethod, ExpectedException(typeof(Exception))]
        public async Task GetRawDataFromSource_Empty_Exception()
        {
            var jsonContent = "";
            _mockHandler.When(_settings.Url)
                    .Respond("application/json", jsonContent); // Respond with JSON

            var actual = await _reader.GetRawDataFromSource(null);
        }

        [TestMethod]
        public async Task GetRawDataFromSource_Empty_NoData()
        {
            var jsonContent = "{}";
            _mockHandler.When(_settings.Url)
                    .Respond("application/json", jsonContent); // Respond with JSON

            var actual = await _reader.GetRawDataFromSource(null);

            Assert.IsNotNull(actual);
            Assert.IsNull(actual.Products);
            Assert.IsNull(actual.ApiKeys);
        }

        [TestMethod]
        public async Task GetRawDataFromSource_SampleMockyProducts230628_1_ManyData()
        {
            var jsonContent = ReadSampleFile("SampleMockyProducts230628-1");
            _mockHandler.When(_settings.Url)
                    .Respond("application/json", jsonContent); // Respond with JSON

            var actual = await _reader.GetRawDataFromSource(null);

            Assert.IsNotNull(actual);
            Assert.IsNotNull(actual.Products);
            Assert.AreEqual(48, actual.Products.Count);
            Assert.IsTrue(actual.Products.All(p => !string.IsNullOrWhiteSpace(p.Title)));
            Assert.IsTrue(actual.Products.All(p => !string.IsNullOrWhiteSpace(p.Description)));
            Assert.IsTrue(actual.Products.All(p => p.Price > 0));
            Assert.AreEqual(42, actual.Products.Count(p => p.Sizes?.Count > 0));

            Assert.IsNotNull(actual.ApiKeys);
            Assert.IsNotNull(actual.ApiKeys.Primary);
            Assert.IsNotNull(actual.ApiKeys.Secondary);
        }

        private ConfigReaderSettings FakeSettings()
        {
            return new ConfigReaderSettings() { Url = "http://localhost/mocky" };
        }

        private string ReadSampleFile(string filename)
        {
            return CommonUnitTests.ReadSampleJsonFile("Repository\\Data", filename);
        }
    }
}
