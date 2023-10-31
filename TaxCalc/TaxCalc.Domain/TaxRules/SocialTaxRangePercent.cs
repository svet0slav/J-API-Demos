using TaxCalc.Domain.Data;

namespace TaxCalc.Domain.TaxRules
{
    /// <summary>
    /// 3.)	Social contributions of 15% are expected to be made as well. 
    /// As for the previous case, the taxable income is whatever is above 1000 IDR 
    /// but social contributions never apply to amounts higher than 3000.
    /// </summary>
    internal class SocialTaxRangePercent : TaxRuleBase
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
        public SocialTaxRangePercent(
            decimal minAmountIncl,
            decimal maxAmountIncl,
            decimal percent)
        : base()
        {
            _minAmountIncl = minAmountIncl;
            _maxAmount = maxAmountIncl;
            _percent = percent;
        }

        public override TaxesData CalculateTax(TaxPayer taxPayer, TaxesData input)
        {
            var result = input;
            if (input.WorkingTaxIncome <= _minAmountIncl)
            {
                result.SocialTax = 0;
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
