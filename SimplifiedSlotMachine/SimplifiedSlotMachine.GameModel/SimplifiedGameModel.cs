using GameModel.Abstract;
using SimplifiedSlotMachine.DataModel;

namespace SimplifiedSlotMachine.GameModel
{
    /// <summary>
    /// Responsible class for the game model based on session with stages.
    /// </summary>
    public class SimplifiedGameModel: IGameModel
    {
        public GameSession? CurrentSession { get; protected set; }
        public List<Stage>? SessionStages {  get; protected set; }
        public int SessionSize { get; protected set; }
        public IGameSpin Spin { get; protected set; }

        public SimplifiedGameModel(GameConfiguration configuration, IGameSpin spin) {

            if (configuration.SessionSize <= 0 || configuration.SessionSize > 100)
            {
                throw new ArgumentOutOfRangeException("Session Size between 1 and 100");
            }
            SessionSize = configuration.SessionSize;

            if (spin == null)
            {
                throw new ArgumentNullException("Spin not configured");
            }
            Spin = spin;

            if (configuration.Symbols == null || configuration.Symbols?.Count() <= 2 || configuration.Symbols?.Count() > 100)
            {
                throw new ArgumentOutOfRangeException("Symbols should be set between 3 and 100.");
            }

            Spin.AvailableSymbols = configuration.Symbols.ToList();

            CurrentSession = null;
            SessionStages = null;
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