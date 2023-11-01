using TaxCalc.Domain.Data;
using TaxCalc.Domain.TaxRules;

namespace TaxCalc.UnitTests.Domain.TaxRules
{
    [TestClass]
    public class CharitySpentTaxPercentUnitTests
    {
        private TaxPayer _payer;
        private TaxesData _taxesData;

        public CharitySpentTaxPercentUnitTests()
        {
            _payer = new TaxPayer()
            {
                GrossIncome = 100,
                CharitySpent = 10,
            };
            _taxesData = new TaxesData() { GrossIncome = 100, CharitySpent = 10, WorkingTaxIncome = 100 };
        }

        [TestMethod]
        public void Normal_SmallIncome_Ok()
        {
            var rule = new IncomeTaxRangePercent(1000, 3000, 0.10m);

            var actual = rule.CalculateTax(_payer, _taxesData);

            Assert.IsNotNull(actual);
            Assert.AreEqual(0, _taxesData.IncomeTax);
        }

        [TestMethod]
        public void Normal_LargeIncome_Ok()
        {
            var rule = new IncomeTaxRangePercent(10, 3000, 0.10m);

            _taxesData.WorkingTaxIncome = 100;

            var actual = rule.CalculateTax(_payer, _taxesData);

            Assert.IsNotNull(actual);
            Assert.AreEqual(9, _taxesData.IncomeTax);
        }

        [TestMethod]
        public void Normal_LargeIncomeBorder_Ok()
        {
            var rule = new IncomeTaxRangePercent(1000, 3000, 0.10m);

            _taxesData.WorkingTaxIncome = 3000;

            var actual = rule.CalculateTax(_payer, _taxesData);

            Assert.IsNotNull(actual);
            Assert.AreEqual(200, _taxesData.IncomeTax);
        }
    }
}
