using RockPapSci.Dtos.Choices;

namespace RockPapSci.Service.Common
{
    public interface IGameService
    {
        Task<ChoicesResponse> GetChoices(CancellationToken cancellationToken);

        Task<ChoiceDto?> GetRandomChoice(CancellationToken cancellationToken);
    }
}