using Microsoft.Extensions.Logging;
using Moq;
using RockPapSci.Data;
using RockPapSci.Data.Interfaces;
using RockPapSci.Dtos.Choices;
using RockPapSci.Dtos.Play;
using RockPapSci.Service;
using RockPapSci.Service.Common;

namespace RockPapSci.UnitTests.Service
{
    [TestClass]
    public class GameServiceUnitTests
    {
        private Mock<IGameModel> _gameModel;
        private Mock<IGameModelRules> _gameModelRules;
        private Mock<IRandomGeneratorService> _randomGeneratorService;
        private Mock<ILogger<GameService>> _logger;

        [TestInitialize]
        public void TestInitialize()
        {
            _gameModel = new Mock<IGameModel>();
            var testModel = new GameModel();
            testModel.Initialize();
            _gameModel.SetupGet(m => m.ChoiceItems).Returns(testModel.ChoiceItems);
            _gameModel.SetupGet(m => m.Strengths).Returns(testModel.Strengths);

            _randomGeneratorService = new Mock<IRandomGeneratorService>();
            var n = _gameModel.Object.ChoiceItems.Count;

            _randomGeneratorService.Setup(r => r.GetRandom(n, It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(GetRandomServiceResult(n)))
                .Verifiable();

            _gameModelRules = new Mock<IGameModelRules>();
            _gameModelRules.SetupGet(s => s.GameModel).Returns(_gameModel.Object).Verifiable();
            // We will setup non-working rules by default, so in tests they fail if we do not setup correct winner result.
            _gameModelRules.Setup(s => s.GetWinner(It.IsAny<ChoiceItem>(), It.IsAny<ChoiceItem>()))
                .Returns(WinnerResult.NotAvailable).Verifiable();

            _logger = new Mock<ILogger<GameService>>();
        }

        [TestMethod]
        public void Service_NoModel_FailAll()
        {
            var service = new GameService(null, null, null, _logger.Object);

            Assert.ThrowsExceptionAsync<InvalidOperationException>(() => service.GetChoices(CancellationToken.None));
            
            Assert.ThrowsExceptionAsync<InvalidOperationException>(() => service.GetChoices(CancellationToken.None));
            Assert.ThrowsExceptionAsync<InvalidOperationException>(() => service.GetRandomChoice(CancellationToken.None));
            Assert.ThrowsExceptionAsync<NullReferenceException>(() => service.BotPlayOne(new ChoiceDto(), CancellationToken.None));
        }

        [TestMethod]
        public void Service_NoModelRules_FailsPlayOnly()
        {
            var service = new GameService(_gameModel.Object, null, null, _logger.Object);

            // Throws no exception
            var choices = service.GetChoices(CancellationToken.None);
            var choice = service.GetRandomChoice(CancellationToken.None);

            Assert.ThrowsExceptionAsync<NullReferenceException>(() => service.BotPlayOne(new ChoiceDto(), CancellationToken.None));
        }

        [TestMethod]
        public void Service_NoRandomGen_FailsPlayOnly()
        {
            var service = new GameService(_gameModel.Object, _gameModelRules.Object, null, _logger.Object);

            // Throws no exception
            var choices = service.GetChoices(CancellationToken.None);

            Assert.ThrowsExceptionAsync<NullReferenceException>(() => service.GetRandomChoice(CancellationToken.None));

            Assert.ThrowsExceptionAsync<NullReferenceException>(() => service.BotPlayOne(new ChoiceDto(), CancellationToken.None));
        }

        [TestMethod]
        public void Service_AllSetup_CorrectCalls()
        {
            var service = new GameService(_gameModel.Object, _gameModelRules.Object, _randomGeneratorService.Object, _logger.Object);
            // Setup the result, so it is NA
            _gameModelRules.Setup(s => s.GetWinner(It.IsAny<ChoiceItem>(), It.IsAny<ChoiceItem>()))
                .Returns(WinnerResult.FirstWins).Verifiable();

            // Throws no exception
            var choices = service.GetChoices(CancellationToken.None);
            var choice = service.GetRandomChoice(CancellationToken.None);
            var result = service.BotPlayOne(new ChoiceDto() { Id = 1, Name="Rock" }, CancellationToken.None);

            Assert.IsNotNull(choices);
            Assert.IsNotNull(choices.Result);
            Assert.IsNotNull(choices.Result.Choices);
            Assert.AreEqual(choices.Result.Choices.Count(), _gameModel.Object.ChoiceItems.Count);
            _gameModel.Verify(m => m.ChoiceItems, Times.AtLeast(1 + 2 + 2));
            
            Assert.IsNotNull(choice);
            Assert.IsNotNull(choice.Result);
            Assert.IsNotNull(choice.Result.Name);
            Assert.IsTrue(choice.Result.Id > 0);
            _randomGeneratorService.Verify(rgs => rgs.GetRandom(It.IsAny<int>(), It.IsAny<CancellationToken>()),
                Times.Exactly(2));

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Result);
            Assert.IsTrue(result.Result.PlayerChoice > 0);
            Assert.IsTrue(result.Result.BotChoice > 0);
            Assert.IsNotNull(result.Result.Result);
            Assert.AreEqual(WinnerResult.FirstWins.ToText(), result.Result.Result);
            
            _gameModelRules.Verify(r => r.GetWinner(It.IsAny<ChoiceItem>(), It.IsAny<ChoiceItem>()),
                Times.Once);
        }

        [TestMethod]
        public void Service_AllSetupNoPlayerChoice_Fails()
        {
            var service = new GameService(_gameModel.Object, _gameModelRules.Object, _randomGeneratorService.Object, _logger.Object);

            Assert.ThrowsExceptionAsync<ArgumentNullException>(() => service.BotPlayOne(null, CancellationToken.None));
            Assert.ThrowsExceptionAsync<ArgumentOutOfRangeException>(() => service.BotPlayOne(new ChoiceDto(), CancellationToken.None));
        }

        [TestMethod]
        public void Service_AllSetupBotNoChoice_Fails()
        {
            var service = new GameService(_gameModel.Object, _gameModelRules.Object, _randomGeneratorService.Object, _logger.Object);
            // Setup the result, so it is NA
            _gameModelRules.Setup(s => s.GetWinner(It.IsAny<ChoiceItem>(), It.IsAny<ChoiceItem>()))
                .Returns(WinnerResult.NotAvailable).Verifiable();
            _randomGeneratorService.Setup(r => r.GetRandom(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(-1))
                .Verifiable();

            var actualEx = Assert.ThrowsExceptionAsync<ServiceException>(() =>
                service.BotPlayOne(new ChoiceDto() { Id = 3, Name = "Lizard" }, CancellationToken.None));

            Assert.IsNotNull(actualEx.Result);
            Assert.AreEqual("The bot does not make choice", actualEx.Result?.Message);
            Assert.IsNull(actualEx.Result?.InnerException);

            _gameModel.Verify(m => m.ChoiceItems, Times.AtLeast(2));
            _gameModelRules.Verify(r => r.GetWinner(It.IsAny<ChoiceItem>(), It.IsAny<ChoiceItem>()),
                Times.Never);
            _randomGeneratorService.Verify(rgs => rgs.GetRandom(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [TestMethod]
        public void Service_AllSetupPlayerChoiceInvalidId_FindByName() {
            var service = new GameService(_gameModel.Object, _gameModelRules.Object, _randomGeneratorService.Object, _logger.Object);
            // Setup the result, so it is NA
            _gameModelRules.Setup(s => s.GetWinner(It.IsAny<ChoiceItem>(), It.IsAny<ChoiceItem>()))
                .Returns(WinnerResult.SecondWins).Verifiable();

            var actual = service.BotPlayOne(new ChoiceDto() { Id = 9999, Name = "Lizard" }, CancellationToken.None);

            Assert.IsNotNull(actual);
            Assert.IsNotNull(actual.Result);
            Assert.IsTrue(actual.Result.PlayerChoice > 0);
            Assert.AreNotEqual(9999, actual.Result.PlayerChoice);
            Assert.IsTrue(actual.Result.BotChoice > 0);
            Assert.IsNotNull(actual.Result.Result);
            Assert.AreEqual(WinnerResult.SecondWins.ToText(), actual.Result.Result);

            _gameModel.Verify(m => m.ChoiceItems, Times.AtLeast(3));
            _gameModelRules.Verify(r => r.GetWinner(It.IsAny<ChoiceItem>(), It.IsAny<ChoiceItem>()),
                Times.Once);
            _randomGeneratorService.Verify(rgs => rgs.GetRandom(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [TestMethod]
        public void Service_AllSetupPlayerChoiceInvalidIdAndName_Fails()
        {
            var service = new GameService(_gameModel.Object, _gameModelRules.Object, _randomGeneratorService.Object, _logger.Object);
            // Setup the result, so it is NA
            _gameModelRules.Setup(s => s.GetWinner(It.IsAny<ChoiceItem>(), It.IsAny<ChoiceItem>()))
                .Returns(WinnerResult.SecondWins).Verifiable();

            var actualEx = Assert.ThrowsExceptionAsync<ArgumentOutOfRangeException>(() =>
                    service.BotPlayOne(new ChoiceDto() { Id = 9999, Name = "NA" }, CancellationToken.None));

            Assert.IsNotNull(actualEx);
            Assert.AreEqual("The choice 9999 - NA is invalid (Parameter 'playerChoice')", actualEx.Result.Message);
            Assert.AreEqual("playerChoice", actualEx.Result.ParamName);
            Assert.IsNull(actualEx.Result.InnerException);

            _gameModel.Verify(m => m.ChoiceItems, Times.AtLeast(2));
            _gameModelRules.Verify(r => r.GetWinner(It.IsAny<ChoiceItem>(), It.IsAny<ChoiceItem>()),
                Times.Never);
            _randomGeneratorService.Verify(rgs => rgs.GetRandom(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        private int GetRandomServiceResult(int n)
        {
            var randomGen = new Random(DateTime.Now.Millisecond * 7 + (int)(DateTime.Now.Ticks % 739));
            var count = randomGen.Next(5, 25);
            for(int i=0; i< count; i++)
            {
                randomGen.Next();
            }
            
            return randomGen.Next(0, 255) % n;
        }
    }
}
