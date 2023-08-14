using Microsoft.Extensions.Logging;
using RockPapSci.Data.Interfaces;
using RockPapSci.Dtos.Choices;
using RockPapSci.Dtos.Play;
using RockPapSci.Service.Common;
using RockPapSci.Service.Mappers;

namespace RockPapSci.Service
{
    public class GameService : IGameService
    {
        private readonly IGameModel _gameModel;
        private readonly IGameModelRules _gameModelRules;
        private readonly IRandomGeneratorService _randomGeneratorService;
        private readonly ILogger<GameService> _logger;

        public GameService(IGameModel gameModel, 
            IGameModelRules gameModelRules,
            IRandomGeneratorService randomGeneratorService,
            ILogger<GameService> logger)
        {
            _gameModel = gameModel;
            _gameModelRules = gameModelRules;
            _randomGeneratorService = randomGeneratorService;
            _logger = logger;
        }

        public Task<ChoicesResponse> GetChoices(CancellationToken cancellationToken)
        {
            if (_gameModel == null)
                throw new InvalidOperationException("Game Model not initialized");

            var result = _gameModel.ChoiceItems.Select(x =>
                new ChoiceDto()
                {
                    Id = x.Id,
                    Name = x.Name,
                });

            return Task.FromResult(new ChoicesResponse() { Choices = result });
        }

        public async Task<ChoiceDto?> GetRandomChoice(CancellationToken cancellationToken)
        {
            if (_gameModel == null)
                throw new InvalidOperationException("Game Model not initialized");
            var count = _gameModel.ChoiceItems.Count();
            var choiceNumber = await _randomGeneratorService.GetRandom(count, cancellationToken);
            if (choiceNumber < 0)
            {
                _logger.LogError("The random generator did not work.");
                return null;
            }

            // Get the correct choice. Zero based index is used.
            var selected = _gameModel.ChoiceItems.ElementAtOrDefault(choiceNumber);

            return selected?.ToDto();
        }

        /// <summary>
        /// Selects a choice for the bot and returns the result agains player choice.
        /// The Player is counted first.
        /// </summary>
        /// <param name="playerChoice">The player choice</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The response for the game play between player and the bot.
        ///     ArgumentNullException - if player choice is null.
        ///     ArgumentOutOfRangeException - if player choice not found in the list.
        ///     Other if bot choice can not be made.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<PlayResponse?> BotPlayOne(ChoiceDto playerChoice, CancellationToken cancellationToken)
        {
            if (playerChoice == null)
                throw new ArgumentNullException("Missing choice");

            var firstChoice = _gameModel.ChoiceItems.FirstOrDefault(c => c.Id == playerChoice.Id);
            if (firstChoice == null)
                firstChoice = _gameModel.ChoiceItems.FirstOrDefault(c => c.Name.ToUpper() == playerChoice.Name?.ToUpper());
            if (firstChoice == null)
                throw new ArgumentOutOfRangeException($"The choice {playerChoice.Id} - {playerChoice.Name} is invalid");

            var botChoice = await GetRandomChoice(cancellationToken);
            var secondChoice = _gameModel.ChoiceItems.FirstOrDefault(c => c.Id == botChoice?.Id);
            if (secondChoice == null)
                throw new ServiceException("The bot does not make choice");

            var result = _gameModelRules.GetWinner(firstChoice, secondChoice);
            var response = new PlayResponse() {
                PlayerChoice = firstChoice.Id,
                BotChoice = botChoice == null ? -1 : botChoice.Id,
                Result = result.ToText(),
            };

            return response;
        }
    }
}