using Microsoft.AspNetCore.Mvc;
using RockPapSci.Api.ErrorHandling;
using RockPapSci.Dtos.Choices;
using RockPapSci.Dtos.Play;
using RockPapSci.Service.Common;

namespace RockPapSci.Api.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Route("v{version:apiVersion}")]
    [Produces("application/json")]
    public class MainController : ControllerBase
    {
        private readonly IGameService _gameService;
        private readonly ILogger<MainController> _logger;

        public MainController(
            IGameService gameService,
            ILogger<MainController> logger)
        {
            _gameService = gameService;
            _logger = logger;
        }

        [MapToApiVersion("1")]
        [HttpGet("choices", Name = "GetChoices")]
        [ProducesResponseType(statusCode: 200, type: typeof(ChoicesResponse))]
        [ProducesResponseType(statusCode: 404, type: typeof(ErrorDetails))]
        [ProducesResponseType(statusCode: 500, type: typeof(ErrorDetails))]
        public async Task<ActionResult<ChoicesResponse>> GetChoices(CancellationToken cancellationToken)
        {
            _logger.LogInformation("GetChoices started");
            var result = await _gameService.GetChoices(cancellationToken);


            if (result == null)
            {
                _logger.LogInformation("GetChoices finished", 0);
                return NotFound(ErrorDetails.NotFound("Choices"));
            }

            _logger.LogInformation("GetChoices finished", result?.Choices?.Count());

            return Ok(result);
        }

        [MapToApiVersion("1")]
        [HttpGet("choice", Name = "GetChoice")]
        [ProducesResponseType(statusCode: 200, type: typeof(ChoiceDto))]
        [ProducesResponseType(statusCode: 400, type: typeof(ErrorDetails))]
        [ProducesResponseType(statusCode: 404, type: typeof(ErrorDetails))]
        [ProducesResponseType(statusCode: 500, type: typeof(ErrorDetails))]
        public async Task<ActionResult<ChoiceDto>> GetChoice(CancellationToken cancellationToken)
        {
            _logger.LogInformation("GetChoice started");

            var result = await _gameService.GetRandomChoice(cancellationToken);

            if (result == null)
            {
                _logger.LogInformation("GetChoices finished without results", 0);
                return NotFound(ErrorDetails.NotFound("Choice or Random generated value"));
            }

            _logger.LogInformation("GetChoice finished", result?.Id);
            return Ok(result);
        }

        [MapToApiVersion("1")]
        [HttpPost("play", Name = "Play")]
        [ProducesResponseType(statusCode: 200, type: typeof(PlayResponse))]
        [ProducesResponseType(statusCode: 400, type: typeof(ErrorDetails))]
        [ProducesResponseType(statusCode: 404, type: typeof(ErrorDetails))]
        [ProducesResponseType(statusCode: 500, type: typeof(ErrorDetails))]
        public async Task<ActionResult<PlayResponse>> PlayOne([FromBody] PlayRequest playRequest, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Play One started");

            if (playRequest == null || string.IsNullOrWhiteSpace(playRequest.PlayerChoice))
            {
                _logger.LogInformation("Play One finished without player choice.");
                return BadRequest("No player choice");
            }

            var playerChoice =
                (int.TryParse(playRequest.PlayerChoice, out int choiceId))
                ? new ChoiceDto() { Id = choiceId, Name = playRequest.PlayerChoice }
                : new ChoiceDto() { Name = playRequest.PlayerChoice };

            var result = await _gameService.BotPlayOne(playerChoice, cancellationToken);

            if (result == null)
            {
                _logger.LogInformation("Play One finished without result.");
                return NotFound(ErrorDetails.NotFound("Choice or Random generated value"));
            }

            _logger.LogInformation("Play One finished", result);
            return Ok(result);
        }
    }
}
