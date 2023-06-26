using GameModel.Abstract;
using SimplifiedSlotMachine.DataModel;

namespace SimplifiedSlotMachine.GameModel
{
    public class SimplifiedSpin : IGameSpin
    {
        protected Random random = new Random();

        public List<Symbol> AvailableSymbols {  get; set; }

        public SimplifiedSpin(List<Symbol> availableSymbols) {
            AvailableSymbols = availableSymbols;

            // To rely on random numbers, must rotate random generator first.
            var rotateSpin = 10 + DateTime.Now.Second;
            for (int i = 0; i < rotateSpin; i++)
            {
                random.Next(0, 100);
            }
        }

        public virtual List<Symbol> Rotate(int outputCount)
        {
            var result = new List<Symbol>();
            for (int i = 0; i < outputCount; i++)
            {
                decimal coeff = random.Next(0, 100) / 100;
                Symbol? selected = null;
                foreach (Symbol item in AvailableSymbols)
                {
                    if (coeff < item.Coefficient)   // Coefficient is between 0 and 99.
                    {
                        selected = item;
                        break;
                    }
                    coeff -= item.Coefficient;
                }
                if (selected != null)
                {
                    result.Add(selected);
                }
                else
                {
                    // This case may be the generator is not well or sum of coefficients does not cover 100%.
                    // It is not recommended to return * wildcard.
                    // Best to retry and 
                    // TODO: protect agains freezing this algorithm.
                    i--;
                }
            }

            return result;
        }

    }
}
