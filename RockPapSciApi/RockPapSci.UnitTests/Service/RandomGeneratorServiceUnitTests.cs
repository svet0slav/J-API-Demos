using Microsoft.Extensions.Logging;
using Moq;
using RichardSzalay.MockHttp;
using RockPapSci.Service;
using RockPapSci.Service.Common;
using RockPapSci.UnitTests.Common;
using System.Net;

namespace RockPapSci.UnitTests.Service
{
    /// <summary>
    /// Test the reader.
    /// Moq handler example from https://github.com/richardszalay/mockhttp
    /// </summary>
    [TestClass]
    public class RandomGeneratorServiceUnitTests
    {
        private RandomGeneratorService _generatorService;
        private MockHttpMessageHandler _mockHandler;
        private ConfigReaderSettings _settings;
        private Mock<ILogger<RandomGeneratorService>> _logger;

        [TestInitialize]
        public void TestInitialize()
        {
            _settings = FakeSettings();
            _logger = new Mock<ILogger<RandomGeneratorService>>();

            // Inject the handler or client into your application code
            _mockHandler = new MockHttpMessageHandler();
            var httpClient = _mockHandler.ToHttpClient();

            _generatorService = new RandomGeneratorService(_settings, httpClient, _logger.Object);
        }

        [DataTestMethod, ExpectedException(typeof(ArgumentOutOfRangeException))]
        [DataRow(-1)]
        [DataRow(0)]
        [DataRow(1)]
        public async Task GetRandom_EmptyContent_Exception(int n)
        {
            var jsonContent = SampleJsonContent();
            _mockHandler.When(_settings.RandomUrl)
                    .Respond("application/json", jsonContent); // Respond with JSON

            var actual = await _generatorService.GetRandom(n, CancellationToken.None);
        }
        
        [TestMethod, ExpectedException(typeof(ThirdServiceException))]
        public async Task GetRandom_EmptyContent_Exception()
        {
            var jsonContent = "";
            _mockHandler.When(_settings.RandomUrl)
                    .Respond("application/json", jsonContent); // Respond with JSON

            var actual = await _generatorService.GetRandom(5, CancellationToken.None);
        }

        [TestMethod, ExpectedException(typeof(ThirdServiceException))]
        public async Task GetRandom_EmptyObject_NoData()
        {
            var jsonContent = "{}";
            _mockHandler.When(_settings.RandomUrl)
                    .Respond("application/json", jsonContent); // Respond with JSON

            var actual = await _generatorService.GetRandom(5, CancellationToken.None);

            Assert.IsNotNull(actual);
        }

        [DataTestMethod]
        [DataRow(2)]
        [DataRow(4)]
        [DataRow(5)]
        [DataRow(10)]
        [DataRow(256)]
        public async Task GetRandom_SampleJson_Ok(int count)
        {
            var jsonContent = SampleJsonContent();
            _mockHandler.When(_settings.RandomUrl)
                    .Respond("application/json", jsonContent); // Respond with JSON

            var actual = await _generatorService.GetRandom(count, CancellationToken.None);

            Assert.IsTrue(actual >= 0);
            Assert.IsTrue(actual < count);

            //_logger.Verify(l => l.LogInformation(It.IsAny<string>()), Times.Exactly(2));
            // _logger.Verify(l => l.LogError(It.IsAny<Exception>(), It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public async Task GetRandom_RandomGenerator_Oleg_v189_CalculatesCorrectly()
        {
            int count = 5;
            var jsonContent = ReadSampleFile("RandomGenerator_Oleg_v189");
            _mockHandler.When(_settings.RandomUrl)
                    .Respond("application/json", jsonContent); // Respond with JSON

            var actual = await _generatorService.GetRandom(count, CancellationToken.None);

            Assert.AreEqual(4, actual);

            // _logger.Verify(l => l.LogInformation(It.IsAny<string>()), Times.Exactly(2));
            // _logger.Verify(l => l.LogError(It.IsAny<Exception>(), It.IsAny<string>()), Times.Never);
        }

        [DataTestMethod]
        [DataRow(HttpStatusCode.InternalServerError)]
        [DataRow(501)]
        [DataRow(502)]
        [DataRow(503)]
        [DataRow(504)]
        [DataRow(505)]
        [DataRow(506)]
        [DataRow(507)]
        public void GetRandom_RandomGenerator_Oleg_v189_Http500_GetError(int statusCode)
        {
            int count = 5;
            var jsonContent = ReadSampleFile("RandomGenerator_Oleg_v189");
            _mockHandler.When(_settings.RandomUrl)
                .Respond(req => new HttpResponseMessage((HttpStatusCode)statusCode)); // Respond with Http 500

            var ex = Assert.ThrowsExceptionAsync<ThirdServiceException>(() => _generatorService.GetRandom(count, CancellationToken.None));

            //_logger.Verify(l => l.LogInformation(It.IsAny<string>()), Times.Exactly(2));
            //_logger.Verify(l => l.LogError(It.IsAny<string>(), It.IsAny<Exception>()), Times.Once);

            Assert.IsNotNull(ex);
            Assert.IsNotNull(ex.Result);
            Assert.IsNotNull(ex.Result.InnerException);
            Assert.AreEqual("Failed reading json data.", ex.Result.Message);
        }

        [DataTestMethod]
        [DataRow(HttpStatusCode.BadRequest)]
        [DataRow(HttpStatusCode.Unauthorized)]
        [DataRow(HttpStatusCode.PaymentRequired)]
        [DataRow(HttpStatusCode.Forbidden)]
        [DataRow(HttpStatusCode.NotFound)]
        [DataRow(HttpStatusCode.MethodNotAllowed)]
        [DataRow(HttpStatusCode.NotAcceptable)]
        [DataRow(HttpStatusCode.ProxyAuthenticationRequired)]
        [DataRow(HttpStatusCode.RequestTimeout)]
        [DataRow(HttpStatusCode.Conflict)]
        [DataRow(HttpStatusCode.Gone)]
        public void GetRandom_RandomGenerator_Oleg_v189_Http400_GetError(int statusCode)
        {
            int count = 5;
            var jsonContent = ReadSampleFile("RandomGenerator_Oleg_v189");
            _mockHandler.When(_settings.RandomUrl)
                .Respond(req => new HttpResponseMessage((HttpStatusCode)statusCode)); // Respond with Http 400+ error

            var ex = Assert.ThrowsExceptionAsync<ThirdServiceException>(() => _generatorService.GetRandom(count, CancellationToken.None));

            //_logger.Verify(l => l.LogInformation(It.IsAny<string>()), Times.Exactly(2));
            //_logger.Verify(l => l.LogError(It.IsAny<string>(), It.IsAny<Exception>()), Times.Once);

            Assert.IsNotNull(ex);
            Assert.IsNotNull(ex.Result);
            Assert.IsNotNull(ex.Result.InnerException);
            Assert.AreEqual("Failed reading json data.", ex.Result.Message);
        }

        private ConfigReaderSettings FakeSettings()
        {
            return new ConfigReaderSettings() { RandomUrl = "http://localhost/fakeurl", TimeoutSeconds = 20 };
        }

        private string SampleJsonContent()
        {
            return "{\r\n    \"random\": 15\r\n}";
        }

        private string ReadSampleFile(string filename)
        {
            return CommonUnitTests.ReadSampleJsonFile("Samples\\RandomGeneratorService", filename);
        }
    }
}
