using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Mapper.Configuration.Mapping
{
    internal static class MappingExpression<TSource, TDest>
    {
        private static readonly IDictionary<string, (Func<TSource, object> Getter, Action<TDest, object> Setter)> 
            DestNameGetterSetterPair = new Dictionary<string, (Func<TSource, object> Getter, Action<TDest, object> Setter)>();

        public static bool IsConfigured { get; private set; } = false;

        public static void Create()
        {
            var sourceProps = typeof(TSource).GetProperties()
                .Where(p => p.CanRead)
                .ToList();

            var destProps = typeof(TDest).GetProperties()
                .Where(p => p.CanWrite)
                .ToList();

            foreach (var sourcePropInfo in sourceProps)
            {
                var destPropInfo = destProps
                    .FirstOrDefault(
                        p => (p.Name == sourcePropInfo.Name) &&
                             (p.PropertyType == sourcePropInfo.PropertyType));

                if (destPropInfo != null)
                {
                    DestNameGetterSetterPair.Add(destPropInfo.Name, (GetPropGetter(sourcePropInfo), GetPropSetter(destPropInfo)));
                }
            }

            IsConfigured = true;
        }

        public static void ReplaceMappingSource(
            MemberExpression destExpr,
            MemberExpression sourceExpr)
        {
            DestNameGetterSetterPair[destExpr.Member.Name] =
                (GetPropGetter((PropertyInfo)sourceExpr.Member), GetPropSetter((PropertyInfo)destExpr.Member));
        }

        public static void RemoveMapping(MemberExpression destExpr)
        {
            if (DestNameGetterSetterPair.Remove(destExpr.Member.Name)) ;
        }

        public static void Apply(TSource source, TDest dest)
        {
            foreach (var getSetPair in DestNameGetterSetterPair.Values)
            {
                var value = getSetPair.Getter(source);
                getSetPair.Setter(dest, value);
            }
        }

        private static Func<TSource, object> GetPropGetter(PropertyInfo propInfo)
        {
            var paramExpression = Expression.Parameter(typeof(TSource), "value");
            var propGetter = Expression.Convert(
                Expression.Property(paramExpression, propInfo.Name),
                typeof(object));

            return Expression.Lambda<Func<TSource, object>>(propGetter, paramExpression).Compile();
        }

        private static Action<TDest, object> GetPropSetter(PropertyInfo propInfo)
        {
            var destParam = Expression.Parameter(typeof(TDest));
            var sourceParam = Expression.Parameter(typeof(object), "value");

            var destGetter = Expression.Property(destParam, propInfo.Name);

            var mapAction = Expression
                .Lambda<Action<TDest, object>>( // TODO: get rid of boxing
                    body: Expression.Assign(destGetter, Expression.Convert(sourceParam, propInfo.PropertyType)),
                    parameters: new []{ destParam, sourceParam })
                .Compile();

            return mapAction;
        }
    }
}
