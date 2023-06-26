using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplifiedSlotMachine.DataModel
{
    /// <summary>
    /// Specific exception dedicated to game related problems.
    /// </summary>
    public class GameException: Exception
    {
        public GameException(string message): base(message) { }
        public GameException(string message, Exception inner) : base(message, inner) { }
    }
}
