using SimplifiedSlotMachine.DataModel;
using System;

namespace GameModel.Abstract
{
    public interface IGameSpin
    {
        List<Symbol> AvailableSymbols { get; set; }

        List<Symbol> Rotate(int outputCount);
    }
}
