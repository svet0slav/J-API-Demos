namespace SimplifiedSlotMachine.DataModel
{
    public class GameSession
    {
        public decimal BeginBalance { get; set; }
        public decimal Stake { get; set; }
        public decimal WinAmount { get; set; }
        public decimal EndBalance { get; set; }

        public List<Stage> Stages { get; set; }

        public GameSession(decimal beginBalance, decimal stake) { 
            Stages = new List<Stage>();
            BeginBalance = beginBalance;
            Stake = stake;
            WinAmount = 0;
            EndBalance = beginBalance;
        }
    }
}
