using System;
using System.Linq.Expressions;

namespace Mapper.Configuration.Builder
{
    public interface IMappingBuilder<TSource, TDest>
    {
        void ForMember<TProp>(Expression<Func<TDest, TProp>> destProp, Expression<Func<TDest, TProp>> sourceProp);

        void ForMember<TProp>(Expression<Func<TDest, TProp>> destProp, Expression<Action<MappingOptions<TSource, TDest>>> options);
    }
}
