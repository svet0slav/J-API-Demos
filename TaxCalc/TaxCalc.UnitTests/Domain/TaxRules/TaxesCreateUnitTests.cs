using TaxCalc.Domain.Data;
using TaxCalc.Domain.TaxRules;

namespace TaxCalc.UnitTests.Domain.TaxRules
{
    [TestClass]
    public class TaxesCreateUnitTests
    {
        private TaxPayer _payer;
        private TaxesData _taxesData;

        public TaxesCreateUnitTests()
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
        public void Normal_Income_Ok()
        {
            var rule = new TaxesCreate();

            var actual = rule.CalculateTax(_payer, _taxesData);

            Assert.IsNotNull(actual);
            Assert.AreEqual(0, _taxesData.IncomeTax);
            Assert.AreEqual(0, _taxesData.SocialTax);
            Assert.AreEqual(_payer.GrossIncome, _taxesData.GrossIncome);
            Assert.AreEqual(_payer.GrossIncome, _taxesData.WorkingTaxIncome);
            Assert.AreEqual(_payer.CharitySpent, _taxesData.CharitySpent);
            Assert.AreEqual(0, _taxesData.TotalTax);
            Assert.AreEqual(0, _taxesData.NetIncome);
        }
    }
}
