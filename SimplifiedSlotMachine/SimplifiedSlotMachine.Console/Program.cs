// See https://aka.ms/new-console-template for more information
using SimplifiedSlotMachine.Console;
using GameModel.Abstract;
using SimplifiedSlotMachine.GameModel;

Console.WriteLine("Hello, welcome to Simple Slot Machine ABP*!");

var ui = new UIController();

decimal initialBalance = ui.EnterBalance();
decimal currentBalance = initialBalance;

var spin = new SimplifiedSpin();
var game = new SimplifiedGameModel(spin);

while (currentBalance > 0)
{
    decimal stake = ui.EnterStake(currentBalance);
    game?.StartSession(currentBalance, stake);
    game?.RotateSession();
    var results = game?.SessionStages ?? new List<SimplifiedSlotMachine.DataModel.Stage>();

    ui.PrintStage(results);
    ui.PrintTotals(game, results);

    currentBalance = game?.CurrentSession?.EndBalance ?? 0;
}

Console.WriteLine("No money left! End of the game!");
Console.ReadKey();
