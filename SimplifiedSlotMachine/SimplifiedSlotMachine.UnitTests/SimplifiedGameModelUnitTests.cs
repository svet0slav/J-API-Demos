using Moq;
using SimplifiedSlotMachine.DataModel;
using SimplifiedSlotMachine.GameModel;

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

        [DataTestMethod]
        [DataRow(20.0, 10.0)]
        [DataRow(10.0, 0.0)]
        public void StartSession_CreatesSession_LoadsData(double balanceDouble, double stakeDouble)
        {
            decimal balance = (decimal)balanceDouble;
            decimal stake = (decimal)stakeDouble;
            gameModel.StartSession(balance, stake);

            Assert.IsNotNull(gameModel.CurrentSession);
            Assert.AreEqual(balance, gameModel.CurrentSession.BeginBalance);
            Assert.AreEqual(stake, gameModel.CurrentSession.Stake);
            Assert.AreEqual(0M, gameModel.CurrentSession.WinAmount);
            Assert.AreEqual(balance - stake, gameModel.CurrentSession.EndBalance);
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

            gameModel.StartSession(20M, 10M);
            gameModel.RotateSession();

            Assert.IsNotNull(gameModel.SessionStages);
            Assert.AreEqual(gameModel.SessionStages.Count, gameModel.SessionSize);
            var stage = gameModel.SessionStages[0];
            Assert.IsNotNull(stage.SpinResult);
            Assert.AreEqual(3, stage.SpinResult.Count);
            spin.Verify(s => s.Rotate(It.IsAny<int>()), Times.Exactly(4));
        }

        [TestMethod]
        public void Rotate_CallsSpinRotate_CorrectBalanceCalcs()
        {
            var resultSpin = new List<Symbol>()
            {
                new Symbol("Pineapple", "P", 0.8M, 0.15M ),
                new Symbol("Pineapple", "P", 0.8M, 0.15M ),
                new Symbol( "Wildcard", "*", 0, 0.05M, true),
            };
            spin.Setup((s) => s.Rotate(It.IsAny<int>())).Returns(resultSpin).Verifiable();

            gameModel.StartSession(200M, 10M);
            gameModel.RotateSession();

            Assert.IsNotNull(gameModel.CurrentSession);
            Assert.AreEqual(200M, gameModel.CurrentSession.BeginBalance);
            Assert.AreEqual(10M, gameModel.CurrentSession.Stake);
            Assert.AreEqual(64M, gameModel.CurrentSession.WinAmount);
            Assert.AreEqual(254M, gameModel.CurrentSession.EndBalance);
            spin.Verify(s => s.Rotate(It.IsAny<int>()), Times.Exactly(4));
        }
    }
}
