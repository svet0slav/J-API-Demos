using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxCalc.Domain.Data;
using TaxCalc.Services.Common.Dtos;

namespace TaxCalc.Service.Mappers
{
    internal static class MappersConfiguration
    {
        public static IMapper CreateMappersConfiguration()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.CreateMap<TaxPayerDto, TaxPayer>();

                mc.CreateMap<TaxesData, TaxesDto>();
            });

            IMapper mapper = mappingConfig.CreateMapper();

            return mapper;
        }
    }
}
