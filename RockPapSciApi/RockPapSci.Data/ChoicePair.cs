namespace RockPapSci.Data
{
    public class ChoicePair
    {
        public ChoiceItem Item1 { get; set; }

        public ChoiceItem Item2 { get; set; }

        public ChoicePair(ChoiceItem item1, ChoiceItem item2) { 
            Item1 = item1;
            Item2 = item2;
        }
    }
}
