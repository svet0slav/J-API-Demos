namespace SimplifiedSlotMachine.DataModel
{
    public class Symbol
    {
        public string Name { get; set; }
        public string Letter { get; set; }
        public decimal Coefficient { get; set; }
        public double Probability { get; set; }
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
