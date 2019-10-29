using System;
using System.Linq;
using System.Linq.Expressions;
using QueryProvider.Helpers;
using QueryProvider.Processing;
using QueryProvider.Processing.Translator;
using QueryProvider.Services;

namespace QueryProvider.Provider
{
    public class E3SLinqProvider : IQueryProvider
    {
        private readonly IE3SSearchService _client;

        private readonly IExpressionTranslator _exprTranslator;

        public E3SLinqProvider(IE3SSearchService client, IExpressionTranslator exprTranslator)
        {
            _client = client;
            _exprTranslator = exprTranslator;
        }

        public IQueryable CreateQuery(Expression expression)
        {
            throw new NotImplementedException();
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new E3SQuery<TElement>(this, expression);
        }

        public object Execute(Expression expression)
        {
            throw new NotImplementedException();
        }

        public TResult Execute<TResult>(Expression expression)
        {
            var itemType = TypeHelper.GetElementType(expression.Type);
            var queryString = _exprTranslator.Translate(expression);

            return (TResult)_client.SearchFts(itemType, queryString);
        }
    }
}
