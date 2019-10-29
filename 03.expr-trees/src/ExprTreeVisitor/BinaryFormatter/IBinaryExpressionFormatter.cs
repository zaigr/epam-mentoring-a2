using System.Linq.Expressions;

namespace ExprTreeVisitor.BinaryFormatter
{
    public interface IBinaryExpressionFormatter
    {
        Expression<TDelegate> Format<TDelegate>(Expression<TDelegate> expression);
    }
}
