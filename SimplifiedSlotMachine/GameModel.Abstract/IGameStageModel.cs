using SimplifiedSlotMachine.DataModel;

namespace GameModel.Abstract
{
    public interface IGameStageModel
    {
        Stage Start(decimal balance, decimal stake);
        Stage NextStart(Stage stage);
        void Rotate(Stage stage);
        void RecalculateStage(Stage stage);
    }
}
