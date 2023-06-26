using GameModel.Abstract;
using SimplifiedSlotMachine.DataModel;

namespace SimplifiedSlotMachine.GameModel
{
    public class SimplifiedGameStageModel : IGameStageModel
    {
        protected IGameModel Model { get; private set; }

        public SimplifiedGameStageModel(IGameModel model)
        {
            Model = model;
        }

        public Stage Start(decimal stake)
        {
            var result = new Stage(stake);
            result.Stake = stake;
            result.WinAmount = 0;
            result.SpinResult = null;
            return result;
        }

        /// <summary>
        /// Rotate the spin and set results for given stage.
        /// </summary>
        /// <param name="stage"></param>
        /// <param name="spin"></param>
        public void Rotate(Stage stage)
        {
            stage.SpinResult = Model.Spin.Rotate(3);
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

        /// <summary>
        /// Recalculate the stage after play.
        /// </summary>
        /// <param name="stage">The stage to recalculate.</param>
        public void RecalculateStage(Stage stage)
        {
            if (stage == null || stage.Stake == 0)
            {
                throw new GameException("No initialized stage");
            };
            if (stage.SpinResult == null)
            {
                throw new GameException("Not prepared");
            }

            try
            {
                var winAmount = CalculateWinAmount(stage.SpinResult, stage.Stake);
                stage.WinAmount = winAmount;
            }
            catch (Exception e)
            {
                throw new GameException("Calculation problem", e);
            }
        }

    }
}
