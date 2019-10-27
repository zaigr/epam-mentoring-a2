using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace QueryProvider.Processing.Translator.Internal
{
    internal class WhereExpressionTranslator : ExpressionVisitor
    {
        private const string EqualsExpressionTemplate = "{Parameter}:({Value})";
        private const string StartsWithExpressionTemplate = "{Parameter}:({Value}*)";
        private const string EndsWithExpressionTemplate = "{Parameter}:(*{Value})";

        private readonly StringBuilder _stringBuilder = new StringBuilder();

        private string _currentOperationTemplate;

        public string Translate(MethodCallExpression exp)
        {
            EnsureExpressionValid(exp);
            Visit(exp);

            return _stringBuilder.ToString();
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            switch (node.NodeType)
            {
                case ExpressionType.Equal:
                {
                    _currentOperationTemplate = EqualsExpressionTemplate;
                    Visit(node.Left);
                    Visit(node.Right);

                    _stringBuilder.Append(_currentOperationTemplate);
                } break;
                case ExpressionType.Call:
                {
                    _currentOperationTemplate = StartsWithExpressionTemplate;
                } break;
                default:
                {
                    throw new NotSupportedException($"Operation {node.NodeType} is not supported");
                }
            };

            return node;
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            _currentOperationTemplate = _currentOperationTemplate
                .Replace("{Value}", node.Value.ToString());

            return base.VisitConstant(node);
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            _currentOperationTemplate = _currentOperationTemplate
                .Replace("{Parameter}", node.Member.Name);

            return base.VisitMember(node);
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            return base.VisitMethodCall(node);
        }

        private void EnsureExpressionValid(MethodCallExpression exp)
        {
            var quote = exp.Arguments[1];
            if (quote.GetType() != typeof(UnaryExpression))
            {
                throw new NotSupportedException("Unsupported type of expression");
            }

            var body = ((UnaryExpression)quote).Operand;
            var predicate = ((LambdaExpression) body).Body;

            switch (predicate)
            {
                case BinaryExpression binaryExp:
                    EnsureBinaryExpressionValid(binaryExp);
                    break;
                case MethodCallExpression callExp:
                    EnsureCallExpressionValid(callExp);
                    break;
                default:
                    throw new NotSupportedException("Provided type of expression is not supported.");
            }
        }

        private void EnsureBinaryExpressionValid(BinaryExpression exp)
        {
            if (exp.NodeType != ExpressionType.Equal)
            {
                throw new NotSupportedException("Only equal operation predicate supported.");
            }

            if (!(exp.Left.NodeType == ExpressionType.MemberAccess && exp.Right.NodeType == ExpressionType.Constant) &&
                !(exp.Left.NodeType == ExpressionType.Constant && exp.Right.NodeType == ExpressionType.MemberAccess))
            {
                throw new NotSupportedException("Predicate should contain");
            }
        }

        private void EnsureCallExpressionValid(MethodCallExpression callExp)
        {
            throw new NotImplementedException();
        }
    }
}
