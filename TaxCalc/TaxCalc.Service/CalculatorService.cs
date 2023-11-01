using AutoMapper;
using Microsoft.Extensions.Logging;
using System.IO.MemoryMappedFiles;
using TaxCalc.Domain.Calculate;
using TaxCalc.Domain.Data;
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

        public async Task<TaxesDto> Calculate(TaxPayerDto taxPayer)
        {
            var payer = _mapper.Map<TaxPayerDto, TaxPayer>(taxPayer);

            _logger.LogInformation($"TaxPayer {taxPayer.FullName} calculation with income {taxPayer.GrossIncome}");

            _taxCalculator.LoadRules(_jurisdictionConfiguration);

            var taxesData = await _taxCalculator.Calculate(payer);

            var result = _mapper.Map<TaxesData, TaxesDto>(taxesData);

            _logger.LogInformation($"TaxPayer {taxPayer.FullName} total tax {result.TotalTax}");

            return result;
        }
    }
}