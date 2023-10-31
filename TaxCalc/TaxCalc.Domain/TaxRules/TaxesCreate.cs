using TaxCalc.Domain.Data;

namespace TaxCalc.Domain.TaxRules
{
    /// <summary>
    /// Create a taxes base with data from tax payer.
    /// </summary>
    internal class TaxesCreate: TaxRuleBase
    {
        public TaxesCreate()
        : base()
        { }

        public override TaxesData CalculateTax(TaxPayer taxPayer, TaxesData input)
        {
            var result = new TaxesData()
            {
                GrossIncome = taxPayer.GrossIncome,
                CharitySpent = taxPayer.CharitySpent,
                WorkingTaxIncome = taxPayer.GrossIncome
            };
            
            return result;
        }
    }
}
