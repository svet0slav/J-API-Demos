using System;

namespace TaxCalc.Services.Common.Dtos
{
    public class TaxPayerDto
    {
        public string FullName { get; set; }

        public string SSN { get; set; }

        public decimal GrossIncome { get; set; }

        public decimal CharitySpent { get; set; }

        public DateOnly DateOfBirth { get; set; }
    }
}
