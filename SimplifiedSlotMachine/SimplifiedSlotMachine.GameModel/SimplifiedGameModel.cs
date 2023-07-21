using GameModel.Abstract;
using SimplifiedSlotMachine.DataModel;

namespace SimplifiedSlotMachine.GameModel
{
    /// <summary>
    /// Responsible class for the game model based on session with stages.
    /// </summary>
    public class SimplifiedGameModel: IGameModel
    {
        public const int Rotate_Spin_Per_Session = 4;

        public List<Symbol> Symbols { get; private set; }
        public GameSession? CurrentSession { get; protected set; }
        public List<Stage>? SessionStages {  get; protected set; }
        public int SessionSize { get; protected set; }
        public IGameSpin Spin { get; protected set; }

        public SimplifiedGameModel(IGameSpin spin) {
            SessionSize = Rotate_Spin_Per_Session;
            Spin = spin;

            Symbols = InitializeSymbols();
            Spin.AvailableSymbols = Symbols;

            CurrentSession = null;
            SessionStages = null;
        }

        protected List<Symbol> InitializeSymbols()
        {
            return new List<Symbol>()
            {
                new Symbol("Apple", "A", 0.4M, 0.45),
                new Symbol( "Banana", "B", 0.6M, 0.35 ),
                new Symbol("Pineapple", "P", 0.8M, 0.15 ),
                new Symbol( "Wildcard", "*", 0, 0.05, true)
            };
        }

        public void StartSession(decimal balance, decimal stake) {
            CurrentSession = new GameSession(balance);
            CurrentSession.SetStake(stake);
            SessionStages = new List<Stage>(SessionSize);
        }

        public void RotateSession()
        {
            if (CurrentSession == null)
                throw new GameException("Session not initialized");

            var model = new SimplifiedGameStageModel(this);

            int i = 0;
            do
            {
                decimal stake = CurrentSession.Stake;
                var stage = RotateStage(model, stake);
                SessionStages?.Add(stage);
                CurrentSession.AddWinAmount(stage.WinAmount);
                
                i++;
            } while (i < SessionSize);
        }

        protected Stage RotateStage(SimplifiedGameStageModel model, decimal stake)
        {
            var stage = new Stage(stake);
            model.Rotate(stage);
            model.RecalculateStage(stage);
            return stage;
        }
    }
}