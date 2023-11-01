using FluentValidation;
using FluentValidation.TestHelper;
using TaxCalc.Api.Validators;
using TaxCalc.Interfaces.Requests;

namespace TaxCalc.Api.UnitTests
{
    [TestClass]
    public class TaxPayerRequestValidatorUnitTests
    {
        [TestMethod]
        public void Payer_MissingFullName_ReturnsValidationError()
        {
            TaxPayerRequestValidator validator = new();
            TaxPayerRequest request = FakeGoodRequest();
            request.FullName = "";
            var result = validator.Validate(request);

            Assert.IsFalse(result.IsValid); // .ShouldHaveValidationErrorFor(e => e.FullName, request);
        }

        [TestMethod]
        [DataRow("ab")]
        [DataRow("a b")]
        [DataRow("aston Berry-Kolev")]
        public void Payer_BadFullName_ReturnsValidationError(string name)
        {
            TaxPayerRequestValidator validator = new();
            TaxPayerRequest request = FakeGoodRequest();
            request.FullName = name;
            var result = validator.Validate(request);

            Assert.IsFalse(result.IsValid); // validator.ShouldHaveValidationErrorFor(e => e.FullName, request);
        }

        [TestMethod]
        [DataRow("Aston Berry")]
        [DataRow("Aston Berry Ivan")]
        [DataRow("Ivan Petrov Kolev")]
        public void Payer_GoodFullName_ReturnsNoValidationError(string name)
        {
            TaxPayerRequestValidator validator = new();
            TaxPayerRequest request = FakeGoodRequest();
            request.FullName = name;
            var result = validator.Validate(request);

            Assert.IsTrue(result.IsValid); // validator.ShouldNotHaveValidationErrorFor(e => e.FullName, request);
        }

        [TestMethod]
        [DataRow("ab")]
        [DataRow("1234")]
        [DataRow("12-berry-3435345")]
        [DataRow("123456789011")]
        public void Payer_BadSSN_ReturnsValidationError(string data)
        {
            TaxPayerRequestValidator validator = new();
            TaxPayerRequest request = FakeGoodRequest();
            request.SSN = data;
            var result = validator.Validate(request);

            Assert.IsFalse(result.IsValid); // validator.ShouldHaveValidationErrorFor(e => e.SSN, request);
        }

        [TestMethod]
        [DataRow("12345")]
        [DataRow("6543297811")]
        public void Payer_GoodSSN_ReturnsNoValidationError(string data)
        {
            TaxPayerRequestValidator validator = new();
            TaxPayerRequest request = FakeGoodRequest();
            request.SSN = data;
            var result = validator.Validate(request);

            Assert.IsTrue(result.IsValid); // validator.ShouldNotHaveValidationErrorFor(e => e.SSN, request);
        }

        [TestMethod]
        [DataRow(-0.01)]
        [DataRow(-100)]
        public void Payer_BadGrossIncome_ReturnsValidationError(double data)
        {
            TaxPayerRequestValidator validator = new();
            TaxPayerRequest request = FakeGoodRequest();
            request.GrossIncome = (decimal)data;
            var result = validator.Validate(request);
            Assert.IsFalse(result.IsValid);
        }

        [TestMethod]
        [DataRow(-0.01)]
        [DataRow(-100)]
        public void Payer_BadCharitySpent_ReturnsValidationError(double data)
        {
            TaxPayerRequestValidator validator = new();
            TaxPayerRequest request = FakeGoodRequest();
            request.CharitySpent = (decimal)data;
            var result = validator.Validate(request);
            Assert.IsFalse(result.IsValid);
        }

        [TestMethod]
        [DataRow("data")]
        [DataRow("Sun 10th")]
        [DataRow("11/11")]
        [DataRow("1-1-1")]
        public void Payer_BadCharitySpent_ReturnsValidationError(string data)
        {
            TaxPayerRequestValidator validator = new();
            TaxPayerRequest request = FakeGoodRequest();
            request.DateOfBirth = data;
            var result = validator.Validate(request);

            Assert.IsFalse(result.IsValid); // validator.ShouldHaveValidationErrorFor(e => e.DateOfBirth, request);
        }

        private static TaxPayerRequest FakeGoodRequest()
        {
            return new TaxPayerRequest()
            {
                FullName = "Ivan Petrov",
                SSN = "6543297811",
                GrossIncome = 1000m,
                CharitySpent = 100m,
                DateOfBirth = null
            };
        }
    }
}

