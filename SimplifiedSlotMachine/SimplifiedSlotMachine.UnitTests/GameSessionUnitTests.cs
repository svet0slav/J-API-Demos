using SimplifiedSlotMachine.DataModel;

namespace SimplifiedSlotMachine.UnitTests
{
    [TestClass]
    public class GameSessionUnitTests
    {
        [TestMethod]
        public void InitializeClass_Ok()
        {
            var session = new GameSession(10);

            Assert.IsNotNull(session);
            Assert.AreEqual(10, session.BeginBalance);
            Assert.AreEqual(0, session.Stake);
            Assert.AreEqual(0, session.WinAmount);
            Assert.AreEqual(10, session.EndBalance);
        }

        [TestMethod]
        public void SetStake_Ok()
        {
            var session = new GameSession(10);

            session.SetStake(1);
            
            Assert.IsNotNull(session);
            Assert.AreEqual(10, session.BeginBalance);
            Assert.AreEqual(1, session.Stake);
            Assert.AreEqual(0, session.WinAmount);
            Assert.AreEqual(9, session.EndBalance);
        }


        [TestMethod]
        public void SetWinAmount_Ok()
        {
            var session = new GameSession(200);

            session.SetStake(10);
            session.SetWinAmount(20);

            Assert.IsNotNull(session);
            Assert.AreEqual(200, session.BeginBalance);
            Assert.AreEqual(10, session.Stake);
            Assert.AreEqual(20, session.WinAmount);
            Assert.AreEqual(210, session.EndBalance);
        }

        [TestMethod]
        public void AddWinAmount_Ok()
        {
            var session = new GameSession(200);

            session.SetStake(10);
            session.SetWinAmount(20);
            session.AddWinAmount(20);

            Assert.IsNotNull(session);
            Assert.AreEqual(200, session.BeginBalance);
            Assert.AreEqual(10, session.Stake);
            Assert.AreEqual(40, session.WinAmount);
            Assert.AreEqual(230, session.EndBalance);
        }

        [TestMethod]
        public void WinAmount_Negative()
        {
            var session = new GameSession(200);

            session.SetStake(10);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => session.SetWinAmount(-20));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => session.AddWinAmount(-20));
        }
    }
}