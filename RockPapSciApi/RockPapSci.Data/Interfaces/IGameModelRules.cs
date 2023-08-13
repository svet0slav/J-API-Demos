namespace RockPapSci.Data.Interfaces
{
    /// <summary>
    /// Abstract representation of the rules for the game.
    /// </summary>
    public interface IGameModelRules
    {
        IGameModel GameModel { get; }

        WinnerResult GetWinner(ChoiceItem? Item1,  ChoiceItem? Item2);
    }
}