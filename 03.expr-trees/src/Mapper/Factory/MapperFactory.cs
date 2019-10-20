using System;
using Mapper.Configuration.Mapping;
using Mapper.Helpers;

namespace Mapper.Factory
{
    public class MapperFactory : IMapperFactory
    {
        static MapperFactory()
        {
            var mapperConfigTypes = AssemblyScan.GetMapperConfigurationTypes();
            foreach (var mapperConfigType in mapperConfigTypes)
            {
                Activator.CreateInstance(mapperConfigType);
            }
        }

        public IMapper Create<TSource, TDest>()
        {
            if (!MappingExpression<TSource, TDest>.IsConfigured)
            {
                throw new InvalidOperationException(
                    $"Mapping between types {typeof(TSource)} and {typeof(TDest)} is not configured.");
            }

            return new Mapper();
        }
    }
}
