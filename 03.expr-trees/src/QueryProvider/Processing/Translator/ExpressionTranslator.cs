using System.Linq;
using System.Linq.Expressions;
using System.Text;
using QueryProvider.Processing.Translator.Internal;

namespace QueryProvider.Processing.Translator
{
    public class ExpressionTranslator : ExpressionVisitor, IExpressionTranslator
    {
        readonly StringBuilder _stringBuilder;

        public ExpressionTranslator()
        {
            _stringBuilder = new StringBuilder();
        }

        public string Translate(Expression exp)
        {
            Visit(exp);

            return _stringBuilder.ToString();
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (IsWhere(node))
            {
                var translator = new WhereExpressionTranslator();
                _stringBuilder.Append(translator.Translate(node));

                return node;
            }

            return base.VisitMethodCall(node);
        }

        private bool IsWhere(MethodCallExpression node)
            => (node.Method.DeclaringType == typeof(Queryable)) && (node.Method.Name == "Where");
    }
}
