namespace SimplifiedSlotMachine.DataModel
{
    public class GameConfiguration
    {
        /// <summary>
        /// The number of symbols in one stage, in one slot.
        /// </summary>
        public int SlotSymbolsCount { get; set; }

        /// <summary>
        /// The list of the symbols.
        /// </summary>
        public IEnumerable<Symbol> Symbols { get; set; }

        /// <summary>
        /// The number of stages in a game session.
        /// </summary>
        public int SessionSize { get; set; }
    }
}
