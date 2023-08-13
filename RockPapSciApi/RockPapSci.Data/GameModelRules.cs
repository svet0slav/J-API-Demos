using RockPapSci.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RockPapSci.Data
{
    public class GameModelRules : IGameModelRules
    {
        private IGameModel _gameModel;
        public IGameModel GameModel { get { return _gameModel; } }

        public GameModelRules(IGameModel model)
        {
            _gameModel = model;
        }

        public WinnerResult GetWinner(ChoiceItem item1, ChoiceItem item2)
        {
            if (item1 == null || item2 == null)
                throw new ArgumentNullException("Choice 1 or 2 not made");

            var definition = _gameModel.Strengths.FirstOrDefault(s => s.Item1.Equals(item1) && s.Item2.Equals(item2));
            if (definition != null)
            {
                return WinnerResult.FirstWins;
            }
            else
            {
                definition = _gameModel.Strengths.FirstOrDefault(s => s.Item1.Equals(item2) && s.Item2.Equals(item1));
                if (definition != null)
                {
                    return WinnerResult.SecondWins;
                }
                else
                {
                    return WinnerResult.NotAvailable;
                }
            }

        }
    }
}
