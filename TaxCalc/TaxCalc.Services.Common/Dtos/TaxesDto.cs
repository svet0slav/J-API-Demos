using System;

namespace TaxCalc.Services.Common.Dtos
{
    public class TaxesDto
    {
        public decimal GrossIncome { get; set; }

        public decimal CharitySpent { get; set; }

        public decimal IncomeTax { get; set; }

        public decimal SocialTax { get; set; }

        public decimal TotalTax { get; set; }

        public decimal NetIncome { get; set; }
    }
}
