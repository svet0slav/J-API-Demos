using GameModel.Abstract;
using SimplifiedSlotMachine.DataModel;

namespace SimplifiedSlotMachine.Console
{
    internal class UIController
    {

        public UIController() { }

        public decimal EnterBalance() {
            decimal initialBalance = 0;

            while (initialBalance <= 0)
            {
                System.Console.WriteLine("Please, deposit money you would like to play with:");
                var enteredbalance = System.Console.ReadLine();

                if (!decimal.TryParse(enteredbalance, out initialBalance))
                {
                    initialBalance = 0;
                }
            }
            return initialBalance;
        }

        public decimal EnterStake(decimal currentBalance)
        {
            decimal stake = 0;
            while (stake <= 0)
            {
                System.Console.WriteLine("Enter stake amount:");
                var enteredStake = System.Console.ReadLine();
                if (!decimal.TryParse(enteredStake, out stake) || stake <= 0.0M || stake > currentBalance)
                {
                    System.Console.WriteLine("Stake amount should be positive <= Current Balance:");
                    stake = 0;
                }
            }

            return stake;
        }

        public void PrintStage(List<Stage> stages)
        {
            foreach(var stage in stages)
            {
                var symbols = stage.SpinResult;
                if (symbols == null) continue;
                foreach(var symbol in symbols)
                {
                    System.Console.Write(symbol.Letter);
                }
                System.Console.WriteLine();
            }
        }

        public void PrintTotals(IGameModel gameModel, List<Stage> stages)
        {
            var won = gameModel?.CurrentSession?.WinAmount;
            System.Console.WriteLine($"You have won: {won}");

            var balance = gameModel?.CurrentSession?.EndBalance;
            System.Console.WriteLine($"Current balance is: {balance}");
        }

    }
}
