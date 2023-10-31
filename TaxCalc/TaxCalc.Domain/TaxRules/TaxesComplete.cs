using TaxCalc.Domain.Data;

namespace TaxCalc.Domain.TaxRules
{
    /// <summary>
    /// Finalize the taxes calculations with data from the resulting calculations.
    /// </summary>
    internal class TaxesComplete : TaxRuleBase
    {
        public TaxesComplete(TaxPayer taxPayer)
        : base()
        { }

        public override TaxesData CalculateTax(TaxPayer taxPayer, TaxesData input)
        {
            var result = input;
            result.TotalTax = result.IncomeTax + result.SocialTax;

            return result;
        }
    }
}
