using Moq;
using RockPapSci.Data;
using RockPapSci.Data.Interfaces;

namespace RockPapSci.UnitTests.Data
{
    [TestClass]
    public class GameModelRulesUnitTests
    {
        private Mock<IGameModel> _gameModel;

        public GameModelRulesUnitTests()
        {
            _gameModel = new Mock<IGameModel>();

            var testModel = FakeGameModel();
            testModel.Initialize();

            _gameModel.SetupGet(m => m.ChoiceItems).Returns(testModel.ChoiceItems);
            _gameModel.SetupGet(m => m.Strengths).Returns(testModel.Strengths);
        }

        [DataTestMethod]
        // Scissors cuts paper.
        [DataRow("S", "P")]
        // Paper covers rock.
        [DataRow("P", "R")]
        // Rock crushes lizard.
        [DataRow("R", "L")]
        // Lizard poisons Spock.
        [DataRow("L", "K")]
        // Spock smashes scissors.
        [DataRow("K", "S")]
        // Scissors decapitates lizard.
        [DataRow("S", "L")]
        // Lizard eats paper.
        [DataRow("L", "P")]
        // Paper disproves Spock.
        [DataRow("P", "K")]
        // Spock vaporizes rock.
        [DataRow("K", "R")]
        //Rock crushes scissors.
        [DataRow("R", "S")]
        public void Rules_Work_Correctly(string symbol1, string symbol2)
        {
            Assert.IsTrue(_gameModel.Object.ChoiceItems.Count > 1, "The test should be loaded with choice items");

            var choice1 = _gameModel.Object.ChoiceItems.First(x => x.Letter == symbol1);
            var choice2 = _gameModel.Object.ChoiceItems.First(x => x.Letter == symbol2);
            var rules = new GameModelRules(_gameModel.Object);

            var result = rules.GetWinner(choice1, choice2);
            var resultReverse = rules.GetWinner(choice2, choice1);

            Assert.AreEqual(WinnerResult.FirstWins, result);
            Assert.AreEqual(WinnerResult.SecondWins, resultReverse);
        }

        [TestMethod]
        public void Rules_EqualsWork_Correctly()
        {
            Assert.IsTrue(_gameModel.Object.ChoiceItems.Count > 1, "The test should be loaded with choice items");

            var rules = new GameModelRules(_gameModel.Object);
            foreach (var choice in _gameModel.Object.ChoiceItems)
            {
                var result = rules.GetWinner(choice, choice);
                Assert.AreEqual(WinnerResult.Equal, result);
            }
        }

        [TestMethod]
        public void Rules_NullWork_NA()
        {
            Assert.IsTrue(_gameModel.Object.ChoiceItems.Count > 1, "The test should be loaded with choice items");

            var rules = new GameModelRules(_gameModel.Object);
            var result = rules.GetWinner(null, null);
            Assert.AreEqual(WinnerResult.NotAvailable, result);
        }

        [TestMethod]
        public void Rules_NAWork_NA()
        {
            Assert.IsTrue(_gameModel.Object.ChoiceItems.Count > 1, "The test should be loaded with choice items");
            Assert.IsTrue(_gameModel.Object.Strengths.Count > 1, "The test should be loaded with strengths rules");

            var rules = new GameModelRules(_gameModel.Object);
            var last = _gameModel.Object.Strengths.Last();
            _gameModel.Object.Strengths.Remove(last);

            var result = rules.GetWinner(last.Item1, last.Item2);
            Assert.AreEqual(WinnerResult.NotAvailable, result);
        }

        private IGameModel FakeGameModel()
        {
            var model = new GameModel();
            model.Initialize();
            return model;
        }
    }
}
