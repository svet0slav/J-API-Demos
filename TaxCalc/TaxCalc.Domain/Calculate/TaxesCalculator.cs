using TaxCalc.Domain.Data;
using TaxCalc.Domain.TaxRules;

namespace TaxCalc.Domain.Calculate
{
    public class TaxesCalculator : ITaxesCalculator
    {
        private ITaxJurisdictionConfiguration? _configuration;
        public IEnumerable<ITaxRule> Rules { get; protected set; }

        public TaxesCalculator()
        {
            _configuration = null;
            Rules = new List<ITaxRule>();
        }

        public void LoadRules(ITaxJurisdictionConfiguration configuration)
        {
            _configuration = configuration;

            // The taxation rules in the country of Imaginaria as of date are as follows:
            var listRules = new List<ITaxRule>() {
                new TaxesCreate(),
                new CharitySpentTaxPercent(_configuration.CharityFreePercent),
                new NoTaxBelow(_configuration.NoTaxMinAmount),
                new IncomeTaxRangePercent(_configuration.IncomeTaxMinAmount,
                    _configuration.IncomeTaxMaxAmount, _configuration.IncomeTaxPercent),
                new SocialTaxRangePercent(_configuration.SocialTaxMinAmount,
                    _configuration.SocialTaxMaxAmount, _configuration.SocialTaxPercent),
                new TaxesComplete(),
            };

            Rules = listRules;
        }

        /// <summary>
        /// Calculate the taxes for given TaxPayer.
        /// </summary>
        /// <param name="taxPayer"></param>
        /// <returns></returns>
        public async Task<TaxesData> Calculate(TaxPayer taxPayer)
        {
            var taxes = new TaxesData();

            //Implement the rules
            TaxesData input;
            TaxesData output = taxes;
            foreach (var rule in Rules)
            {
                input = output;
                output = rule.CalculateTax(taxPayer, input);
            }

            // TODO: In future improve for multi-threading
            return await Task.FromResult(output);
        }
    }
}
