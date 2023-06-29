using MockyProducts.Shared.Requests;
using MockyProducts.Shared.ServiceRequests.Mappers;

namespace MockyProducts.Api.Controllers
{
    [TestClass]
    public class GetProductsRequestToServiceMapperUnitTests
    {
        [TestMethod]
        public void Mapper_Works()
        {
            var request = new GetProductsRequest() { MinPrice = "5", MaxPrice = "10", Size = "medium", Highlight = "one, two, three" };

            var actual = request.ToProductServiceFilterRequest();

            Assert.IsNotNull(actual);
            Assert.IsNotNull(actual.MinPrice);
            Assert.AreEqual(5, actual.MinPrice);
            Assert.IsNotNull(actual.MaxPrice);
            Assert.AreEqual(10, actual.MaxPrice);

            Assert.IsNotNull(actual.Size);
            Assert.AreEqual("medium", actual.Size);

            Assert.IsNotNull(actual.Highlight);
        }
    }
}
