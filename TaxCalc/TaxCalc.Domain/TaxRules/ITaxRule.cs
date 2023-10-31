using TaxCalc.Domain.Data;

namespace TaxCalc.Domain.TaxRules
{
    public interface ITaxRule
    {
        TaxesData CalculateTax(TaxPayer Payer, TaxesData input);
    }
}
