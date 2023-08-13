using Microsoft.AspNetCore.Mvc;
using RockPapSci.Dtos.Choices;
using RockPapSci.Service.Common;
using System.Threading;

namespace RockPapSci.Api.Controllers
{
    [ApiController]
    [Route("")]
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

        [HttpGet("choices", Name = "GetChoices")]
        [ProducesResponseType(statusCode: 200, type: typeof(ChoicesResponse))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ChoicesResponse>> GetChoices(CancellationToken cancellationToken)
        {
            var result = await _gameService.GetChoices(cancellationToken);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }


        [HttpGet("/choice", Name = "GetChoice")]
        [ProducesResponseType(statusCode: 200, type: typeof(ChoiceDto))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ChoiceDto>> GetChoice(CancellationToken cancellationToken)
        {
            var result = await _gameService.GetRandomChoice(cancellationToken);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost("/play", Name = "Play")]
        public IEnumerable<string> Play()
        {
            return Enumerable.Empty<string>();
        }
    }
}