using GameModel.Abstract;
using Moq;
using SimplifiedSlotMachine.DataModel;
using SimplifiedSlotMachine.GameModel;

namespace SimplifiedSlotMachine.UnitTests
{
    [TestClass]
    public class SimplifiedGameStageModelUnitTests
    {
        protected SimplifiedGameStageModel stageModel;
        protected Mock<IGameModel> model;

        [TestInitialize]
        public void Initialize()
        {
            model = new Mock<IGameModel>();
            stageModel = new SimplifiedGameStageModel(model.Object);
        }

        [TestMethod]
        public void HasStageWin_NoSymbols_False()
        {
            List<Symbol>? symbols = null;
            Assert.IsFalse(stageModel.HasStageWin(symbols));

            List<Symbol> symbols2 = new List<Symbol>();
            Assert.IsFalse(stageModel.HasStageWin(symbols2));
        }

        [TestMethod]
        public void HasStageWin_ABA_False()
        {
            List<Symbol> symbols = new List<Symbol>()
            {
                new Symbol("Apple", "A", 0.4M, 0.45M ),
                new Symbol( "Banana", "B", 0.6M, 0.35M ),
                new Symbol("Apple", "A", 0.4M, 0.45M ),
            };
            Assert.IsFalse(stageModel.HasStageWin(symbols));
        }

        [TestMethod]
        public void HasStageWin_ABW_False()
        {
            List<Symbol> symbols = new List<Symbol>()
            {
                new Symbol("Apple", "A", 0.4M, 0.45M ),
                new Symbol( "Banana", "B", 0.6M, 0.35M ),
                new Symbol( "Wildcard", "*", 0, 0.05M, true)
            };
            Assert.IsFalse(stageModel.HasStageWin(symbols));
        }

        [TestMethod]
        public void HasStageWin_AAA_True()
        {
            List<Symbol> symbols = new List<Symbol>()
            {
                new Symbol("Apple", "A", 0.4M, 0.45M ),
                new Symbol("Apple", "A", 0.4M, 0.45M ),
                new Symbol("Apple", "A", 0.4M, 0.45M ),
            };
            Assert.IsTrue(stageModel.HasStageWin(symbols));
        }

        [TestMethod]
        public void HasStageWin_AAW_True()
        {
            List<Symbol> symbols = new List<Symbol>()
            {
                new Symbol("Apple", "A", 0.4M, 0.45M ),
                new Symbol("Apple", "A", 0.4M, 0.45M ),
                new Symbol( "Wildcard", "*", 0, 0.05M, true)
            };
            Assert.IsTrue(stageModel.HasStageWin(symbols));
        }

        [TestMethod]
        public void HasStageWin_BBB_True()
        {
            List<Symbol> symbols = new List<Symbol>()
            {
                new Symbol( "Banana", "B", 0.6M, 0.35M ),
                new Symbol( "Banana", "B", 0.6M, 0.35M ),
                new Symbol( "Banana", "B", 0.6M, 0.35M ),
            };
            Assert.IsTrue(stageModel.HasStageWin(symbols));
        }

        [TestMethod]
        public void HasStageWin_BWB_True()
        {
            List<Symbol> symbols = new List<Symbol>()
            {
                new Symbol( "Banana", "B", 0.6M, 0.35M ),
                new Symbol( "Wildcard", "*", 0, 0.05M, true),
                new Symbol( "Banana", "B", 0.6M, 0.35M ),
            };
            Assert.IsTrue(stageModel.HasStageWin(symbols));
        }

        [TestMethod]
        public void HasStageWin_PPP_True()
        {
            List<Symbol> symbols = new List<Symbol>()
            {
               new Symbol("Pineapple", "P", 0.8M, 0.15M ),
               new Symbol("Pineapple", "P", 0.8M, 0.15M ),
               new Symbol("Pineapple", "P", 0.8M, 0.15M ),
            };
            Assert.IsTrue(stageModel.HasStageWin(symbols));
        }

        [TestMethod]
        public void HasStageWin_PWP_True()
        {
            List<Symbol> symbols = new List<Symbol>()
            {
                new Symbol("Pineapple", "P", 0.8M, 0.15M ),
                new Symbol( "Wildcard", "*", 0, 0.05M, true),
                new Symbol("Pineapple", "P", 0.8M, 0.15M ),
            };
            Assert.IsTrue(stageModel.HasStageWin(symbols));
        }

        [TestMethod]
        public void CalculateWinAmount_NoSymbols_Fails()
        {
            var actual = stageModel.CalculateWinAmount(new List<Symbol>(), 9M);
            Assert.AreEqual(0M, actual);

            Assert.AreEqual(0M, stageModel.CalculateWinAmount(null, 0M));
        }

        [TestMethod]
        public void CalculateWinAmount_NoStake_Zero()
        {
            List<Symbol> symbols = new List<Symbol>()
            {
                new Symbol("Pineapple", "P", 0.8M, 0.15M ),
                new Symbol( "Wildcard", "*", 0, 0.05M, true),
                new Symbol("Pineapple", "P", 0.8M, 0.15M ),
            };
            var actual = stageModel.CalculateWinAmount(symbols, 0M);
            Assert.AreEqual(0M, actual);
        }

        [TestMethod]
        public void CalculateWinAmount_NoWin_Zero()
        {
            List<Symbol> symbols = new List<Symbol>()
            {
                new Symbol("Banana", "B", 0.8M, 0.15M ),
                new Symbol( "Wildcard", "*", 0, 0.05M, true),
                new Symbol("Pineapple", "P", 0.8M, 0.15M ),
            };
            var actual = stageModel.CalculateWinAmount(symbols, 9M);
            Assert.AreEqual(0M, actual);
        }

        [TestMethod]
        public void CalculateWinAmount_WinAAA_12()
        {
            List<Symbol> symbols = new List<Symbol>()
            {
                new Symbol("Apple", "A", 0.4M, 0.45M ),
                new Symbol("Apple", "A", 0.4M, 0.45M ),
                new Symbol("Apple", "A", 0.4M, 0.45M ),
            };
            var actual = stageModel.CalculateWinAmount(symbols, 10M);
            Assert.AreEqual(12M, actual);
        }

        [TestMethod]
        public void CalculateWinAmount_WinBWB_12()
        {
            List<Symbol> symbols = new List<Symbol>()
            {
                new Symbol( "Banana", "B", 0.6M, 0.35M ),
                new Symbol( "Wildcard", "*", 0, 0.05M, true),
                new Symbol( "Banana", "B", 0.6M, 0.35M ),
            };
            var actual = stageModel.CalculateWinAmount(symbols, 10M);
            Assert.AreEqual(12M, actual);
        }

        [TestMethod]
        public void CalculateWinAmount_WinPPP_24()
        {
            List<Symbol> symbols = new List<Symbol>()
            {
                new Symbol("Pineapple", "P", 0.8M, 0.15M ),
                new Symbol("Pineapple", "P", 0.8M, 0.15M ),
                new Symbol("Pineapple", "P", 0.8M, 0.15M ),
            };
            var actual = stageModel.CalculateWinAmount(symbols, 10M);
            Assert.AreEqual(24M, actual);
        }

        [TestMethod]
        public void CalculateWinAmount_WinPPW_16()
        {
            List<Symbol> symbols = new List<Symbol>()
            {
                new Symbol("Pineapple", "P", 0.8M, 0.15M ),
                new Symbol("Pineapple", "P", 0.8M, 0.15M ),
                new Symbol( "Wildcard", "*", 0, 0.05M, true),
            };
            var actual = stageModel.CalculateWinAmount(symbols, 10M);
            Assert.AreEqual(16M, actual);
        }


        [DataTestMethod]
        [DataRow(10.0)]
        [DataRow(0.0)]
        public void Start_CreatesStage_LoadsData(double stakeDouble)
        {
            decimal stake = (decimal)stakeDouble;
            var stage = stageModel.Start(stake);

            Assert.IsNotNull(stage);
            Assert.AreEqual(stake, stage.Stake);
            Assert.IsNull(stage.SpinResult);
            Assert.AreEqual(0M, stage.WinAmount);
        }

        [TestMethod]
        public void Rotate_CallsModelSpinRotate()
        {
            var spin = new Mock<IGameSpin>();
            spin.Setup(s => s.Rotate(It.IsAny<int>())).Returns(GetWinCombination()).Verifiable();
            model.SetupGet(m => m.Spin).Returns(spin.Object).Verifiable();
            Stage stage = stageModel.Start(5);
            stageModel.Rotate(stage);

            model.Verify(m => m.Spin, Times.Once);
            spin.Verify(s => s.Rotate(It.IsAny<int>()), Times.Once);
        }

        [TestMethod, ExpectedException(typeof(GameException))]
        public void RecalculateStage_NoStage_GameException()
        {
            stageModel.RecalculateStage(null);
        }

        [TestMethod, ExpectedException(typeof(GameException))]
        public void RecalculateStage_EmptyStake_GameException()
        {
            stageModel.RecalculateStage(new Stage(0) { Stake = 0M });
        }

        [TestMethod, ExpectedException(typeof(GameException))]
        public void RecalculateStage_EmptyStage_GameException()
        {
            stageModel.RecalculateStage(new Stage(0) { Stake = 10M, SpinResult = null });
        }

        [TestMethod]
        [DataRow(10.0, 12.0)]
        [DataRow(15.0, 18.0)]
        public void RecalculateStage_CalcsEndBalance(double stake, double winAmount)
        {
            Stage stage = stageModel.Start((decimal)stake);
            var symbols = GetWinCombination();
            stage.SpinResult = symbols;

            stageModel.RecalculateStage(stage);

            Assert.AreEqual((decimal)stake, stage.Stake);
            Assert.AreEqual(1.2M * (decimal)stake, stage.WinAmount);
        }

        [TestMethod, ExpectedException(typeof(GameException))]
        public void RecalculateStage_OnException_ThrowsGameException()
        {
            Stage stage = stageModel.Start(5);
            var symbols = GetWinCombination();
            symbols.Add(null); // Inject NullReferenceException
            stage.SpinResult = symbols;

            stageModel.RecalculateStage(stage);
        }

        protected List<Symbol> GetWinCombination()
        {
            return new List<Symbol>()
            {
                new Symbol("Apple", "A", 0.4M, 0.45M ),
                new Symbol("Apple", "A", 0.4M, 0.45M ),
                new Symbol("Apple", "A", 0.4M, 0.45M ),
            };
        }


    }
}
