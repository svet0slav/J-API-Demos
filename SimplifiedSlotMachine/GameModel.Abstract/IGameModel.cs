using SimplifiedSlotMachine.DataModel;

namespace GameModel.Abstract
{
    public interface IGameModel
    {
        IGameSpin Spin { get; }

        GameSession? CurrentSession { get; }

        void StartSession(decimal balance, decimal stake);

        void RotateSession();
    }
}