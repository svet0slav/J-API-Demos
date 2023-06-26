using SimplifiedSlotMachine.DataModel;
using SimplifiedSlotMachine.GameModel;

namespace SimplifiedSlotMachine.UnitTests
{
    [TestClass]
    public class SimplifiedSpinUnitTests
    {
        [TestMethod]
        public void Rotate_ReturnsSymbols()
        {
            var spin = new SimplifiedSpin(AvailableSymbols());

            var actual = spin.Rotate(3);

            Assert.IsNotNull(actual);
            Assert.AreEqual(3, actual.Count());
        }

        [TestMethod]
        public void Rotate_NoAvailableSymbols_ReturnsException_()
        {
            var spin = new SimplifiedSpin(null);

            Assert.ThrowsException<NullReferenceException>(() => spin.Rotate(3));
        }

        protected List<Symbol> AvailableSymbols()
        {
            return new List<Symbol>()
            {
                new Symbol("Apple", "A", 0.4M, 0.45M ),
                new Symbol( "Banana", "B", 0.6M, 0.35M ),
                new Symbol("Pineapple", "P", 0.8M, 0.15M ),
                new Symbol( "Wildcard", "*", 0, 0.05M, true)
            };
        }
    }
}