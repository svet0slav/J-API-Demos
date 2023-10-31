namespace TaxCalc.Domain.Data
{
    public class TaxPayer
    {
        public string FullName {  get; set; }

        public string SSN { get; set;}

        public decimal GrossIncome { get; set; }

        public decimal CharitySpent { get; set; }

        public DateOnly DateOfBirth { get; set; }

        public TaxPayer()
        {
            FullName = string.Empty;
            SSN = string.Empty;
        }
    }
}
