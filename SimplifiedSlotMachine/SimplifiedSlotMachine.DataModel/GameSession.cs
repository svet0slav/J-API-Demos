namespace SimplifiedSlotMachine.DataModel
{
    public class GameSession
    {
        public Player GamePlayer { get; set; }
        public decimal Deposit { get; set; }

        public List<Stage> Stages { get; set; }

        public GameSession(Player gamePlayer) { 
            Stages = new List<Stage>();
            GamePlayer = gamePlayer;
        }
    }
}
