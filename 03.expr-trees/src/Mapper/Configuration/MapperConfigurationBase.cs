using Mapper.Configuration.Builder;

namespace Mapper.Configuration
{
    public abstract class MapperConfigurationBase
    {
        protected IMappingBuilder<TSource, TDest> CreateMap<TSource, TDest>()
        {
            return new MappingBuilder<TSource, TDest>();
        }
    }
}
