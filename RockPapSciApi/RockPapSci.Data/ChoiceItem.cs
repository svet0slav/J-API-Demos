namespace RockPapSci.Data
{
    public class ChoiceItem: IEquatable<ChoiceItem>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Letter {  get; set; }

        public ChoiceItem(int id, string name, string letter)
        {
            Id = id;
            Name = name;
            Letter = letter;
        }

        public bool Equals(ChoiceItem? other)
        {
            if (other == null)
                return false;
            if (ReferenceEquals(this, other)) return true;

            return this.Id == other.Id || this.Name.ToUpper() == other.Name.ToUpper();
        }

        /// <summary>
        /// Show easier in debugging.
        /// </summary>
        /// <returns>Format {Letter} ({Id}) {Name}, like R (1) Rock</returns>
        public override string ToString()
        {
            return $"{Letter} ({Id}) {Name}";
        }
    }
}