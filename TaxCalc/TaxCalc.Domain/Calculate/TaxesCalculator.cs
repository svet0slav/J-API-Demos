using TaxCalc.Domain.Common.Config;
using TaxCalc.Domain.Data;
using TaxCalc.Domain.TaxRules;

namespace TaxCalc.Domain.Calculate
{
    public class TaxesCalculator : ITaxesCalculator
    {
        public ITaxJurisdictionConfiguration? configuration { get; protected set; }
        
        public IEnumerable<ITaxRule> Rules { get; protected set; }

        public TaxesCalculator()
        {
            configuration = null;
            Rules = new List<ITaxRule>();
        }

        public void LoadRules(ITaxJurisdictionConfiguration configuration)
        {
            configuration = configuration;

            // The taxation rules in the country of Imaginaria as of date are as follows:
            var listRules = new List<ITaxRule>() {
                new TaxesCreate(),
                new CharitySpentTaxPercent(configuration.CharityFreePercent),
                new NoTaxBelow(configuration.NoTaxMinAmount),
                new IncomeTaxRangePercent(configuration.IncomeTaxMinAmount,
                    configuration.IncomeTaxMaxAmount, configuration.IncomeTaxPercent),
                new SocialTaxRangePercent(configuration.SocialTaxMinAmount,
                    configuration.SocialTaxMaxAmount, configuration.SocialTaxPercent),
                new TaxesComplete(),
            };

            Rules = listRules;
        }

        /// <summary>
        /// Calculate the taxes for given TaxPayer.
        /// </summary>
        /// <param name="taxPayer"></param>
        /// <returns></returns>
        public async Task<TaxesData> Calculate(TaxPayer taxPayer, CancellationToken cancellationToken)
        {
            var taxes = new TaxesData();

            //Implement the rules
            TaxesData input;
            TaxesData output = taxes;
            foreach (var rule in Rules)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return await Task.FromCanceled<TaxesData>(cancellationToken);
                }
                input = output;
                output = rule.CalculateTax(taxPayer, input);
            }

            // TODO: In future improve for multi-threading
            return await Task.FromResult(output);
        }
    }
}
