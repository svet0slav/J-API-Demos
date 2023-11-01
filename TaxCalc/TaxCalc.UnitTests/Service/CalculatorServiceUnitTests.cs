using Microsoft.Extensions.Logging;
using Moq;
using TaxCalc.Domain.Calculate;
using TaxCalc.Domain.Data;
using TaxCalc.Service;
using TaxCalc.Services.Common.Dtos;

namespace TaxCalc.UnitTests.Service
{
    [TestClass]
    public class CalculatorServiceUnitTests
    {
        private ITaxJurisdictionConfiguration _configuration;
        private Mock<ITaxesCalculator> _calculator;
        private Mock<ILogger<CalculatorService>> _logger;

        [TestInitialize]
        public void TestInitialize()
        {
            _configuration = GetConfiguration();
            _calculator = new Mock<ITaxesCalculator> { CallBase = true };
            _calculator.Setup(c => c.LoadRules(It.IsAny<ITaxJurisdictionConfiguration>()))
                .Verifiable();
            _calculator.Setup(c => c.Calculate(It.IsAny<TaxPayer>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new TaxesData())
                .Verifiable();

            _logger = new Mock<ILogger<CalculatorService>>();
        }

        [TestMethod]
        public async Task Service_AllSetup_CorrectCallsAsync()
        {
            var service = new CalculatorService(_configuration, _calculator.Object, _logger.Object);

            var taxPayerDto = new TaxPayerDto()
            {
                FullName = "test",
                GrossIncome = 1500,
                CharitySpent = 100
            };
            var taxesOutput = new TaxesData()
            {
                GrossIncome = 1500,
                WorkingTaxIncome = 1400,
                IncomeTax = 140,
                TotalTax = 140,
                NetIncome = 1360
            };
            // Setup the result, so it is working normally
            _calculator.Setup(s => s.Calculate(It.IsAny<TaxPayer>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(taxesOutput)
                .Verifiable();

            // Throws no exception
            var result = await service.Calculate(taxPayerDto, CancellationToken.None);

            Assert.IsNotNull(result);
            Assert.AreEqual(taxesOutput.GrossIncome, result.GrossIncome);
            Assert.AreEqual(taxesOutput.IncomeTax, result.IncomeTax);
            Assert.AreEqual(taxesOutput.TotalTax, result.TotalTax);
            Assert.AreEqual(taxesOutput.NetIncome, result.NetIncome);
            _calculator.Verify(c => c.Calculate(It.IsAny<TaxPayer>(), It.IsAny<CancellationToken>()), Times.Once);
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
