using SimplifiedSlotMachine.DataModel;
using System;

namespace GameModel.Abstract
{
    public interface IGameSpin
    {
        List<Symbol> Rotate(int outputCount);
    }
}
