using TaxCalc.Domain.Data;

namespace TaxCalc.Domain.TaxRules
{
    internal abstract class TaxRuleBase: ITaxRule
    {
        public TaxRuleBase() {
        }

        public abstract TaxesData CalculateTax(TaxPayer taxPayer, TaxesData input);
    }
}
