using MockyProducts.Repository.Data;
using MockyProducts.Service.Filters;
using MockyProducts.Shared.ServiceRequests;

namespace MockyProducts.UnitTests.Service
{
    [TestClass]
    public class ProductServiceFilterUnitTests
    {
        [TestInitialize]
        public void TestInitialize()
        {
        }

        [TestMethod]
        public void Filter_NoRequest_NoFilter()
        {
            var filter = new ProductServiceFilter();
            var products = GetMyProducts();

            var actual = filter.Filter(products, null, CancellationToken.None);

            Assert.IsNotNull(actual);
            Assert.AreEqual(products.Count(), actual.Count());
        }

        [TestMethod]
        [DataRow(1.0, null, 3)]
        [DataRow(2.0, 6.0, 1)]
        [DataRow(null, 6.0, 3)]
        [DataRow(1.0, 7.0, 2)]
        [DataRow(null, null, 5)]
        public void Filter_RequestPrice(double? minPrice, double? maxPrice, int result)
        {
            var request = new ProductServiceFilterRequest() { MinPrice = minPrice, MaxPrice = maxPrice };
            var filter = new ProductServiceFilter();
            var products = GetMyProducts();

            var actual = filter.Filter(products, request, CancellationToken.None);

            Assert.IsNotNull(actual);
            Assert.AreEqual(result, actual.Count());
        }

        [TestMethod]
        [DataRow(null, 5)]
        [DataRow("one", 2)]
        [DataRow("medium", 0)]
        [DataRow("two", 1)]
        [DataRow("three", 2)]
        public void Filter_RequestSize(string? size, int result)
        {
            var request = new ProductServiceFilterRequest() { Size = size };
            var filter = new ProductServiceFilter();
            var products = GetMyProducts();

            var actual = filter.Filter(products, request, CancellationToken.None);

            Assert.IsNotNull(actual);
            Assert.AreEqual(result, actual.Count());
        }

        [TestMethod]
        [DataRow(null, 5)]
        [DataRow("one", 5)]
        [DataRow("medium", 5)]
        [DataRow("two", 5)]
        [DataRow("one,two", 5)]
        public void Filter_RequestHighlight_NoFilter(string? highlight, int result)
        {
            var request = new ProductServiceFilterRequest() { Highlight = highlight?.Split(',')?.ToList() };
            var filter = new ProductServiceFilter();
            var products = GetMyProducts();

            var actual = filter.Filter(products, request, CancellationToken.None);

            Assert.IsNotNull(actual);
            Assert.AreEqual(result, actual.Count());
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
