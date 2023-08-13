using RockPapSci.Data;
using RockPapSci.Dtos.Choices;

namespace RockPapSci.Service.Mappers
{
    public static class ChoiceMappersExtensions
    {
        public static ChoiceDto ToDto(this ChoiceItem item)
        {
            if (item == null) return null;
            return new ChoiceDto()
            {
                Id = item.Id,
                Name = item.Name,
            };
        }
    }
}
