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
            MappingExpression<TSource, TDest>.Apply(source, destination);

            return destination;
        }

        public TDest Map<TSource, TDest>(TSource source) where TDest : new()
        {
            var destination = new TDest();
            MappingExpression<TSource, TDest>.Apply(source, destination);

            return destination;
        }
    }
}
