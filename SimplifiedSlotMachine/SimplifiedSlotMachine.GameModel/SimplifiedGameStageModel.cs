using GameModel.Abstract;
using SimplifiedSlotMachine.DataModel;

namespace SimplifiedSlotMachine.GameModel
{
    public class SimplifiedGameStageModel: IGameStageModel
    {
        protected IGameModel Model { get; private set; }

        public SimplifiedGameStageModel(IGameModel model) { 
            Model = model;
        }

        public Stage Start(decimal balance, decimal stake)
        {
            var result = new Stage(balance);
            result.Stake = stake;
            result.WinAmount = 0;
            result.EndBalance = balance - stake;
            result.SpinResult = null;
            return result;
        }

        public void Rotate(Stage stage)
        {
            Model.Rotate(stage);
        }

        /// <summary>
        /// Recalculate the stage after play.
        /// </summary>
        /// <param name="stage"></param>
        public void RecalculateStage(Stage stage)
        {
            if (stage == null || stage.Stake == 0)
            {
                throw new GameException("No initialized stage");
            };
            if (stage.SpinResult == null) {
                throw new GameException("Not prepared");       // TODO: May show exception.
            }

            try
            {
                var winAmount = Model.CalculateWinAmount(stage.SpinResult, stage.Stake);
                stage.WinAmount = winAmount;
                stage.EndBalance = stage.BeginBalance - stage.Stake + stage.WinAmount;
            }
            catch (Exception e)
            {
                throw new GameException("Calculation problem", e);
            }
        }

    }
}
