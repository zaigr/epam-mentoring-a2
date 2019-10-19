using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;

namespace ExprTreeVisitor.LambdaFormatter
{
    public class LambdaParametersFormatter : ExpressionVisitor, ILambdaExpressionFormatters
    {
        private IDictionary<string, ConstantExpression> _parameterConstantDict;

        public Expression<TResult> ReplaceParameters<TDelegate, TResult>(
            Expression<TDelegate> expression,
            IDictionary<string, ConstantExpression> parameterConstantDict)
        {
            EnsureParameterNameAndConstantTypeValid(expression, parameterConstantDict);

            _parameterConstantDict = parameterConstantDict;

            return (Expression<TResult>)base.Visit(expression);
        }

        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            var preservedParams = node.Parameters.Where(p => !_parameterConstantDict.ContainsKey(p.Name));

            return Expression.Lambda(Visit(node.Body), preservedParams);
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            if (_parameterConstantDict.ContainsKey(node.Name))
            {
                return _parameterConstantDict[node.Name];
            }

            return base.VisitParameter(node);
        }

        private void EnsureParameterNameAndConstantTypeValid<TDelegate>(
            Expression<TDelegate> expression,
            IDictionary<string, ConstantExpression> parameterConstantDict)
        {
            foreach (var paramConstantPair in parameterConstantDict)
            {
                EnsureParameterExists(expression.Parameters, paramName: paramConstantPair.Key);

                EnsureConstantTypeValid(
                    expression.Parameters,
                    paramName: paramConstantPair.Key,
                    paramConstantPair.Value);
            }
        }

        private void EnsureParameterExists(
            ReadOnlyCollection<ParameterExpression> expressionParameters,
            string paramName)
        {
            if (expressionParameters.All(p => p.Name != paramName))
            {
                throw new ArgumentException($"Cannot find parameter of name '{paramName}'.");
            }
        }

        private void EnsureConstantTypeValid(
            ReadOnlyCollection<ParameterExpression> expressionParameters,
            string paramName,
            ConstantExpression constExpr)
        {
            var parameter = expressionParameters.First(p => p.Name == paramName);
            if (parameter.Type != constExpr.Type)
            {
                throw new ArgumentException($"Type of constant '{constExpr.Value}' should be equal to '{paramName}' parameter type.");
            }
        }
    }
}
