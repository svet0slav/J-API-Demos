namespace RockPapSci.Data.Interfaces
{
    public interface IGameModel
    {
        IEnumerable<ChoiceItem> ChoiceItems { get; }

        /// <summary>
        /// List of choices, where Item1 is stronger than Item2.
        /// </summary>
        IEnumerable<ChoicePair> Strengths { get; }

        void Initialize();
    }
}
