using Castle.Core.Logging;
using Microsoft.Extensions.Logging;
using MockyProducts.Repository.Data;
using MockyProducts.Repository.Readers;
using MockyProducts.Repository.Requests;
using MockyProducts.Service;
using MockyProducts.Service.Filters;
using MockyProducts.Service.Processors;
using MockyProducts.Shared.Dto;
using MockyProducts.Shared.ServiceRequests;
using Moq;

namespace MockyProducts.UnitTests.Service
{
    [TestClass]
    public class MockyProductsServiceUnitTests
    {
        private Mock<IMockyJsonReader> _reader;
        Mock<IProductServiceFilter> _filter;
        private Mock<IProductsHighlightWordsProcessor> _highlighter;
        private Mock<IProductsStatsProcessor> _stats;
        private Mock<ILogger<MockyProductsService>> _logger;

        [TestInitialize]
        public void TestInitialize()
        {
            _reader = new Mock<IMockyJsonReader>();
            ProductsSource? myResult = new ProductsSource() { Products = new List<Product>(GetMyProducts()) };
            _reader.Setup(r => r.GetRawDataFromSource(It.IsAny<MockyRawDataParams>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(myResult));

            _filter = new Mock<IProductServiceFilter>();
            _highlighter = new Mock<IProductsHighlightWordsProcessor>();
            _stats = new Mock<IProductsStatsProcessor>();
            _logger = new Mock<ILogger<MockyProductsService>>();
        }

        [TestMethod]
        public void Service_NoFilter()
        {
            var service = new MockyProductsService(_reader.Object, _filter.Object, _highlighter.Object, _stats.Object, _logger.Object);
            ProductServiceFilterRequest? filterRequest = null;

            Assert.ThrowsExceptionAsync<ArgumentNullException>(() => service.GetProducts(filterRequest, CancellationToken.None));
        }

        [TestMethod]
        public async Task Service_NullData_FilterWorks()
        {
            IEnumerable<Product> myProducts = null;
            _reader.Setup(r => r.GetRawDataFromSource(It.IsAny<MockyRawDataParams>(), CancellationToken.None))
                .Returns(Task.FromResult((ProductsSource?)new ProductsSource() { Products = null }))
                .Verifiable();
            _filter.Setup(f => f.Filter(It.IsAny<IEnumerable<Product>>(), It.IsAny<ProductServiceFilterRequest>(), It.IsAny<CancellationToken>()))
                .Returns(myProducts)
                .Verifiable();
            var service = new MockyProductsService(_reader.Object, _filter.Object, _highlighter.Object, _stats.Object, _logger.Object);

            ProductServiceFilterRequest? filterRequest = new ProductServiceFilterRequest() { MinPrice = 10 };

            var actual = await service.GetProducts(filterRequest, CancellationToken.None);

            Assert.IsNotNull(actual);
            Assert.IsNull(actual.Products);
            //Assert.AreEqual(0, actual.Products.Count);
            _filter.Verify(f => f.Filter(It.IsAny<IEnumerable<Product>>(), It.IsAny<ProductServiceFilterRequest>(), It.IsAny<CancellationToken>()), Times.Never);
            _reader.Verify(r => r.GetRawDataFromSource(It.IsAny<MockyRawDataParams>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [TestMethod]
        public async Task Service_NoData_FilterWorks()
        {
            var myProducts = new List<Product>();
            _reader.Setup(r => r.GetRawDataFromSource(It.IsAny<MockyRawDataParams>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult((ProductsSource?)new ProductsSource() {  Products = new List<Product>()}))
                .Verifiable();
            _filter.Setup(f => f.Filter(It.IsAny<IEnumerable<Product>>(), It.IsAny<ProductServiceFilterRequest>(), It.IsAny<CancellationToken>()))
                .Returns(myProducts)
                .Verifiable();
            var service = new MockyProductsService(_reader.Object, _filter.Object, _highlighter.Object, _stats.Object, _logger.Object);

            ProductServiceFilterRequest? filterRequest = new ProductServiceFilterRequest() { MinPrice = 10 };

            var actual = await service.GetProducts(filterRequest, CancellationToken.None);

            Assert.IsNotNull(actual);
            Assert.IsNotNull(actual.Products);
            Assert.AreEqual(0, actual.Products.Count);
            _filter.Verify(f => f.Filter(It.IsAny<IEnumerable<Product>>(), It.IsAny<ProductServiceFilterRequest>(), It.IsAny<CancellationToken>()), Times.Once);
            _reader.Verify(r => r.GetRawDataFromSource(It.IsAny<MockyRawDataParams>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [TestMethod]
        public async Task Service_FilterWorks()
        {
            var myProducts = GetMyProducts();
            _reader.Setup(r => r.GetRawDataFromSource(It.IsAny<MockyRawDataParams>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult((ProductsSource?)new ProductsSource() { Products = new List<Product>(myProducts) }))
                .Verifiable();
            _filter.Setup(f => f.Filter(It.IsAny<IEnumerable<Product>>(), It.IsAny<ProductServiceFilterRequest>(), It.IsAny<CancellationToken>()))
                .Returns(new List<Product>() { myProducts.First() })
                .Verifiable();
            var service = new MockyProductsService(_reader.Object, _filter.Object, _highlighter.Object, _stats.Object, _logger.Object);

            ProductServiceFilterRequest? filterRequest = new ProductServiceFilterRequest() { MinPrice = 10 };

            var actual = await service.GetProducts(filterRequest, CancellationToken.None);

            Assert.IsNotNull(actual);
            Assert.IsNotNull(actual.Products);
            Assert.AreEqual(1, actual.Products.Count);
            _filter.Verify(f => f.Filter(It.IsAny<IEnumerable<Product>>(), It.IsAny<ProductServiceFilterRequest>(), It.IsAny<CancellationToken>()), Times.Once);
            _reader.Verify(r => r.GetRawDataFromSource(It.IsAny<MockyRawDataParams>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [TestMethod]
        public async Task Service_ProcessorWorks()
        {
            var myProducts = GetMyProducts();
            _reader.Setup(r => r.GetRawDataFromSource(It.IsAny<MockyRawDataParams>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult((ProductsSource?)new ProductsSource() { Products = new List<Product>(myProducts) }))
                .Verifiable();
            _filter.Setup(f => f.Filter(It.IsAny<IEnumerable<Product>>(), It.IsAny<ProductServiceFilterRequest>(), It.IsAny<CancellationToken>()))
                .Returns(myProducts)
                .Verifiable();
            _highlighter.Setup(p => p.Process(It.IsAny<ProductDto>()))
                .Verifiable();
            var service = new MockyProductsService(_reader.Object, _filter.Object, _highlighter.Object, _stats.Object, _logger.Object);

            ProductServiceFilterRequest? filterRequest = new ProductServiceFilterRequest() { Highlight = new List<string>() { "green" } };

            var actual = await service.GetProducts(filterRequest, CancellationToken.None);

            Assert.IsNotNull(actual);
            Assert.IsNotNull(actual.Products);
            _reader.Verify(r => r.GetRawDataFromSource(It.IsAny<MockyRawDataParams>(), It.IsAny<CancellationToken>()), Times.Once);
            _highlighter.Verify(p => p.Process(It.IsAny<ProductDto>()), Times.Exactly(myProducts.Count()));
        }

        private IEnumerable<Product> GetMyProducts()
        {
            return new List<Product>()
            {
                new Product() { Id = 1, Title = "P1", Description = "P1 green", Price = 10, Sizes = null },
                new Product() { Id = 2, Title = "P1", Description = "P1 green", Price = 1, Sizes = { } },
                new Product() { Id = 3, Title = "P1", Description = "P1 green", Price = 5, Sizes = new List<string> { "one" } },
                new Product() { Id = 4, Title = "P1", Description = "P1 green", Price = 0, Sizes = new List<string> { "one","two","three" } },
                new Product() { Id = 5, Title = "P2", Description = "P1 green", Price = null, Sizes = new List<string> { "three" } }
            };
        }
    }
}
