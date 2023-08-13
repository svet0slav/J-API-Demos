using Microsoft.Extensions.Logging;
using RockPapSci.Data.Interfaces;
using RockPapSci.Dtos.Choices;
using RockPapSci.Service.Common;
using RockPapSci.Service.Mappers;

namespace RockPapSci.Service
{
    public class GameService : IGameService
    {
        private readonly IGameModel _gameModel;
        private readonly IRandomGeneratorService _randomGeneratorService;
        private readonly ILogger<GameService> _logger;

        public GameService(IGameModel gameModel, IRandomGeneratorService randomGeneratorService,
            ILogger<GameService> logger)
        {
            _gameModel = gameModel;
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
    }
}