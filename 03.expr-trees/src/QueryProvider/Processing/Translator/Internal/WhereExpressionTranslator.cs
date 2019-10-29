using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace QueryProvider.Processing.Translator.Internal
{
    internal class WhereExpressionTranslator : ExpressionVisitor
    {
        private const string EqualsExpressionTemplate = "{Parameter}:({Value})";
        private const string StartsWithExpressionTemplate = "{Parameter}:({Value}*)";
        private const string EndsWithExpressionTemplate = "{Parameter}:(*{Value})";
        private const string ContainsExpressionTemplate = "{Parameter}:(*{Value}*)";

        private readonly IReadOnlyCollection<Type> _supportedTypes
            = new List<Type> { typeof(string) };

        private readonly IReadOnlyCollection<ExpressionType> _supportedOperations
            = new List<ExpressionType>
            {
                ExpressionType.Equal, ExpressionType.AndAlso,
            };

        private readonly IDictionary<Type, string[]> _supportedMethods
            = new Dictionary<Type, string[]>
            {
                [typeof(string)] = new[] { "Equals", "Contains", "StartsWith", "EndsWith" }
            };

        private readonly IDictionary<Type, IReadOnlyCollection<(string, string)>> _methodTemplates
            = new Dictionary<Type, IReadOnlyCollection<(string, string)>>
            {
                [typeof(string)] = new[]
                {
                    ("Equals", EqualsExpressionTemplate), 
                    ("StartsWith", StartsWithExpressionTemplate),
                    ("EndsWith", EndsWithExpressionTemplate),
                    ("Contains", ContainsExpressionTemplate),
                }
            };

        private readonly StringBuilder _stringBuilder = new StringBuilder();

        private string _currentOperationTemplate;

        public string Translate(MethodCallExpression exp)
        {
            EnsureExpressionValid(exp);
            Visit(exp.Arguments[1]);

            return _stringBuilder.ToString();
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            switch (node.NodeType)
            {
                case ExpressionType.Equal:
                {
                    EnsureEqualsExpressionValid(node);
                    VisitEqualBinaryExpression(node);
                } break;
                case ExpressionType.AndAlso:
                {
                    VisitAndAlsoBinaryExpression(node);
                } break;
                default:
                {
                    throw new NotSupportedException($"Operation {node.NodeType} is not supported");
                }
            };

            return node;
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            _currentOperationTemplate = GetMethodExpressionTemplate(node.Method);

            foreach (var nodeArgument in node.Arguments)
            {
                Visit(nodeArgument);
            }

            Visit(node.Object);

            _stringBuilder.Append(_currentOperationTemplate);

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

        private void VisitEqualBinaryExpression(BinaryExpression node)
        {
            _currentOperationTemplate = EqualsExpressionTemplate;
            Visit(node.Left);
            Visit(node.Right);

            _stringBuilder.Append(_currentOperationTemplate);
        }

        private void VisitAndAlsoBinaryExpression(BinaryExpression node)
        {
            Visit(node.Left);

            _stringBuilder.Append(Environment.NewLine);

            Visit(node.Right);
        }

        private string GetMethodExpressionTemplate(MethodInfo methodInfo)
        {
            var methods = _methodTemplates[methodInfo.DeclaringType];

            return methods.First(m => m.Item1 == methodInfo.Name).Item2;
        }

        private void EnsureExpressionValid(MethodCallExpression exp)
        {
            var quote = exp.Arguments[1];
            if (quote.GetType() != typeof(UnaryExpression))
            {
                throw new NotSupportedException("Unsupported type of expression");
            }

            var body = ((UnaryExpression)quote).Operand;
            var predicate = ((LambdaExpression)body).Body;

            switch (predicate)
            {
                case BinaryExpression binaryExp:
                    EnsureBinaryExpressionValid(binaryExp);
                    break;
                case MethodCallExpression callExp:
                    EnsureCallExpressionValid(callExp);
                    break;
                default:
                    throw new NotSupportedException($"'{predicate.NodeType}' expression is not supported.");
            }
        }

        private void EnsureBinaryExpressionValid(BinaryExpression exp)
        {
            if (!_supportedOperations.Contains(exp.NodeType))
            {
                throw new NotSupportedException($"Not supported expression type '{exp.NodeType}'.");
            }
        }

        private void EnsureCallExpressionValid(MethodCallExpression exp)
        {
            if (exp.Type != typeof(bool))
            {
                throw new ArgumentException("Expression should be predicate.");
            }

            if (exp.Method.IsStatic)
            {
                throw new ArgumentException("Static methods not supported.");
            }

            if (!_supportedTypes.Contains(exp.Method.DeclaringType))
            {
                throw new NotSupportedException($"Not supported parameter type '{exp.Method.DeclaringType}'.");
            }

            if (!_supportedMethods[exp.Method.DeclaringType].Contains(exp.Method.Name))
            {
                throw new NotSupportedException($"Method '{exp.Method.Name}' is not supported for type '{exp.Method.DeclaringType}'");
            }
        }

        private void EnsureEqualsExpressionValid(BinaryExpression exp)
        {
            if (!(exp.Left.NodeType == ExpressionType.MemberAccess && exp.Right.NodeType == ExpressionType.Constant) &&
                !(exp.Left.NodeType == ExpressionType.Constant && exp.Right.NodeType == ExpressionType.MemberAccess))
            {
                throw new NotSupportedException("Predicate should contain parameter and constant expression.");
            }
        }
    }
}
