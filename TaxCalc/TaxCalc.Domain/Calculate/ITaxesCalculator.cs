using TaxCalc.Domain.Common.Config;
using TaxCalc.Domain.Data;
using TaxCalc.Domain.TaxRules;

namespace TaxCalc.Domain.Calculate
{
    public interface ITaxesCalculator
    {
        IEnumerable<ITaxRule> Rules { get; }

        Task<TaxesData> Calculate(TaxPayer taxPayer, CancellationToken cancellationToken);
        void LoadRules(ITaxJurisdictionConfiguration configuration);
    }
}