using Mapper.Configuration;
using Mapper.Tests.Unit.OptionsConfiguration.Classes;

namespace Mapper.Tests.Unit.OptionsConfiguration
{
    public class MappingConfiguration : MapperConfigurationBase
    {
        public MappingConfiguration()
        {
            CreateMap<Source, Dest>()
                .ForMember(dest => dest.Id, s => s.Identified)
                .ForMember(dest => dest.Name, opt => opt.Ignore());
        }
    }
}
