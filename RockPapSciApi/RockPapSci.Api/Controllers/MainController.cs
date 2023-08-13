using Microsoft.AspNetCore.Mvc;

namespace RockPapSci.Api.Controllers
{
    [ApiController]
    [Route("")]
    public class MainController : ControllerBase
    {
      
        private readonly ILogger<MainController> _logger;

        public MainController(ILogger<MainController> logger)
        {
            _logger = logger;
        }

        [HttpGet("/choices", Name = "GetChoices")]
        public IEnumerable<string> GetChoices()
        {
            return Enumerable.Empty<string>();
        }

        [HttpGet("/choice", Name = "GetChoice")]
        public IEnumerable<string> GetChoice()
        {
            return Enumerable.Empty<string>();
        }

        [HttpPost("/play", Name = "Play")]
        public IEnumerable<string> Play()
        {
            return Enumerable.Empty<string>();
        }
    }
}