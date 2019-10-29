using System;
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

        public IMapper Create()
        {
            return new Mapper();
        }
    }
}
