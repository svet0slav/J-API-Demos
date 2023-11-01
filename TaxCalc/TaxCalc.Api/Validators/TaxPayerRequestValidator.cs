using FluentValidation;
using FluentValidation.Validators;
using TaxCalc.Interfaces.Requests;

namespace TaxCalc.Api.Validators
{
    public class TaxPayerRequestValidator: AbstractValidator<TaxPayerRequest>
    {
        public TaxPayerRequestValidator() {
            RuleFor(t => t.FullName)
                .NotEmpty().MinimumLength(4)
                .WithMessage("[PropertyName] must contain minimum 4 symbols");

            // used from https://stackoverflow.com/questions/64205825/regex-with-fluent-validation-in-c-sharp-how-to-not-allow-spaces-and-certain-sp
            RuleFor(t => t.FullName)
                .Matches("^[a-zA-Z\\s]+$")
                .WithMessage("[PropertyName] must contain letters or spaces");
            RuleFor(t => t.FullName)
                .Matches("[\\s]+")
                .WithMessage("[PropertyName] must contain at least a space");
            
            RuleFor(t => t.SSN)
                .NotEmpty()
                .Matches("\\d+")
                .MinimumLength(5).MaximumLength(10).Matches(@"\d")
                .WithMessage("SSN must be a valid 5 to 10 digits number unique per taxpayer (mandatory) (e.g. 12345, 6543297811)");

            RuleFor(t => t.GrossIncome)
                .NotEmpty().GreaterThanOrEqualTo(0);

            RuleFor(t => t.CharitySpent)
                .GreaterThanOrEqualTo(0);

            //// From https://stackoverflow.com/questions/16747164/fluentvalidation-check-value-is-a-date-only-if-not-null
            //RuleFor(t => string.IsNullOrEmpty(t.DateOfBirth)
            //            || DateTime.TryParse(t.DateOfBirth, out DateTime x))
            //    .WithMessage("DateOfBirth - a valid date (optional)");
        }
    }
}
