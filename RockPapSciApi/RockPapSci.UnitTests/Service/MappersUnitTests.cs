using RockPapSci.Data;
using RockPapSci.Service.Mappers;

namespace RockPapSci.UnitTests.Service
{
    [TestClass]
    public class MappersUnitTests
    {

        [TestMethod]
        public void ChoiceMappersExtensions_Null_Null()
        {
            ChoiceItem? choice = null;

            var actual = choice?.ToDto();

            Assert.IsNull(actual);
        }

        [TestMethod]
        public void ChoiceMappersExtensions_Data_Ok()
        {
            ChoiceItem? choice = new ChoiceItem(33, "Some", "S");

            var actual = choice?.ToDto();

            Assert.IsNotNull(actual);
            Assert.AreEqual(33, actual.Id);
            Assert.AreEqual("Some", actual.Name);
        }
    }
}
