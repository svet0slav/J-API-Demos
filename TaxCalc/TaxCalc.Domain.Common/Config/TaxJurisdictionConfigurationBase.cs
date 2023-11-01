namespace TaxCalc.Domain.Common.Config
{
    public class TaxJurisdictionConfigurationBase
    {
        public string Jusriction { get; set; }

        public TaxJurisdictionConfigurationBase()
        {
            Jusriction = string.Empty;
        }
    }
}
