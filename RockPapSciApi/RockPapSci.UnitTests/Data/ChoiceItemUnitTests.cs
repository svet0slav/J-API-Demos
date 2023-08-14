using RockPapSci.Data;

namespace RockPapSci.UnitTests.Data
{
    [TestClass]
    public class ChoiceItemUnitTests
    {
        [TestMethod]
        public void ChoiceItem_Construct()
        {
            ChoiceItem? choice = new ChoiceItem(1, "Rock", "R");

            Assert.IsNotNull(choice);
            Assert.AreEqual(1, choice.Id);
            Assert.IsNotNull("Rock", choice.Name);
            Assert.IsNotNull("R", choice.Letter);
        }

        [DataTestMethod]
        [DataRow(1,"Rock", "R", 1, "Rock", "R", true)]
        [DataRow(1,"Rock", "R", 1, "RockyWest", "R", true)]
        [DataRow(1,"Rock", "R", 1, "Lizard", "R", true)]
        [DataRow(1,"Rock", "R", 111, "rock", "Ro", true)]
        [DataRow(1,"Rock", "R", 111, "Rocky", "R", false)]
        [DataRow(1,"Rock", "R", 111, "rocke", "R", false)]
        public void ChoiceItem_Equals_Correct(
            int id1, string name1, string letter1, 
            int id2, string name2, string letter2, 
            bool result)
        {
            var choice1 = new ChoiceItem(id1, name1, letter1);
            var choice2= new ChoiceItem(id2, name2, letter2);

            var actual = choice1.Equals(choice2);

            Assert.AreEqual(result, actual);
        }

        [TestMethod]
        public void ChoiceItem_Equals_Null1()
        {
            ChoiceItem? choice1 = null;
            ChoiceItem? choice2 = new ChoiceItem(1, "Rock", "R");

            var actual = choice1?.Equals(choice2);

            Assert.IsNull(actual);
        }

        [TestMethod]
        public void ChoiceItem_Equals_Null2()
        {
            ChoiceItem? choice1 = new ChoiceItem(1, "Rock", "R");
            ChoiceItem? choice2 = null;

            var actual = choice1?.Equals(choice2);

            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void ChoiceItem_Equals_NullBoth()
        {
            ChoiceItem? choice1 = null;
            ChoiceItem? choice2 = null;

            var actual = choice1?.Equals(choice2);

            Assert.IsNull(actual);
        }

        [TestMethod]
        public void ChoiceItem_ToString()
        {
            ChoiceItem? choice = new ChoiceItem(1, "Rock", "R");

            Assert.IsNotNull(choice);
            Assert.AreEqual("R (1) Rock", choice.ToString());
        }

        [TestMethod]
        public void ChoiceItem_ToString_NoName()
        {
            ChoiceItem? choice = new ChoiceItem(1, null, "R");

            Assert.IsNotNull(choice);
            Assert.AreEqual("R (1) ", choice.ToString());
        }

        [TestMethod]
        public void ChoiceItem_ToString_NoNameLetter()
        {
            ChoiceItem? choice = new ChoiceItem(0, null, null);

            Assert.IsNotNull(choice);
            Assert.AreEqual(" (0) ", choice.ToString());
        }
    }
}
