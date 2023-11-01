using TaxCalc.Domain.Data;
using TaxCalc.Domain.TaxRules;

namespace TaxCalc.UnitTests.Domain.TaxRules
{
    [TestClass]
    public class TaxesCompleteUnitTests
    {
        private TaxPayer _payer;
        private TaxesData _taxesData;

        public TaxesCompleteUnitTests()
        {
            _payer = new TaxPayer()
            {
                GrossIncome = 1500,
                CharitySpent = 10
            };
            _taxesData = new TaxesData()
            {
                GrossIncome = 1500,
                CharitySpent = 10,
                WorkingTaxIncome = 1490,
                IncomeTax = 50,
                SocialTax = 75,
                TotalTax = 0,
                NetIncome = 0
            };
        }

        [TestMethod]
        public void Normal_Income_Ok()
        {
            var rule = new TaxesComplete();
           
            var actual = rule.CalculateTax(_payer, _taxesData);

            Assert.IsNotNull(actual);
            Assert.AreEqual(_payer.GrossIncome, _taxesData.GrossIncome);
            Assert.AreEqual(_payer.CharitySpent, _taxesData.CharitySpent);
            Assert.AreEqual(_payer.GrossIncome - _payer.CharitySpent, _taxesData.WorkingTaxIncome);
            Assert.AreEqual(50, _taxesData.IncomeTax);
            Assert.AreEqual(75, _taxesData.SocialTax);
            Assert.AreEqual(125, _taxesData.TotalTax);
            Assert.AreEqual(1375, _taxesData.NetIncome);
        }
    }
}
