using MockyProducts.Service.Processors;
using MockyProducts.Shared.Dto;

namespace MockyProducts.UnitTests.Service
{
    [TestClass]
    public class ProductsDtoHighlightWordsProcessorUnitTests
    {

        [TestInitialize]
        public void Inialize() { }

        [TestCleanup]
        public void Cleanup() { }

        [TestMethod]
        public void HighlightWords_NoWords_NoHighlight() {
            List<string>? words = null;
            var proc = new ProductsDtoHighlightWordsProcessor(words);

            var products = GetMyProducts();
            foreach (var product in products)
            {
                var desc = product.Description;
                proc.Process(product);

                Assert.AreEqual(desc, product.Description);
                Assert.IsFalse(product.Description?.Contains("<em>"));
            }
        }

        [TestMethod]
        public void HighlightWords_NoWords2_NoHighlight()
        {
            List<string>? words = new List<string>();
            var proc = new ProductsDtoHighlightWordsProcessor(words);

            var products = GetMyProducts();
            foreach (var product in products)
            {
                var desc = product.Description;
                proc.Process(product);

                Assert.AreEqual(desc, product.Description);
                Assert.IsFalse(product.Description?.Contains("<em>"));
            }
        }

        [TestMethod]
        [DataRow("green")]
        [DataRow("blue")]
        [DataRow("red")]
        [DataRow("blue")]
        public void HighlightWords_Word_Highlight(string word)
        {
            List<string>? words = new List<string>() { word };
            var proc = new ProductsDtoHighlightWordsProcessor(words);

            var products = GetMyProducts();
            foreach (var product in products)
            {
                var desc = (string)(product.Description ?? string.Empty);
                if (!desc.Contains(word)) continue;
                proc.Process(product);

                Assert.AreNotEqual(desc, product.Description);
                Assert.IsTrue(product.Description?.Contains("<em>"+word+"</em>"));
            }
        }

        [TestMethod]
        [DataRow("green,red", "P1 blue", "P1 blue")]
        [DataRow("blue,green,red", "P1 blue", "P1 <em>blue</em>")]
        [DataRow("blue,green,red", "P1 blue in red", "P1 <em>blue</em> in <em>red</em>")]
        public void HighlightWords_Words_Highlight(string sample, string description, string expected)
        {
            List<string>? words = new List<string>(sample?.Split(',').ToList() ?? new List<string>());
            var proc = new ProductsDtoHighlightWordsProcessor(words);

            var product = new ProductDto() { Id = 1, Title = "P1", Description = description, Price = 10, Sizes = null };
            proc.Process(product);

            Assert.AreEqual(expected, product.Description);
        }


        private IEnumerable<ProductDto> GetMyProducts()
        {
            return new List<ProductDto>()
            {
                new ProductDto() { Id = 1, Title = "P1", Description = "P1 green", Price = 10, Sizes = null },
                new ProductDto() { Id = 2, Title = "P1", Description = "P1 blue", Price = 1, Sizes = { } },
                new ProductDto() { Id = 3, Title = "P1", Description = "P2 red", Price = 5, Sizes = new List<string> { "one" } },
                new ProductDto() { Id = 4, Title = "P1", Description = "P3 green", Price = 0, Sizes = new List<string> { "one","two","three" } },
                new ProductDto() { Id = 5, Title = "P2", Description = "P5 green blue", Price = null, Sizes = new List<string> { "three" } }
            };
        }
    }
}
