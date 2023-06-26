using GameModel.Abstract;
using Moq;
using SimplifiedSlotMachine.DataModel;
using SimplifiedSlotMachine.GameModel;
using System.Collections.Generic;

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

        [DataTestMethod]
        [DataRow(20.0, 10.0)]
        [DataRow(10.0, 0.0)]
        public void Start_CreatesStage_LoadsData(double balanceDouble, double stakeDouble)
        {
            decimal balance = (decimal)balanceDouble;
            decimal stake = (decimal)stakeDouble;
            var stage = stageModel.Start(balance, stake);

            Assert.IsNotNull(stage);
            Assert.AreEqual(balance, stage.BeginBalance);
            Assert.AreEqual(stake, stage.Stake);
            Assert.AreEqual(0M, stage.WinAmount);
            Assert.AreEqual(balance - stake, stage.EndBalance);
        }

        [TestMethod]
        public void Rotate_CallsModelRotate()
        {
            Stage stage = stageModel.Start(10, 5);
            model.Setup(m => m.Rotate(It.IsAny<Stage>())).Verifiable();

            stageModel.Rotate(stage);

            model.Verify(m => m.Rotate(It.IsAny<Stage>()), Times.Once);
        }

        [TestMethod, ExpectedException(typeof(GameException))]
        public void RecalculateStage_NoStage_GameException()
        {
            stageModel.RecalculateStage(null);

            stageModel.RecalculateStage(new Stage(0) {  Stake = 0M });
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
        public void RecalculateStage_CallsWinAmount()
        {
            Stage stage = stageModel.Start(10, 5);
            var symbols = GetWinCombination();
            stage.SpinResult = symbols;
            model.Setup(m => m.CalculateWinAmount(symbols, It.IsAny<decimal>()))
                .Returns((decimal)(6M))
                .Verifiable();

            stageModel.RecalculateStage(stage);
            model.Verify(m => m.CalculateWinAmount(symbols, It.IsAny<decimal>()), Times.Once);
        }

        [TestMethod]
        [DataRow(10.0, 22.0)]
        [DataRow(15.0, 23.0)]
        public void RecalculateStage_CalcsEndBalance(double stake, double endBalance)
        {
            Stage stage = stageModel.Start(20, (decimal)stake);
            var symbols = GetWinCombination();
            stage.SpinResult = symbols;
            model.Setup(m => m.CalculateWinAmount(symbols, (decimal)stake))
                .Returns((decimal)(1.2M * (decimal)stake))
                .Verifiable();

            stageModel.RecalculateStage(stage);

            Assert.AreEqual((decimal)stake, stage.Stake);
            Assert.AreEqual(1.2M * (decimal)stake, stage.WinAmount);
            Assert.AreEqual((decimal)endBalance, stage.EndBalance);
        }

        [TestMethod, ExpectedException(typeof(GameException))]
        public void RecalculateStage_OnException_ThrowsGameException()
        {
            Stage stage = stageModel.Start(10, 5);
            var symbols = GetWinCombination();
            stage.SpinResult = symbols;
            model.Setup(m => m.CalculateWinAmount(symbols, It.IsAny<decimal>()))
                .Throws<Exception>(() => new Exception("..."));

            stageModel.RecalculateStage(stage);
            model.Verify(m => m.CalculateWinAmount(symbols, It.IsAny<decimal>()), Times.Once);
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
