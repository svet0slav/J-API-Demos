namespace RockPapSci.Data.Interfaces
{
    public interface IGameModel
    {
        List<ChoiceItem> ChoiceItems { get; }

        /// <summary>
        /// List of choices, where Item1 is stronger than Item2.
        /// </summary>
        List<ChoicePair> Strengths { get; }

        void Initialize();
    }
}
