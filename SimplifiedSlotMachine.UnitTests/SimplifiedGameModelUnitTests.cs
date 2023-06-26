using Moq;
using SimplifiedSlotMachine.DataModel;
using SimplifiedSlotMachine.GameModel;
using System.Collections.Generic;

namespace SimplifiedSlotMachine.UnitTests
{
    [TestClass]
    public class SimplifiedGameModelUnitTests
    {
        protected SimplifiedGameModel gameModel;
        protected Mock<SimplifiedSpin> spin;

        [TestInitialize] 
        public void Initialize() {
            spin = new Mock<SimplifiedSpin>();
            gameModel = new SimplifiedGameModel(spin.Object);
        }

        [TestMethod]
        public void HasSymbols()
        {
            Assert.IsTrue(gameModel.Symbols.Count > 0);
        }

        [TestMethod]
        public void HasStageWin_NoSymbols_False()
        {
            List<Symbol> symbols = null;
            Assert.IsFalse(gameModel.HasStageWin(symbols));

            List<Symbol> symbols2 = new List<Symbol>();
            Assert.IsFalse(gameModel.HasStageWin(symbols2));
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
            Assert.IsFalse(gameModel.HasStageWin(symbols));
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
            Assert.IsFalse(gameModel.HasStageWin(symbols));
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
            Assert.IsTrue(gameModel.HasStageWin(symbols));
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
            Assert.IsTrue(gameModel.HasStageWin(symbols));
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
            Assert.IsTrue(gameModel.HasStageWin(symbols));
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
            Assert.IsTrue(gameModel.HasStageWin(symbols));
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
            Assert.IsTrue(gameModel.HasStageWin(symbols));
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
            Assert.IsTrue(gameModel.HasStageWin(symbols));
        }

        [TestMethod]
        public void CalculateWinAmount_NoSymbols_Fails()
        {
            var actual = gameModel.CalculateWinAmount(new List<Symbol>(), 9M);
            Assert.AreEqual(0M, actual);

            Assert.AreEqual(0M, gameModel.CalculateWinAmount(null, 0M));
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
            var actual = gameModel.CalculateWinAmount(symbols, 0M);
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
            var actual = gameModel.CalculateWinAmount(symbols, 9M);
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
            var actual = gameModel.CalculateWinAmount(symbols, 10M);
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
            var actual = gameModel.CalculateWinAmount(symbols, 10M);
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
            var actual = gameModel.CalculateWinAmount(symbols, 10M);
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
            var actual = gameModel.CalculateWinAmount(symbols, 10M);
            Assert.AreEqual(16M, actual);
        }

        [TestMethod]
        public void Rotate_CallsSpinRotate_CallsSpinRotate()
        {
            var resultSpin = new List<Symbol>()
            {
                new Symbol("Pineapple", "P", 0.8M, 0.15M ),
                new Symbol("Pineapple", "P", 0.8M, 0.15M ),
                new Symbol( "Wildcard", "*", 0, 0.05M, true),
            };
            spin.Setup((s) => s.Rotate(It.IsAny<int>())).Returns(resultSpin).Verifiable();
            var stage = new Stage(20M);

            gameModel.Rotate(stage);
            Assert.IsNotNull(stage.SpinResult);
            Assert.AreEqual(3, stage.SpinResult.Count);
            spin.Verify(s => s.Rotate(It.IsAny<int>()), Times.Once);
        }

        [TestMethod]
        public void Rotate_CallsSpinRotate_NoCalcsStage()
        {
            var resultSpin = new List<Symbol>()
            {
                new Symbol("Pineapple", "P", 0.8M, 0.15M ),
                new Symbol("Pineapple", "P", 0.8M, 0.15M ),
                new Symbol( "Wildcard", "*", 0, 0.05M, true),
            };
            spin.Setup((s) => s.Rotate(It.IsAny<int>())).Returns(resultSpin).Verifiable();
            var stage = new Stage(20M);
            stage.Stake = 10M;
            Assert.AreEqual(0M, stage.WinAmount);
            Assert.AreEqual(20M, stage.EndBalance);

            gameModel.Rotate(stage);
            Assert.AreEqual(20M, stage.EndBalance); // Must be 26M, because of stake and win, but Rotate does not recalculate the stage.
        }
    }
}
