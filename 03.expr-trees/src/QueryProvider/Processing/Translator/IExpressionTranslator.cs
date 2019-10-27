using System.Linq.Expressions;

namespace QueryProvider.Processing.Translator
{
    public interface IExpressionTranslator
    {
        string Translate(Expression exp);
    }
}
