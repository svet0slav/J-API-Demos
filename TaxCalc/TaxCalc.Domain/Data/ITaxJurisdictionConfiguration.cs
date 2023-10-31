namespace TaxCalc.Domain.Data
{
    public interface ITaxJurisdictionConfiguration
    {
        string Jusriction { get; }
        decimal CharityFreePercent { get; }
        decimal IncomeTaxMaxAmount { get; }
        decimal IncomeTaxMinAmount { get; }
        decimal IncomeTaxPercent { get; }
        decimal NoTaxMinAmount { get; }
        decimal SocialTaxMaxAmount { get; }
        decimal SocialTaxMinAmount { get; }
        decimal SocialTaxPercent { get; }
    }
}
