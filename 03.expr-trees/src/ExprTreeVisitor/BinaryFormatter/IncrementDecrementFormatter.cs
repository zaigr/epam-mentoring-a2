using System.Linq.Expressions;

namespace ExprTreeVisitor.BinaryFormatter
{
    public class IncrementDecrementFormatter : ExpressionVisitor, IBinaryExpressionFormatter
    {
        public Expression<TDelegate> Format<TDelegate>(Expression<TDelegate> expression)
        {
            return this.VisitAndConvert(expression, callerName: string.Empty);
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            if ((IsAddType(node) || IsSubtractType(node)) &&
                (IsParameter(node.Left) && IsConstant(node.Right, value: 1)))
            {
                var parameter = (ParameterExpression)node.Left;

                return IsAddType(node)
                    ? MakeIncrement(parameter)
                    : MakeDecrement(parameter);
            }

            return base.VisitBinary(node);
        }

        private bool IsAddType(BinaryExpression node)
            => (node.NodeType == ExpressionType.Add) || (node.NodeType == ExpressionType.AddChecked);

        private bool IsSubtractType(BinaryExpression node)
            => (node.NodeType == ExpressionType.Subtract) || (node.NodeType == ExpressionType.SubtractChecked);

        private bool IsParameter(Expression node)
            => node.NodeType == ExpressionType.Parameter;

        private bool IsConstant(Expression node, int value)
        {
            if (node is ConstantExpression constantExpression)
            {
                return (constantExpression.Type == typeof(int)) &&
                       ((int)constantExpression.Value == value);
            }

            return false;
        }

        private UnaryExpression MakeIncrement(ParameterExpression node)
            => Expression.Increment(node);

        private Expression MakeDecrement(ParameterExpression node)
            => Expression.Decrement(node);
    }
}
