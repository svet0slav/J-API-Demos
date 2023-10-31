using TaxCalc.Domain.Data;

namespace TaxCalc.Domain.TaxRules
{
    /// <summary>
    /// 1.)	There is no taxation for any amount lower or equal to 1000 Imagiaria Dollars (IDR).
    /// </summary>
    internal class NoTaxBelow : TaxRuleBase
    {
        private readonly decimal _amount;

        public NoTaxBelow(decimal limit)
        : base()
        { 
            _amount = limit;
        }

        public override TaxesData CalculateTax(TaxPayer taxPayer, TaxesData input)
        {
            var result = input;
            if (input.WorkingTaxIncome <= _amount) {
                result.IncomeTax = 0;
            }
            
            return result;
        }
    }
}
