namespace SimplifiedSlotMachine.DataModel
{
    public class Stage
    {
        public decimal Stake { get; set; }
        public decimal BeginBalance { get; set; }
        public decimal WinAmount { get; set; }
        public decimal EndBalance { get; set; }
        public List<Symbol>? SpinResult { get; set; }

        public Stage(decimal beginBalance) {
            BeginBalance = beginBalance;
            Stake = 0;
            WinAmount = 0;
            EndBalance = beginBalance;
        }
    }
}
