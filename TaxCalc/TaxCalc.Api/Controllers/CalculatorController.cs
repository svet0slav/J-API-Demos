using AutoMapper;
using IdempotentAPI.Filters;
using Microsoft.AspNetCore.Mvc;
using TaxCalc.Api.ErrorHandling;
using TaxCalc.Interfaces.Requests;
using TaxCalc.Interfaces.Responses;
using TaxCalc.Services.Common.Dtos;
using TaxCalc.Services.Common.Interfaces;
using Microsoft.Extensions.Logging;

namespace TaxCalc.Api.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Route("v{version:apiVersion}")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [Idempotent(Enabled = true, ExpireHours = 1, CacheOnlySuccessResponses = true)]
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
        [Idempotent(CacheOnlySuccessResponses = true, 
            DistributedCacheKeysPrefix = "Calculate", 
            Enabled = true, ExpireHours = 1)]
        [ProducesResponseType(statusCode: 200, type: typeof(TaxesResponse))]
        [ProducesResponseType(statusCode: 400, type: typeof(ErrorDetails))]
        [ProducesResponseType(statusCode: 404, type: typeof(ErrorDetails))]
        [ProducesResponseType(statusCode: 500, type: typeof(ErrorDetails))]
        public async Task<ActionResult<TaxesResponse>> Calculate([FromBody] TaxPayerRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Request received", request);

            //Validate the request is automatic from FluentValidation.            

            //Convert to Dto
            var requestDto = _mapper.Map<TaxPayerRequest, TaxPayerDto>(request);

            var result = await _calculatorService.Calculate(requestDto, cancellationToken);
            if (result == null)
            {
                return BadRequest();
            }
            
            var response = _mapper.Map<TaxesDto, TaxesResponse>(result);
            _logger.LogInformation("Request processed", response);
            return Ok(response);
        }
    }
}