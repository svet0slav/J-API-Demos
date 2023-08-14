using RockPapSci.Data;

namespace RockPapSci.UnitTests.Data
{
    [TestClass]
    public class GameModelUnitTests
    {
        [TestMethod]
        public void GameModel_Initialized()
        {
            var model = new GameModel();

            model.Initialize();

            Assert.IsNotNull(model);
            Assert.IsNotNull(model.ChoiceItems);
            Assert.IsTrue(model.ChoiceItems.Count > 1);
            Assert.IsNotNull(model.Strengths);
            Assert.IsTrue(model.Strengths.Count >= model.ChoiceItems.Count);
        }

        /// <summary>
        /// We should check here whether the strengths are initialized without conflicts.
        /// In real system, where Initialization is from configuration, this should be a method in the GameModel.
        /// </summary>
        [TestMethod]
        public void GameModel_Strengths_NoConflicts()
        {
            var model = new GameModel();
            model.Initialize();

            Assert.IsNotNull(model);
            Assert.IsNotNull(model.ChoiceItems);
            Assert.IsTrue(model.ChoiceItems.Count > 1);
            Assert.IsNotNull(model.Strengths);

            // Assert - make each two pairs of choices and find that it has rule for resolution and has no conflicts.
            foreach (var item1 in model.ChoiceItems)
            {
                foreach (var item2 in model.ChoiceItems)
                {
                    if (item1.Equals(item2)) continue;
                    var strength1 = model.Strengths.FirstOrDefault(s => s.Item1.Equals(item1) && s.Item2.Equals(item2));
                    var strength2 = model.Strengths.FirstOrDefault(s => s.Item1.Equals(item2) && s.Item2.Equals(item1));

                    Assert.IsTrue(strength1 != null || strength2 != null, "Each two choices should have definition for result");
                    Assert.IsFalse(strength1 != null && strength2 != null, "There should not be two direct conflicts for same pair of choices");
                }
            }
        }
    }
}