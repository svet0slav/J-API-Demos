namespace SimplifiedSlotMachine.DataModel
{
    public class Symbol
    {
        public string Name { get; set; }

        /// <summary>
        /// Single char string for the letter to show about the symbol.
        /// </summary>
        public string Letter { get; set; }

        public decimal Coefficient { get; set; }

        /// <summary>
        /// Probability between 0.00 and 1.00
        /// </summary>
        public double Probability { get; set; }

        /// <summary>
        /// Is the symbol wildcard.
        /// </summary>
        public bool IsWildcard {  get; set; }

        public Symbol(string name, string letter, decimal coefficient, double probability, bool isWildcard = false)
        {
            this.Name = name;
            this.Letter = letter;
            this.Coefficient = coefficient;
            this.Probability = probability;
            IsWildcard = isWildcard;
        }
    }
}
