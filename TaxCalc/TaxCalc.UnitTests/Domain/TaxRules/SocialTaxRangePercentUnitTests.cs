using TaxCalc.Domain.Data;
using TaxCalc.Domain.TaxRules;

namespace TaxCalc.UnitTests.Domain.TaxRules
{
    [TestClass]
    public class SocialTaxRangePercentUnitTests
    {
        private TaxPayer _payer;
        private TaxesData _taxesData;

        public SocialTaxRangePercentUnitTests()
        {
            _payer = new TaxPayer()
            {
                GrossIncome = 100,
                CharitySpent = 10,
            };
            _taxesData = new TaxesData()
            {
                GrossIncome = 100, 
                CharitySpent = 10, 
                WorkingTaxIncome = 100 
            };
        }

        [TestMethod]
        public void Normal_SmallIncome_Ok()
        {
            var rule = new SocialTaxRangePercent(1000, 3000, 0.15m);

            var actual = rule.CalculateTax(_payer, _taxesData);

            Assert.IsNotNull(actual);
            Assert.AreEqual(0, _taxesData.SocialTax);
        }

        [TestMethod]
        public void Normal_LargeIncome_Ok()
        {
            var rule = new SocialTaxRangePercent(1000, 3000, 0.15m);

            _taxesData.WorkingTaxIncome = 1500;

            var actual = rule.CalculateTax(_payer, _taxesData);

            Assert.IsNotNull(actual);
            Assert.AreEqual(75m, _taxesData.SocialTax);
        }

        [TestMethod]
        public void Normal_LargeIncomeBorder_Ok()
        {
            var rule = new SocialTaxRangePercent(1000, 3000, 0.15m);

            _taxesData.WorkingTaxIncome = 4000;

            var actual = rule.CalculateTax(_payer, _taxesData);

            Assert.IsNotNull(actual);
            Assert.AreEqual(300, _taxesData.SocialTax);
        }
    }
}
