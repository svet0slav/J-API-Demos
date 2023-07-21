namespace SimplifiedSlotMachine.DataModel
{
    public class GameSession
    {
        public decimal BeginBalance { get; protected set; }
        public decimal Stake { get; protected set; }
        public decimal WinAmount { get; protected set; }
        public decimal EndBalance { get; protected set; }

        public List<Stage> Stages { get; set; }

        public GameSession(decimal beginBalance) { 
            Stages = new List<Stage>();
            BeginBalance = beginBalance;
            Stake = 0;
            WinAmount = 0;
            EndBalance = beginBalance;
        }

        public void SetStake(decimal stake)
        {
            Stake = stake;
            EndBalance = BeginBalance - Stake + WinAmount; 
        }

        public void SetWinAmount(decimal winAmount)
        {
            if (winAmount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(winAmount));
            }
            WinAmount = winAmount;
            EndBalance = BeginBalance - Stake + WinAmount;
        }

        public void AddWinAmount(decimal winAmount)
        {
            if (winAmount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(winAmount));
            }
            WinAmount += winAmount;
            EndBalance = BeginBalance - Stake + WinAmount;
        }
    }
}
