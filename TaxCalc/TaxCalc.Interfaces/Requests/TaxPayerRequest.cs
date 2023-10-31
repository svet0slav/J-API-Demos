using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TaxCalc.Interfaces.Requests
{
    public class TaxPayerRequest
    {
        [JsonPropertyName("fullName")]
        public string FullName { get; set; }

        [JsonPropertyName("ssn")]
        public string SSN { get; set; }

        [JsonPropertyName("grossIncome")]
        public decimal GrossIncome { get; set; }

        [JsonPropertyName("charitySpent")]
        public decimal CharitySpent { get; set; }

        [JsonPropertyName("dateOfBirth")]
        public string DateOfBirth { get; set; }
    }
}
