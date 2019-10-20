using System;
using System.Collections.Generic;
using System.Linq;
using Mapper.Configuration;

namespace Mapper.Helpers
{
    internal static class AssemblyScan
    {
        public static IEnumerable<Type> GetMapperConfigurationTypes()
        {
            var instances = AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(
                    a => a.GetTypes()
                        .Where(
                            t => t.IsClass &&
                                 typeof(MapperConfigurationBase).IsAssignableFrom(t) &&
                                 HasEmptyConstructor(t)));

            return instances;
        }

        private static bool HasEmptyConstructor(Type type)
            => (type.GetConstructor(Type.EmptyTypes) != null);
    }
}
