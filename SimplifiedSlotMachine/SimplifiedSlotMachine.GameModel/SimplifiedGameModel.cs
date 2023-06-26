using GameModel.Abstract;
using SimplifiedSlotMachine.DataModel;

namespace SimplifiedSlotMachine.GameModel
{
    public class SimplifiedGameModel: IGameModel
    {
        public List<Symbol> Symbols { get; private set; }
        protected IGameSpin Spin { get; set; }

        public SimplifiedGameModel(IGameSpin spin) {
            Initialize();
        }

        protected void Initialize()
        {
            Symbols = new List<Symbol>()
            {
                new Symbol("Apple", "A", 0.4M, 0.45M ),
                new Symbol( "Banana", "B", 0.6M, 0.35M ),
                new Symbol("Pineapple", "P", 0.8M, 0.15M ),
                new Symbol( "Wildcard", "*", 0, 0.05M, true)
            };
        }

        /// <summary>
        /// Is the stage winning for the player
        /// </summary>
        /// <param name="symbols">The symbols from the Stage.</param>
        /// <returns></returns>
        public bool HasStageWin(List<Symbol> symbols)
        {
            if (symbols == null || symbols.Count == 0) return false;
            var firstSymbol = symbols.First();
            return symbols.All(s => s.Letter == firstSymbol.Letter || s.IsWildcard);
        }

        public decimal CalculateWinAmount(List<Symbol> symbols, decimal stake)
        {
            if (HasStageWin(symbols))
            {
                var sumCoefficients = symbols.Sum(s => s.Coefficient);
                return stake * sumCoefficients;
            }
            return 0M;
        }

        public virtual void Rotate(Stage stage)
        {
            var result = Spin.Rotate(3);
            stage.SpinResult = result;
        }
    }
}