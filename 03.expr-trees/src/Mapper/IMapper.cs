namespace Mapper
{
    public interface IMapper
    {
        TDest Map<TSource, TDest>(TSource source, TDest destination);

        TDest Map<TSource, TDest>(TSource source)
            where TDest : new();
    }
}
