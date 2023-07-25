using GameModel.Abstract;
using SimplifiedSlotMachine.DataModel;

namespace SimplifiedSlotMachine.GameModel
{
    public class SimplifiedSpin : IGameSpin
    {
        private readonly IGameRandomNumberGenerator _spinRotator;

        public List<Symbol> AvailableSymbols {  get; set; }

        public SimplifiedSpin(List<Symbol> availableSymbols, IGameRandomNumberGenerator spinRotator) {
            if (availableSymbols == null || availableSymbols.Count == 0)
            {
                throw new ArgumentNullException(nameof(availableSymbols));
            }
            AvailableSymbols = availableSymbols;
            _spinRotator = spinRotator;
        }

        public virtual List<Symbol> Rotate(int outputCount)
        {
            int antiFreezeCounter = 0;

            var result = new List<Symbol>();
            for (int i = 0; i < outputCount; i++)
            {
                double randomProbability = _spinRotator.GetRandom();
                
                Symbol? selected = null;
                foreach (Symbol item in AvailableSymbols)
                {
                    if (randomProbability < item.Probability)   // Probability is between 0.00 and 1.00, excluding 1.00
                    {
                        selected = item;
                        break;
                    }
                    randomProbability -= item.Probability;
                }
                if (selected != null)
                {
                    result.Add(selected);
                }
                else
                {
                    // This case may be the generator is not well or sum of coefficients does not cover 100%.
                    // It is not recommended to return * wildcard.
                    // Best retry the chance to find a symbol.
                    antiFreezeCounter++;
                    if (antiFreezeCounter > 100)
                    {
                        result.Add(AvailableSymbols.First());
                    }
                    else
                    {
                        i--;
                    }
                }
            }

            return result;
        }

    }
}
