using AutoMapper;
using TaxCalc.Interfaces.Requests;
using TaxCalc.Interfaces.Responses;
using TaxCalc.Services.Common.Dtos;

namespace TaxCalc.Api.Mappers
{
    public class MyMapperConfiguration
    {
        public IMapper CreateMapperConfiguration() {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.CreateMap<TaxPayerRequest, TaxPayerDto>();

                mc.CreateMap<TaxesDto, TaxesResponse>();
            });

            IMapper mapper = mappingConfig.CreateMapper();

            return mapper;
        }
    }
}
