using Mapper.Configuration;
using Mapper.Tests.Unit.SourceConfiguration.Classes;

namespace Mapper.Tests.Unit.SourceConfiguration
{
    public class MappingConfiguration : MapperConfigurationBase
    {
        public MappingConfiguration()
        {
            CreateMap<Source, Dest>()
                .ForMember(d => d.Name, s => s.FirstName);
            // .ForMember(d => d.Id, s => s.Id.ToString());
        }
    }
}
