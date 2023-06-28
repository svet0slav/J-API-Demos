using MockyProducts.Repository.Data;
using MockyProducts.Service.Mappers;

namespace MockyProducts.UnitTests.Service
{
    [TestClass]
    public class ProductsToProductsDtoMapperUnitTests
    {
        [TestMethod]
        public void Mapper_Works()
        {
            Product product = new Product()
            {
                Id = 1,
                Title = "fdkjhfg",
                Description = " iwue iorwueio r",
                Price = 5,
                Sizes = new List<string>() { "one,two, three" }
            };

            var dto = product.ConvertToDto();

            Assert.IsNotNull(dto);
            Assert.AreEqual(product.Id, dto.Id);
            Assert.AreEqual(product.Title, dto.Title);
            Assert.AreEqual(product.Description, dto.Description);
            Assert.AreEqual(product.Price, dto.Price);
            Assert.IsFalse(product.Sizes.Equals(dto.Sizes));
        }
    }
}
