using TaxCalc.Services.Common.Dtos;

namespace TaxCalc.Services.Common.Interfaces
{
    public interface ICalculatorService
    {
        Task<TaxesDto> Calculate(TaxPayerDto taxPayer, CancellationToken cancellationToken);
    }
}
