using SimplifiedSlotMachine.DataModel;

namespace GameModel.Abstract
{
    public interface IGameModel
    {
        /// <summary>
        /// Is the stage winning for the player
        /// </summary>
        /// <param name="symbols">The symbols from the Stage.</param>
        /// <returns></returns>
        bool HasStageWin(List<Symbol> symbols);
        decimal CalculateWinAmount(List<Symbol> symbols, decimal stake);

        void Rotate(Stage stage);
    }
}