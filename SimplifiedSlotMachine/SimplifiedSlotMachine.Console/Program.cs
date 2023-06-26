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

while (currentBalance >= 0)
{
    decimal stake = ui.EnterStake(currentBalance);
    var stageModel = new SimplifiedGameStageModel(game);
    var firstStage = stageModel.Start(currentBalance, stake);
    var results = game.RotateMultiple(firstStage);

    ui.PrintStage(results);
    ui.PrintTotals(stageModel, results);

    currentBalance = results.Last().EndBalance;
}

Console.WriteLine("End of the game!");
Console.ReadKey();
