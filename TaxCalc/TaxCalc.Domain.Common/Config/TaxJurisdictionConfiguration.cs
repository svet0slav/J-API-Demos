namespace TaxCalc.Domain.Common.Config
{
    public class TaxJurisdictionConfiguration : TaxJurisdictionConfigurationBase, ITaxJurisdictionConfiguration
    {
        public decimal NoTaxMinAmount { get; set; }
        public decimal IncomeTaxMinAmount { get; set; }
        public decimal IncomeTaxMaxAmount { get; set; }
        public decimal IncomeTaxPercent { get; set; }
        public decimal SocialTaxMinAmount { get; set; }
        public decimal SocialTaxMaxAmount { get; set; }
        public decimal SocialTaxPercent { get; set; }
        public decimal CharityFreePercent { get; set; }

        public TaxJurisdictionConfiguration()
        : base()
        {
            NoTaxMinAmount = 0;
            IncomeTaxMinAmount = 0;
            IncomeTaxMaxAmount = decimal.MaxValue;
            IncomeTaxPercent = 0;
            SocialTaxMinAmount = 0;
            SocialTaxMaxAmount = decimal.MaxValue;
            SocialTaxPercent = 0;
            CharityFreePercent = 0;
        }
    }
}
