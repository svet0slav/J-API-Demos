using RockPapSci.Dtos.Choices;

namespace RockPapSci.UnitTests.Dtos
{
    [TestClass]
    public class ChoiceDtoUnitTests
    {

        [TestMethod]
        public void ChoiceItem_ToString()
        {
            var choice = new ChoiceDto() { Id = 1, Name = "Rock" };

            Assert.IsNotNull(choice);
            Assert.AreEqual("R (1) Rock", choice.ToString());
        }

        [TestMethod]
        public void ChoiceItem_ToString_NoName()
        {
            var choice = new ChoiceDto() { Id = 1, Name = null };

            Assert.IsNotNull(choice);
            Assert.AreEqual(" (1) ", choice.ToString());
        }

        [TestMethod]
        public void ChoiceItem_ToString_NoData()
        {
            var choice = new ChoiceDto();

            Assert.IsNotNull(choice);
            Assert.AreEqual(" (0) ", choice.ToString());
        }
    }
}
