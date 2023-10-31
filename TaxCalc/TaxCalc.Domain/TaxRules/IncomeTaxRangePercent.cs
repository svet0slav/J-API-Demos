using TaxCalc.Domain.Data;

namespace TaxCalc.Domain.TaxRules
{
    /// <summary>
    /// 2.)	Income tax of 10% is incurred to the excess (amount above 1000).
    /// This rule is for percent from income to the excess above given limit until upper max-limit.
    /// This adds the amount of tax to the input.
    /// </summary>
    internal class IncomeTaxRangePercent: TaxRuleBase
    {
        private readonly decimal _minAmountIncl;
        private readonly decimal _maxAmount;
        private readonly decimal _percent;

        /// <summary>
        /// Initialize the percent of amount between two borders.
        /// </summary>
        /// <param name="minAmountIncl">The minimum amount from which (including it) starts the rule.</param>
        /// <param name="maxAmountIncl">The max amount where the rule stops (including it).</param>
        /// <param name="percent">The percent in normal (nonformatted) value.</param>
        public IncomeTaxRangePercent(
            decimal minAmountIncl, 
            decimal maxAmount,
            decimal percent)
        : base()
        {
            _minAmountIncl = minAmountIncl;
            _maxAmount = maxAmount;
            _percent = percent;
        }

        public override TaxesData CalculateTax(TaxPayer taxPayer, TaxesData input)
        {
            var result = input;
            if (input.WorkingTaxIncome <= _minAmountIncl)
            {
                // Unchanged
                return result;
            }

            var amount = input.WorkingTaxIncome >= _maxAmount
                ? _maxAmount - _minAmountIncl
                : input.WorkingTaxIncome - _minAmountIncl;

            result.IncomeTax += Math.Round(amount * _percent, 2);

            return result;
        }
    }
}
