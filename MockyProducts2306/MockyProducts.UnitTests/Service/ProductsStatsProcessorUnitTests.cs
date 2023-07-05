using MockyProducts.Repository.Data;
using MockyProducts.Service.Processors;

namespace MockyProducts.UnitTests.Service
{
    [TestClass]
    public class ProductsStatsProcessorUnitTests
    {

        [TestInitialize]
        public void Inialize() { }

        [TestCleanup]
        public void Cleanup() { }

        [TestMethod]
        public async Task Stats_NoData_NoStats()
        {
            var proc = new ProductsStatsProcessor();

            var products = new List<Product>();
            var stats = await proc.Summarize(products, CancellationToken.None);
           
            Assert.IsNotNull(stats);
            Assert.IsNull(stats.TotalMinPrice);
            Assert.IsNull(stats.TotalMaxPrice);
            Assert.IsNotNull(stats.AllSizes);
            Assert.AreEqual(0, stats.AllSizes.Count);
        }

        [TestMethod]
        public async Task Stats_Data_CorrectStats()
        {
            var proc = new ProductsStatsProcessor();

            var products = GetMyProducts();
            var stats = await proc.Summarize(products, CancellationToken.None);

            Assert.IsNotNull(stats);
            Assert.AreEqual(0, stats.TotalMinPrice);
            Assert.AreEqual(10, stats.TotalMaxPrice);
            Assert.IsNotNull(stats.AllSizes);
            Assert.AreEqual(3, stats.AllSizes?.Count);
            Assert.IsTrue(stats.AllSizes?.Contains("one"));
            Assert.IsTrue(stats.AllSizes?.Contains("two"));
            Assert.IsTrue(stats.AllSizes?.Contains("three"));
            
            Assert.IsNotNull(stats.MostCommonWords);
            Assert.AreEqual(7, stats.MostCommonWords?.Count);
            Assert.AreEqual("green", stats.MostCommonWords?[0]);
            Assert.IsTrue(stats.MostCommonWords?.Take(3).Contains("P1"));
            Assert.IsTrue(stats.MostCommonWords?.Take(3).Contains("blue1"));
        }

        private IEnumerable<Product> GetMyProducts()
        {
            return new List<Product>()
            {
                new Product() { Id = 1, Title = "P1", Description = "P1 green", Price = 10, Sizes = null },
                new Product() { Id = 2, Title = "P1", Description = "P1 blue", Price = 1, Sizes = { } },
                new Product() { Id = 3, Title = "P1", Description = "P2 red", Price = 5, Sizes = new List<string> { "one" } },
                new Product() { Id = 4, Title = "P1", Description = "P3 green", Price = 0, Sizes = new List<string> { "one","two","three" } },
                new Product() { Id = 5, Title = "P2", Description = "P5 green blue", Price = null, Sizes = new List<string> { "three" } }
            };
        }
    }
}
