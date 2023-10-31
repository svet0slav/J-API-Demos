using TaxCalc.Domain.Data;

namespace TaxCalc.Domain.TaxRules
{
    /// <summary>
    /// 4.)	CharitySpent – Up to 10% of the gross income is allowed to be spent for charity causes.
    /// It then needs to be subtracted from the gross income base before the taxes are calculated.
    /// It substracts the charity percent from gross income for tax purposes.
    /// </summary>
    internal class CharitySpentTaxPercent: TaxRuleBase
    {
        private readonly decimal _percent;

        /// <summary>
        /// Initialize the percent of amount for charity.
        /// </summary>
        /// <param name="taxPayer">The tax payer information.</param>
        /// <param name="percent">The percent in normal (nonformatted) value.</param>
        public CharitySpentTaxPercent(decimal percent)
        : base()
        {
            _percent = percent;
        }

        public override TaxesData CalculateTax(TaxPayer taxPayer, TaxesData input)
        {
            var result = input;

            if (input.CharitySpent > 0)
            {
                var maxDiscount = Math.Round(input.CharitySpent * _percent, 2);
                result.WorkingTaxIncome = Math.Max(input.WorkingTaxIncome - maxDiscount, 0);
            }

            return result;
        }
    }
}
