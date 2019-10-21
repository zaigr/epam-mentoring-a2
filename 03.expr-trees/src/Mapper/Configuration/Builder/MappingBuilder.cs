using System;
using System.Linq.Expressions;
using System.Reflection;
using Mapper.Configuration.Mapping;

namespace Mapper.Configuration.Builder
{
    internal class MappingBuilder<TSource, TDest> : IMappingBuilder<TSource, TDest>
    {
        public MappingBuilder()
        {
            MappingExpression<TSource, TDest>.Create();
        }

        public IMappingBuilder<TSource, TDest> ForMember<TProp>(Expression<Func<TDest, TProp>> destProp, Expression<Func<TSource, TProp>> sourceProp)
        {
            EnsureIsMemberExpression(destProp.Body);
            EnsureIsMemberExpression(sourceProp.Body);

            MappingExpression<TSource, TDest>
                .ReplaceMappingSource(
                    (MemberExpression)destProp.Body,
                    (MemberExpression)sourceProp.Body);

            return this;
        }

        public IMappingBuilder<TSource, TDest> ForMember<TProp>(Expression<Func<TDest, TProp>> destProp, Action<MappingOptions> optionsSetup)
        {
            EnsureIsMemberExpression(destProp.Body);

            var options = new MappingOptions();
            optionsSetup(options);

            if (options.IsPropIgnored)
            {
                MappingExpression<TSource, TDest>.RemoveMapping((MemberExpression)destProp.Body);
            }

            return this;
        }

        private void EnsureIsMemberExpression(Expression expr)
        {
            if (!(expr is MemberExpression memberExpression &&
                  memberExpression.Member is PropertyInfo))
            {
                throw new ArgumentException($"{expr} should be member expression.");
            }
        }
    }
}
