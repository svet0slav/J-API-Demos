// See https://aka.ms/new-console-template for more information
using SimplifiedSlotMachine.Console;
using GameModel.Abstract;
using SimplifiedSlotMachine.GameModel;

Console.WriteLine("Hello, welcome to Simple Slot Machine ABP*!");

var ui = new UIController();

decimal initialBalance = ui.EnterBalance();
decimal currentBalance = initialBalance;

var spin = new SimplifiedSpin();

while (currentBalance > 0)
{
    var game = new SimplifiedGameModel(spin);
    decimal stake = ui.EnterStake(currentBalance);
    game.StartSession(currentBalance, stake);
    game.RotateSession();

    ui.PrintStage(game.SessionStages);
    ui.PrintTotals(game, game.SessionStages);

    currentBalance = game?.CurrentSession?.EndBalance ?? 0;
}

Console.WriteLine("No money left! End of the game!");
Console.ReadKey();
