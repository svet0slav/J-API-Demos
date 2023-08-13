using RockPapSci.Dtos.Choices;
using RockPapSci.Dtos.Play;
using System.Threading;

namespace RockPapSci.Service.Common
{
    public interface IGameService
    {
        Task<ChoicesResponse> GetChoices(CancellationToken cancellationToken);

        Task<ChoiceDto?> GetRandomChoice(CancellationToken cancellationToken);

        Task<PlayResponse?> BotPlayOne(ChoiceDto playerChoice, CancellationToken cancellationToken);
    }
}