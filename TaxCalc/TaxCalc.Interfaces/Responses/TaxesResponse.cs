using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TaxCalc.Interfaces.Responses
{
    public class TaxesResponse
    {
        [JsonPropertyName("grossIncome")]
        public decimal GrossIncome { get; set; }

        [JsonPropertyName("charitySpent")]
        public decimal CharitySpent { get; set; }

        [JsonPropertyName("incomeText")]
        public decimal IncomeTax { get; set; }

        [JsonPropertyName("socialTax")]
        public decimal SocialTax { get; set; }

        [JsonPropertyName("totalTax")]
        public decimal TotalTax { get; set; }

        [JsonPropertyName("netIncome")]
        public decimal NetIncome { get; set; }
    }
}
