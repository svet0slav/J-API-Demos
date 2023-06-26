namespace SimplifiedSlotMachine.DataModel
{
    public class Stage
    {
        public decimal Stake { get; set; }
        public decimal WinAmount { get; set; }
        public List<Symbol>? SpinResult { get; set; }

        public Stage(decimal stake) {
            Stake = stake;
            WinAmount = 0;
        }
    }
}
