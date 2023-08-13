using RockPapSci.Data.Interfaces;
using System.Linq;

namespace RockPapSci.Data
{
    /// <summary>
    /// Model of the game and configurations.
    /// </summary>
    public class GameModel: IGameModel
    {
        public List<ChoiceItem> ChoiceItems { get; protected set; }

        /// <summary>
        /// List of choices, where Item1 is stronger than Item2.
        /// </summary>
        public List<ChoicePair> Strengths { get; protected set; }

        public GameModel() {
            Initialize();
        }

        public void Initialize()
        {
            // Game symbols and rules https://www.wikihow.com/Play-Rock-Paper-Scissors-Lizard-Spock
            ChoiceItems = new List<ChoiceItem>() {
                new (1, "Rock", "R" ),
                new (2, "Paper", "P" ),
                new (3, "Scissors", "S" ),
                new (4, "Lizard", "L" ),
                new (5, "Spock", "K" )
            };

            Strengths = new List<ChoicePair>();
            // A Simple Way to Remember/Express Who Wins
            // Scissors cuts paper.
            AddStrengthRule("S", "P");
            // Paper covers rock.
            AddStrengthRule("P", "R");
            // Rock crushes lizard.
            AddStrengthRule("R", "L");
            // Lizard poisons Spock.
            AddStrengthRule("L", "K");
            // Spock smashes scissors.
            AddStrengthRule("K", "S");
            // Scissors decapitates lizard.
            AddStrengthRule("S", "L");
            // Lizard eats paper.
            AddStrengthRule("L", "P");
            // Paper disproves Spock.
            AddStrengthRule("P", "K");
            // Spock vaporizes rock.
            AddStrengthRule("K", "R");
            //Rock crushes scissors.
            AddStrengthRule("R", "S");
        }

        protected void AddStrengthRule(string letter1, string letter2)
        {
            var item1 = ChoiceItems.Single(x => x.Letter.ToUpper() == letter1.ToUpper());
            if (item1 == null)
                throw new Exception("Invalid symbol");
            var item2 = ChoiceItems.Single(x => x.Letter.ToUpper() == letter2.ToUpper());
            if (item2 == null)
                throw new Exception("Invalid symbol");
            Strengths.Add(new ChoicePair(item1, item2));
        }
    }
}
