using Mapper.Configuration;
using Mapper.Tests.Unit.Plain.Classes;

namespace Mapper.Tests.Unit.Plain
{
    public class MappingConfiguration : MapperConfigurationBase
    {
        public MappingConfiguration()
        {
            CreateMap<Source, Dest>();
        }
    }
}
