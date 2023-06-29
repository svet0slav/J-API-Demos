using Microsoft.Extensions.Logging;
using MockyProducts.Shared.Services;
using MockyProducts.Controllers;
using MockyProducts.Shared.Dto;
using Moq;
using MockyProducts.Shared.ServiceRequests;
using MockyProducts.Shared.Requests;

namespace MockyProducts.UnitTests.Controllers
{
    [TestClass]
    public class ProductsControllerUnitTests
    {
        private Mock<IMockyProductsService> _service;

        private readonly Mock<ILogger<ProductsController>> _logger;

        public ProductsControllerUnitTests() { 
            _logger = new Mock<ILogger<ProductsController>>();
        }

        [TestInitialize]
        public void Setup()
        {
            _service = new Mock<IMockyProductsService>();
        }

        [TestMethod]
        public async Task GetProducts_ReturnsExpectedResult() {
            var getRequest = new GetProductsRequest();
            var filterRequest = new ProductServiceFilterRequest() { };
            _service.Setup(s => s.GetProducts(It.IsAny<ProductServiceFilterRequest>()))
                .Returns(Task.FromResult(
                    new ProductsDto() { Products = new List<ProductDto>() }
                    ));

            var controller = new ProductsController(_service.Object, _logger.Object);
            var actual = await controller.Get(getRequest);

            Assert.IsNotNull(actual);
            Assert.IsNotNull(actual.Products);
        }
    }
}
