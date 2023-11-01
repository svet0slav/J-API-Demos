using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TaxCalc.Api.ErrorHandling;
using TaxCalc.Interfaces.Requests;
using TaxCalc.Interfaces.Responses;
using TaxCalc.Services.Common.Dtos;
using TaxCalc.Services.Common.Interfaces;

namespace TaxCalc.Api.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Route("v{version:apiVersion}")]
    [Produces("application/json")]
    public class CalculatorController : ControllerBase
    {
        private readonly ICalculatorService _calculatorService;
        private readonly IMapper _mapper;
        private readonly ILogger<CalculatorController> _logger;

        public CalculatorController(ICalculatorService calculatorService,
            IMapper mapper,
            ILogger<CalculatorController> logger)
        {
            _calculatorService = calculatorService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost("calculate", Name = "Calculate")]
        [MapToApiVersion("1")]
        [ProducesResponseType(statusCode: 200, type: typeof(TaxesResponse))]
        [ProducesResponseType(statusCode: 400, type: typeof(ErrorDetails))]
        [ProducesResponseType(statusCode: 404, type: typeof(ErrorDetails))]
        [ProducesResponseType(statusCode: 500, type: typeof(ErrorDetails))]
        public async Task<ActionResult<TaxesResponse>> Calculate([FromBody] TaxPayerRequest request, CancellationToken cancellationToken)
        {
            //Validate the request
            //TODO

            //Convert to Dto
            var requestDto = _mapper.Map<TaxPayerRequest, TaxPayerDto>(request);

            var result = await _calculatorService.Calculate(requestDto, cancellationToken);
            if (result == null)
            {
                return BadRequest();
            }
            
            var response = _mapper.Map<TaxesDto, TaxesResponse>(result);
            return Ok(response);
        }
    }
}