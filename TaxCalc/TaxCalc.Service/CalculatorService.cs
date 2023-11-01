using AutoMapper;
using Microsoft.Extensions.Logging;
using TaxCalc.Domain.Calculate;
using TaxCalc.Domain.Common.Config;
using TaxCalc.Domain.Data;
using TaxCalc.Service.Common;
using TaxCalc.Services.Common.Dtos;
using TaxCalc.Services.Common.Interfaces;

namespace TaxCalc.Service
{
    public class CalculatorService : ICalculatorService
    {
        private readonly ITaxJurisdictionConfiguration _jurisdictionConfiguration;
        private readonly ITaxesCalculator _taxCalculator;
        private readonly IMapper _mapper;
        private readonly ILogger<CalculatorService> _logger;

        public CalculatorService(ITaxJurisdictionConfiguration jurisdictionConfiguration,
            ITaxesCalculator taxCalculator,
            ILogger<CalculatorService> logger)
        {
            _jurisdictionConfiguration = jurisdictionConfiguration;
            _taxCalculator = taxCalculator;
            _mapper = Mappers.MappersConfiguration.CreateMappersConfiguration();
            _logger = logger;
        }

        public async Task<TaxesDto> Calculate(TaxPayerDto taxPayer, CancellationToken cancellationToken)
        {
            if (taxPayer == null)
                throw new ArgumentNullException(nameof(taxPayer), "Missing tax payer");

            _logger.LogInformation($"TaxPayer {taxPayer.FullName} calculation with income {taxPayer.GrossIncome}");

            try
            {
                var payer = _mapper.Map<TaxPayerDto, TaxPayer>(taxPayer);

                _taxCalculator.LoadRules(_jurisdictionConfiguration);

                var taxesData = await _taxCalculator.Calculate(payer, cancellationToken);

                if (taxesData == null)
                {
                    throw new ServiceException("No tax calculated");
                }

                var result = _mapper.Map<TaxesData, TaxesDto>(taxesData);

                _logger.LogInformation($"TaxPayer {taxPayer.FullName} total tax {result.TotalTax}");

                return result;
            }
            catch (Exception ex)
            {
                throw new ServiceException("Failed to calculate tax", ex);
            }
        }
    }
}