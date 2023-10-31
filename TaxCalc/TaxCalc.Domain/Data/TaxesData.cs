namespace TaxCalc.Domain.Data
{
    /// <summary>
    /// Class for taxes data internally for the domain.
    /// </summary>
    public class TaxesData
    {
        public decimal GrossIncome { get; set; }

        public decimal CharitySpent { get; set; }

        public decimal WorkingTaxIncome { get; set; }

        public decimal IncomeTax { get; set; }

        public decimal SocialTax { get; set; }

        public decimal TotalTax { get; set; }

        public decimal NetIncome { get; set; }
    }
}
