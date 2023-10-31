using TaxCalc.Domain.Data;
using TaxCalc.Domain.TaxRules;

namespace TaxCalc.Domain.Calculate
{
    public interface ITaxesCalculator
    {
        IEnumerable<ITaxRule> Rules { get; }

        TaxesData Calculate(TaxPayer taxPayer);
        void LoadRules(TaxPayer taxPayer);
    }
}