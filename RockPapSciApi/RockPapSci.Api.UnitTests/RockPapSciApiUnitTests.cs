using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using RockPapSci.Api.Controllers;
using RockPapSci.Data;
using RockPapSci.Data.Interfaces;
using RockPapSci.Dtos.Choices;
using RockPapSci.Dtos.Play;
using RockPapSci.Service.Common;
using RockPapSci.Service.Mappers;

namespace RockPapSci.Api.UnitTests
{
    [TestClass]
    public class RockPapSciApiUnitTests
    {
        private Mock<IGameService> _gameService;
        private MainController _mainController;
        private Mock<ILogger<MainController>> _logger;


        [TestInitialize]
        public void Setup()
        {
            _logger = new Mock<ILogger<MainController>>();
            _gameService = new Mock<IGameService> ( MockBehavior.Strict );
            _mainController = new MainController(_gameService.Object, _logger.Object);
        }

        [TestMethod]
        public async Task MainController_Choices_Ok()
        {
            var testModel = FakeGameModel();
            var testResponse = new ChoicesResponse()
            {
                Choices = testModel.ChoiceItems.Select(x => x.ToDto())
            };
            _gameService.Setup(gs => gs.GetChoices(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(testResponse));

            var actual = await _mainController.GetChoices(CancellationToken.None);

            _gameService.Verify(gs => gs.GetChoices(It.IsAny<CancellationToken>()), Times.Once);

            Assert.IsNotNull(actual);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOfType(actual.Result, typeof(OkObjectResult));
            var actualValue = ((OkObjectResult)actual.Result).Value;
            Assert.IsNotNull(actualValue);
            Assert.IsInstanceOfType(actualValue, typeof(ChoicesResponse));
            var value = (actualValue as ChoicesResponse);
            Assert.IsNotNull(value?.Choices);
            Assert.AreEqual(5, value?.Choices.Count());
        }

        [TestMethod]
        public async Task MainController_Choice_Ok()
        {
            var testModel = FakeGameModel();
            var testChoice = new ChoiceDto()
            {
                Id = 2, Name = "Scissors"
            };
            _gameService.Setup(gs => gs.GetRandomChoice(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<ChoiceDto?>(testChoice));

            var actual = await _mainController.GetChoice(CancellationToken.None);

            // Assertions
            _gameService.Verify(gs => gs.GetRandomChoice(It.IsAny<CancellationToken>()), Times.Once);

            Assert.IsNotNull(actual);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOfType(actual.Result, typeof(OkObjectResult));
            var actualValue = ((OkObjectResult)actual.Result).Value;
            Assert.IsNotNull(actualValue);
            Assert.IsInstanceOfType(actualValue, typeof(ChoiceDto));
            var value = (actualValue as ChoiceDto);
            Assert.IsNotNull(value);
            Assert.AreEqual(2, value.Id);
            Assert.AreEqual("Scissors", value.Name);
        }


        [TestMethod]
        public async Task MainController_Play_Ok()
        {
            var testModel = FakeGameModel();
            var playChoice = new ChoiceDto()
            {
                Id = 1,
                Name = "Rock"
            };
            var testChoice = new ChoiceDto()
            {
                Id = 2,
                Name = "Scissors"
            };
            _gameService.Setup(gs => gs.GetRandomChoice(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<ChoiceDto?>(testChoice));

            var playRequest = new PlayRequest() { PlayerChoice = playChoice.Name };
            var playResponse = new PlayResponse()
            {
                PlayerChoice = playChoice.Id,
                BotChoice = testChoice.Id,
                Result = WinnerResult.FirstWins.ToText(),
            };
            _gameService.Setup(gs => gs.BotPlayOne(It.IsAny<ChoiceDto>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<PlayResponse?>(playResponse));
            
            var actual = await _mainController.PlayOne(playRequest, CancellationToken.None);

            // Assertions
            _gameService.Verify(gs => gs.BotPlayOne(It.IsAny<ChoiceDto>(), It.IsAny<CancellationToken>()), Times.Once);

            Assert.IsNotNull(actual);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOfType(actual.Result, typeof(OkObjectResult));
            var actualValue = ((OkObjectResult)actual.Result).Value;
            Assert.IsNotNull(actualValue);
            Assert.IsInstanceOfType(actualValue, typeof(PlayResponse));
            var value = (actualValue as PlayResponse);
            Assert.IsNotNull(value);
            Assert.AreEqual(1, value.PlayerChoice);
            Assert.AreEqual(2, value.BotChoice);
            Assert.AreEqual(WinnerResult.FirstWins.ToText(), value.Result);
        }

        private IGameModel FakeGameModel()
        {
            var model = new GameModel();
            model.Initialize();
            return model;
        }
    }
}