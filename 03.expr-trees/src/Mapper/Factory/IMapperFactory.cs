﻿namespace Mapper.Factory
{
    public interface IMapperFactory
    {
        IMapper Create<TSource, TDest>();
    }
}
