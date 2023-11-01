using System.Reflection.Metadata;
using System.Runtime.ConstrainedExecution;
using System.Text.RegularExpressions;
using System;
using TaxCalc.Domain.Calculate;
using TaxCalc.Domain.Data;
using TaxCalc.Domain.TaxRules;

namespace TaxCalc.UnitTests.Domain.Calculate
{
    [TestClass]
    public class TaxesCalculatorUnitTests
    {
        private ITaxJurisdictionConfiguration _configuration;
        private ITaxesCalculator _calculator;

        public TaxesCalculatorUnitTests()
        {
            _configuration = GetConfiguration();
            _calculator = new TaxesCalculator(_configuration);
            _calculator.LoadRules();
        }

        /// <summary>
        /// Example 1: George has a salary of 980 IDR.He would pay no taxes since this is below the taxation threshold and his net income is also 980.
        /// </summary>
        [TestMethod]
        public void Example1_Ok()
        {
            var payer = new TaxPayer()
            {
                GrossIncome = 980,
            };

            var actual = _calculator.Calculate(payer);

            Assert.IsNotNull(actual);
            Assert.AreEqual(0, actual.IncomeTax);
            Assert.AreEqual(0, actual.SocialTax);
            Assert.AreEqual(0, actual.TotalTax);
            Assert.AreEqual(980, actual.NetIncome);
        }

        /// <summary>
        /// Example 2: Irina has a salary of 3400 IDR.
        /// She owns income tax: 10% out of 2400 => 240. 
        /// Her Social contributions are 15% out of 2000 => 300. 
        /// In total her tax is 540 and she gets to bring home 2860 IDR
        /// </summary>
        [TestMethod]
        public void Example2_Ok()
        {
            var payer = new TaxPayer()
            {
                GrossIncome = 3400m,
            };

            var actual = _calculator.Calculate(payer);

            Assert.IsNotNull(actual);
            Assert.AreEqual(240m, actual.IncomeTax);
            Assert.AreEqual(300m, actual.SocialTax);
            Assert.AreEqual(540m, actual.TotalTax);
            Assert.AreEqual(2860m, actual.NetIncome);
        }

        /// <summary>
        /// Example 3: Mick has a salary of 2500 IDR.
        /// He has spent 150 IDR on charity causes during the year.
        /// His taxable gross income is 1500 – 150 = 1350 IDR owns income tax: 10% out of 1350 => 135. His Social contributions are 15% out of 1350 => 202.5. In total her tax is 337.5 and he gets to bring home 2162.5 IDR
        /// </summary>
        [TestMethod]
        public void Example3_Ok()
        {
            var payer = new TaxPayer()
            {
                GrossIncome = 2500,
                CharitySpent = 150,
            };

            var actual = _calculator.Calculate(payer);

            Assert.IsNotNull(actual);
            Assert.AreEqual(2350m, actual.WorkingTaxIncome);
            Assert.AreEqual(135m, actual.IncomeTax);
            Assert.AreEqual(202.50m, actual.SocialTax);
            Assert.AreEqual(337.50m, actual.TotalTax);
            Assert.AreEqual(2162.50m, actual.NetIncome);
        }


        /// <summary>
        /// Example 4: Bill has a salary of 3600 IDR.
        /// He has spent 520 IDR on charity causes during the year.
        /// His taxable gross income is 3600 – 360 = 3240 IDR owns income tax: 10% out of 2240 => 224.
        /// His Social contributions are 15% out of 2000 => 300. In total her tax is 524 and she gets to bring home 3076 IDR.
        /// </summary>
        [TestMethod]
        public void Example4_Ok()
        {
            var payer = new TaxPayer()
            {
                GrossIncome = 3600,
                CharitySpent = 520,
            };

            var actual = _calculator.Calculate(payer);

            Assert.IsNotNull(actual);
            Assert.AreEqual(3240, actual.WorkingTaxIncome);
            Assert.AreEqual(224, actual.IncomeTax);
            Assert.AreEqual(300, actual.SocialTax);
            Assert.AreEqual(524, actual.TotalTax);
            Assert.AreEqual(3076, actual.NetIncome);
        }

        [TestMethod]
        public void Normal_ZeroIncome_Ok()
        {
            var payer = new TaxPayer()
            {
                GrossIncome = 0,
                CharitySpent = 0,
            };

            var actual = _calculator.Calculate(payer);

            Assert.IsNotNull(actual);
            Assert.AreEqual(0, actual.WorkingTaxIncome);
            Assert.AreEqual(0, actual.IncomeTax);
            Assert.AreEqual(0, actual.SocialTax);
            Assert.AreEqual(0, actual.TotalTax);
            Assert.AreEqual(0, actual.NetIncome);
        }

        [TestMethod]
        public void Normal_LargeIncomeBorder_Ok()
        {
            var payer = new TaxPayer()
            {
                GrossIncome = 50000m,
                CharitySpent = 10000m,
            };

            var actual = _calculator.Calculate(payer);

            Assert.IsNotNull(actual);
            Assert.AreEqual(45000m, actual.WorkingTaxIncome);
            Assert.AreEqual(4400m, actual.IncomeTax);
            Assert.AreEqual(300m, actual.SocialTax);
            Assert.AreEqual(4700m, actual.TotalTax);
            Assert.AreEqual(45300m, actual.NetIncome);
        }

        private ITaxJurisdictionConfiguration GetConfiguration()
        {
            return new TaxJurisdictionConfiguration()
            {
                Jusriction = "task",
                NoTaxMinAmount = 1000m,
                IncomeTaxMinAmount = 1000m,
                IncomeTaxMaxAmount = decimal.MaxValue,
                IncomeTaxPercent = 0.10m,
                SocialTaxMinAmount = 1000m,
                SocialTaxMaxAmount = 3000m,
                SocialTaxPercent = 0.15m,
                CharityFreePercent = 0.10m
            };
        }
    }
}
