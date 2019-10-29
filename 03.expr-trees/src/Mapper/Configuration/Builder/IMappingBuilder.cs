using System;
using System.Linq.Expressions;

namespace Mapper.Configuration.Builder
{
    public interface IMappingBuilder<TSource, TDest>
    {
        IMappingBuilder<TSource, TDest> ForMember<TProp>(Expression<Func<TDest, TProp>> destProp, Expression<Func<TSource, TProp>> sourceProp);

        IMappingBuilder<TSource, TDest> ForMember<TProp>(Expression<Func<TDest, TProp>> destProp, Action<MappingOptions> options);
    }
}
