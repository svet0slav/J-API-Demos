using AutoMapper;
using System.IO.MemoryMappedFiles;
using TaxCalc.Domain.Calculate;
using TaxCalc.Domain.Data;
using TaxCalc.Domain.TaxRules;
using TaxCalc.Services.Common.Dtos;
using TaxCalc.Services.Common.Interfaces;

namespace TaxCalc.Service
{
    public class CalculatorService : ICalculatorService
    {
        private readonly TaxJurisdictionConfiguration _jurisdictionConfiguration;
        private readonly ITaxesCalculator _taxCalculator;
        private readonly IMapper _mapper;

        public CalculatorService(TaxJurisdictionConfiguration jurisdictionConfiguration,
            ITaxesCalculator taxCalculator) {
            _jurisdictionConfiguration = jurisdictionConfiguration;
            _taxCalculator = taxCalculator;
            _mapper = Mappers.MappersConfiguration.CreateMappersConfiguration();
        }

        public async Task<TaxesDto> Calculate(TaxPayerDto taxPayer)
        {
            var taxes = new TaxesDto();

            // TODO: Use jurisdictionConfiguration.
            // The taxation rules in the country of Imaginaria as of date are as follows:
            //var listRules = new List<ITaxRule>() { 
            //    new TaxesCreate(taxPayer),
            //    new CharitySpent(taxPayer, 0.10),
            //    new NoTaxBelow(taxPayer, 1000),
            //    new IncomeTaxRangePercent(taxPayer, 1000, decimal.MaxValue, 0.10m),
            //    new SocialTaxRangePercent(taxPayer, 1000, 3000, 0.15m)
            //};

            var payer = _mapper.Map<TaxPayerDto, TaxPayer>(taxPayer);

            _taxCalculator.LoadRules(payer);

            var taxesData = _taxCalculator.Calculate(payer);

            var result = _mapper.Map<TaxesData, TaxesDto>(taxesData);
            
            return result;
        }
    }
}