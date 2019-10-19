using System.Collections.Generic;
using System.Linq.Expressions;

namespace ExprTreeVisitor.LambdaFormatter
{
    public interface ILambdaExpressionFormatters
    {
        Expression<TResult> ReplaceParameters<TDelegate, TResult>(
            Expression<TDelegate> expression,
            IDictionary<string, ConstantExpression> parameterConstantDict);
    }
}
