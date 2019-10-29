using System;
using Mapper.Configuration.Mapping;

namespace Mapper
{
    internal class Mapper : IMapper
    {
        internal Mapper()
        {
        }

        public TDest Map<TSource, TDest>(TSource source, TDest destination)
        {
            EnsureMappingConfigured<TSource, TDest>();

            MappingExpression<TSource, TDest>.Apply(source, destination);

            return destination;
        }

        public TDest Map<TSource, TDest>(TSource source) where TDest : new()
        {
            EnsureMappingConfigured<TSource, TDest>();

            var destination = new TDest();
            MappingExpression<TSource, TDest>.Apply(source, destination);

            return destination;
        }

        private void EnsureMappingConfigured<TSource, TDest>()
        {
            if (!MappingExpression<TSource, TDest>.IsConfigured)
            {
                throw new InvalidOperationException(
                    $"Mapping between types {typeof(TSource)} and {typeof(TDest)} is not configured.");
            }
        }
    }
}
