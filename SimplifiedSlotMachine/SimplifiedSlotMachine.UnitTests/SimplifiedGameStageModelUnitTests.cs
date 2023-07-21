using GameModel.Abstract;
using Moq;
using SimplifiedSlotMachine.DataModel;
using SimplifiedSlotMachine.GameModel;
using System.Linq;

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
                new Symbol("Apple", "A", 0.4M, 0.45 ),
                new Symbol( "Banana", "B", 0.6M, 0.35 ),
                new Symbol("Apple", "A", 0.4M, 0.45 ),
            };
            Assert.IsFalse(stageModel.HasStageWin(symbols));
        }

        [TestMethod]
        public void HasStageWin_ABW_False()
        {
            List<Symbol> symbols = new List<Symbol>()
            {
                new Symbol("Apple", "A", 0.4M, 0.45 ),
                new Symbol( "Banana", "B", 0.6M, 0.35 ),
                new Symbol( "Wildcard", "*", 0, 0.05, true)
            };
            Assert.IsFalse(stageModel.HasStageWin(symbols));
        }

        [TestMethod]
        public void HasStageWin_AAA_True()
        {
            List<Symbol> symbols = new List<Symbol>()
            {
                new Symbol("Apple", "A", 0.4M, 0.45 ),
                new Symbol("Apple", "A", 0.4M, 0.45 ),
                new Symbol("Apple", "A", 0.4M, 0.45 ),
            };
            Assert.IsTrue(stageModel.HasStageWin(symbols));
        }

        [TestMethod]
        public void HasStageWin_AAW_True()
        {
            List<Symbol> symbols = new List<Symbol>()
            {
                new Symbol("Apple", "A", 0.4M, 0.45 ),
                new Symbol("Apple", "A", 0.4M, 0.45 ),
                new Symbol( "Wildcard", "*", 0, 0.05, true)
            };
            Assert.IsTrue(stageModel.HasStageWin(symbols));
        }

        [TestMethod]
        public void HasStageWin_BBB_True()
        {
            List<Symbol> symbols = new List<Symbol>()
            {
                new Symbol( "Banana", "B", 0.6M, 0.35 ),
                new Symbol( "Banana", "B", 0.6M, 0.35 ),
                new Symbol( "Banana", "B", 0.6M, 0.35 ),
            };
            Assert.IsTrue(stageModel.HasStageWin(symbols));
        }

        [TestMethod]
        public void HasStageWin_BWB_True()
        {
            List<Symbol> symbols = new List<Symbol>()
            {
                new Symbol( "Banana", "B", 0.6M, 0.35 ),
                new Symbol( "Wildcard", "*", 0, 0.05, true),
                new Symbol( "Banana", "B", 0.6M, 0.35 ),
            };
            Assert.IsTrue(stageModel.HasStageWin(symbols));
        }

        [TestMethod]
        public void HasStageWin_PPP_True()
        {
            List<Symbol> symbols = new List<Symbol>()
            {
               new Symbol("Pineapple", "P", 0.8M, 0.15 ),
               new Symbol("Pineapple", "P", 0.8M, 0.15 ),
               new Symbol("Pineapple", "P", 0.8M, 0.15 ),
            };
            Assert.IsTrue(stageModel.HasStageWin(symbols));
        }

        [TestMethod]
        public void HasStageWin_PWP_True()
        {
            List<Symbol> symbols = new List<Symbol>()
            {
                new Symbol("Pineapple", "P", 0.8M, 0.15 ),
                new Symbol( "Wildcard", "*", 0, 0.05, true),
                new Symbol("Pineapple", "P", 0.8M, 0.15 ),
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
                new Symbol("Pineapple", "P", 0.8M, 0.15 ),
                new Symbol( "Wildcard", "*", 0, 0.05, true),
                new Symbol("Pineapple", "P", 0.8M, 0.15 ),
            };
            var actual = stageModel.CalculateWinAmount(symbols, 0M);
            Assert.AreEqual(0M, actual);
        }

        [TestMethod]
        public void CalculateWinAmount_NoWin_Zero()
        {
            List<Symbol> symbols = new List<Symbol>()
            {
                new Symbol("Banana", "B", 0.8M, 0.15 ),
                new Symbol( "Wildcard", "*", 0, 0.05, true),
                new Symbol("Pineapple", "P", 0.8M, 0.15 ),
            };
            var actual = stageModel.CalculateWinAmount(symbols, 9M);
            Assert.AreEqual(0M, actual);
        }

        [TestMethod]
        public void CalculateWinAmount_WinAAA_12()
        {
            List<Symbol> symbols = new List<Symbol>()
            {
                new Symbol("Apple", "A", 0.4M, 0.45 ),
                new Symbol("Apple", "A", 0.4M, 0.45 ),
                new Symbol("Apple", "A", 0.4M, 0.45 ),
            };
            var actual = stageModel.CalculateWinAmount(symbols, 10M);
            Assert.AreEqual(12M, actual);
        }

        [TestMethod]
        public void CalculateWinAmount_WinBWB_12()
        {
            List<Symbol> symbols = new List<Symbol>()
            {
                new Symbol( "Banana", "B", 0.6M, 0.35 ),
                new Symbol( "Wildcard", "*", 0, 0.05, true),
                new Symbol( "Banana", "B", 0.6M, 0.35 ),
            };
            var actual = stageModel.CalculateWinAmount(symbols, 10M);
            Assert.AreEqual(12M, actual);
        }

        [TestMethod]
        public void CalculateWinAmount_WinPPP_24()
        {
            List<Symbol> symbols = new List<Symbol>()
            {
                new Symbol("Pineapple", "P", 0.8M, 0.15 ),
                new Symbol("Pineapple", "P", 0.8M, 0.15 ),
                new Symbol("Pineapple", "P", 0.8M, 0.15 ),
            };
            var actual = stageModel.CalculateWinAmount(symbols, 10M);
            Assert.AreEqual(24M, actual);
        }

        [TestMethod]
        public void CalculateWinAmount_WinPPW_16()
        {
            List<Symbol> symbols = new List<Symbol>()
            {
                new Symbol("Pineapple", "P", 0.8M, 0.15 ),
                new Symbol("Pineapple", "P", 0.8M, 0.15 ),
                new Symbol( "Wildcard", "*", 0, 0.05, true),
            };
            var actual = stageModel.CalculateWinAmount(symbols, 10M);
            Assert.AreEqual(16M, actual);
        }

        [DataTestMethod]
        [DataRow("*P*", 10.0, 8.0)]
        [DataRow("AAA", 10.0, 12.0)]
        [DataRow("BBB", 10.0, 18.0)]
        [DataRow("PPP", 10.0, 24.0)]
        [DataRow("ABP", 10.0, 0.0)]
        [DataRow("*AB", 10.0, 0.0)]
        [DataRow("BAA", 10.0, 0.0)]
        [DataRow("AAA", 10.0, 12.0)]
        [DataRow("A*B", 10.0, 0.0)]
        [DataRow("*AA", 10.0, 8.0)]
        // We assume that three *** is not a win combination.
        [DataRow("***", 10.0, 0.0)]
        public void CalculateWinAmount_GivenTaskSamples_Ok(string combinationString, double stakeDouble, double winDouble)
        {
            List<Symbol> symbols = new List<Symbol>(combinationString.Length);
            var chars = combinationString.ToCharArray();
            var availableSymbols = AvailableSymbols();
            for (int i=0; i < chars.Length; i++)
            {
                var symbol = availableSymbols.Single(x => x.Letter[0] == chars[i]);
                symbols.Add(symbol);
            }

            var actual = stageModel.CalculateWinAmount(symbols, (decimal)stakeDouble);

            Assert.AreEqual((decimal)winDouble, actual);
        }

        [DataTestMethod]
        [DataRow("*P*P*", 10.0, 22.60)]
        [DataRow("AAAAA", 10.0, 32.69)]
        [DataRow("BBBBB", 10.0, 42.69)]
        [DataRow("PPPPP", 10.0, 52.69)]
        [DataRow("ABPAB", 10.0, 0.0)]
        [DataRow("*AB*AB", 10.0, 0.0)]
        [DataRow("BAA*A", 10.0, 0.0)]
        [DataRow("A*B*A", 10.0, 0.0)]
        [DataRow("*AAAA", 10.0, 26.15)]
        [DataRow("BBBB*", 10.0, 31.11)]
        [DataRow("*PPPP", 10.0, 42.15)]
        public void CalculateWinAmount_RoundingData_Ok(string combinationString, double stakeDouble, double winDouble)
        {
            List<Symbol> symbols = new List<Symbol>(combinationString.Length);
            var chars = combinationString.ToCharArray();
            var availableSymbols = AvailableSymbols();
            for (int i = 0; i < chars.Length; i++)
            {
                var symbol = availableSymbols.Single(x => x.Letter[0] == chars[i]);
                symbol.Coefficient += (decimal)0.0253789 * (decimal)i;
                symbols.Add(symbol);
            }

            var actual = stageModel.CalculateWinAmount(symbols, (decimal)stakeDouble);

            Assert.AreEqual((decimal)winDouble, actual);
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
                new Symbol("Apple", "A", 0.4M, 0.45 ),
                new Symbol("Apple", "A", 0.4M, 0.45 ),
                new Symbol("Apple", "A", 0.4M, 0.45 ),
            };
        }

        protected List<Symbol> AvailableSymbols()
        {
            return new List<Symbol>()
            {
                new Symbol("Apple", "A", 0.4M, 0.45 ),
                new Symbol( "Banana", "B", 0.6M, 0.35 ),
                new Symbol("Pineapple", "P", 0.8M, 0.15 ),
                new Symbol( "Wildcard", "*", 0, 0.05, true)
            };
        }
    }
}
