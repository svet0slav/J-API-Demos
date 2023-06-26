using SimplifiedSlotMachine.DataModel;
using SimplifiedSlotMachine.GameModel;
using System.Threading.Tasks.Dataflow;

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

        [TestMethod]
        public void Rotate_AvailableSymbols_CloseToProbabilities()
        {
            var spin = new SimplifiedSpin(AvailableSymbols());

            var prob = new List<Tuple<Symbol, double>>(spin.AvailableSymbols.Count);
            spin.AvailableSymbols.ForEach(x => prob.Add(new Tuple<Symbol, double>(x, 0.0)));

            double totalCount = 0;
            for(int i=0; i < 30000; i++)
            {
                var result = spin.Rotate(3);
                totalCount += result.Count;
                result.ForEach(r =>
                {
                    var found = prob.Single(p => r.Letter == p.Item1.Letter);
                    prob.Remove(found);
                    prob.Add(new Tuple<Symbol, double>(found.Item1, found.Item2 + 1));
                });
            }

            foreach(var p in prob)
            {
                var actualProbability = (p.Item2 / totalCount);
                var delta = Math.Abs((double)p.Item1.Probability - actualProbability);
                Assert.IsTrue(delta < 0.016, $"Delta is different ({actualProbability} -> {p.Item1.Probability}) than normal for symbol {p.Item1.Letter}");
            }
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