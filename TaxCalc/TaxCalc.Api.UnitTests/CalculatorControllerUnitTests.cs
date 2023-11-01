using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using TaxCalc.Api.Controllers;
using TaxCalc.Services.Common.Interfaces;
using AutoMapper;
using TaxCalc.Interfaces.Requests;
using TaxCalc.Interfaces.Responses;
using TaxCalc.Services.Common.Dtos;

namespace TaxCalc.Api.UnitTests
{
    [TestClass]
    public class CalculatorControllerUnitTests
    {
        private Mock<ICalculatorService> _service;
        private Mock<IMapper> _mapper;
        private CalculatorController _calculatorController;
        private Mock<ILogger<CalculatorController>> _logger;

        [TestInitialize]
        public void Setup()
        {
            _logger = new Mock<ILogger<CalculatorController>>();
            _service = new Mock<ICalculatorService>(MockBehavior.Strict);
            _mapper = new Mock<IMapper>(MockBehavior.Strict);

            _calculatorController = new CalculatorController(_service.Object,
                _mapper.Object, _logger.Object);
        }

        [TestMethod]
        public async Task CalculatorController_Choices_Ok()
        {
            var testRequest = FakeTaxRequest();
            var testDto = FakeTaxDtoResponse();
            var testResponse = FakeTaxResponse();
            _mapper.Setup(m => m.Map<TaxPayerRequest, TaxPayerDto>(
                It.IsAny<TaxPayerRequest>()))
                .Returns(new TaxPayerDto() { FullName = "test", GrossIncome = 100 });
            _mapper.Setup(m => m.Map<TaxesDto, TaxesResponse>(
                It.IsAny<TaxesDto>()))
                .Returns(testResponse);

            _service.Setup(s => s.Calculate(It.IsAny<TaxPayerDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(testDto ?? new TaxesDto());

            var actual = await _calculatorController.Calculate(testRequest, CancellationToken.None);

            _service.Verify(s => s.Calculate(It.IsAny<TaxPayerDto>(), It.IsAny<CancellationToken>()),
                Times.Once);
            _mapper.Verify(m => m.Map<TaxPayerRequest, TaxPayerDto>(
                It.IsAny<TaxPayerRequest>()), Times.Once);
            _mapper.Verify(m => m.Map<TaxesDto, TaxesResponse>(
                            It.IsAny<TaxesDto>()), Times.Once);


            Assert.IsNotNull(actual);
            Assert.IsNotNull(actual.Result);
            Assert.IsInstanceOfType(actual.Result, typeof(OkObjectResult));
            var actualValue = ((OkObjectResult)actual.Result).Value;
            Assert.IsNotNull(actualValue);
            Assert.IsInstanceOfType(actualValue, typeof(TaxesResponse));
            var value = (actualValue as TaxesResponse);
            Assert.AreEqual(testResponse.IncomeTax, value?.IncomeTax);
            Assert.AreEqual(testResponse.SocialTax, value?.SocialTax);
            Assert.AreEqual(testResponse.TotalTax, value?.TotalTax);
        }

        private TaxPayerRequest FakeTaxRequest()
        {
            return new TaxPayerRequest() { FullName = "test", GrossIncome = 100 };
        }
        private TaxesDto FakeTaxDtoResponse()
        {
            return new TaxesDto() { GrossIncome = 100, IncomeTax = 0, TotalTax = 0, NetIncome = 100 };
        }
        private TaxesResponse FakeTaxResponse()
        {
            return new TaxesResponse() { GrossIncome = 100, IncomeTax = 0, TotalTax = 0, NetIncome = 100 };
        }
    }
}