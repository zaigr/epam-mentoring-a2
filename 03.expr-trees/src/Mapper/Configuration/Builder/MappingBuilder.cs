using System;
using System.Linq.Expressions;
using Mapper.Configuration.Mapping;

namespace Mapper.Configuration.Builder
{
    internal class MappingBuilder<TSource, TDest> : IMappingBuilder<TSource, TDest>
    {
        public MappingBuilder()
        {
            MappingExpression<TSource, TDest>.Create();
        }

        public void ForMember<TProp>(Expression<Func<TDest, TProp>> destProp, Expression<Func<TDest, TProp>> sourceProp)
        {
            throw new NotImplementedException();
        }

        public void ForMember<TProp>(Expression<Func<TDest, TProp>> destProp, Expression<Action<MappingOptions<TSource, TDest>>> options)
        {
            throw new NotImplementedException();
        }
    }
}
