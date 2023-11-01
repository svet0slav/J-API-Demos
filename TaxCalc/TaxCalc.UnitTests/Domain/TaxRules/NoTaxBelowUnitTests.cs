using TaxCalc.Domain.Data;
using TaxCalc.Domain.TaxRules;

namespace TaxCalc.UnitTests.Domain.TaxRules
{
    [TestClass]
    public class NoTaxBelowUnitTests
    {
        private TaxPayer _payer;
        private TaxesData _taxesData;

        public NoTaxBelowUnitTests() {
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
            var rule = new NoTaxBelow(1000);

            var actual = rule.CalculateTax(_payer, _taxesData);

            Assert.IsNotNull(actual);
            Assert.AreEqual(0, _taxesData.IncomeTax);
        }

        [TestMethod]
        public void Normal_LargeIncome_Ok()
        {
            var rule = new NoTaxBelow(10);
            _taxesData.IncomeTax = 999;

            var actual = rule.CalculateTax(_payer, _taxesData);

            Assert.IsNotNull(actual);
            Assert.AreEqual(999, _taxesData.IncomeTax); // not changed
        }
    }
}
