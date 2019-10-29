using ExprTreeVisitor.BinaryFormatter;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Xunit;
using Xunit.Abstractions;

namespace ExprTreeVisitor.Tests.Unit.BinaryFormatter
{
    public class IncrementDecrementFormatterTests
    {
        private readonly ITestOutputHelper _testOutput;

        private readonly IBinaryExpressionFormatter _formatter = new IncrementDecrementFormatter();

        public IncrementDecrementFormatterTests(ITestOutputHelper testOutput)
        {
            _testOutput = testOutput;
        }

        [Theory]
        [MemberData(nameof(ParameterlessExprTestMemberData))]
        public void GivenFormatter_WhenParameterlessExprProvided_ThenSameExprReturned(Expression<Func<int>> expr)
        {
            // Arrange
            _testOutput.WriteLine($"Initial: {expr}");

            // Act
            var result = _formatter.Format(expr);

            // Assert
            _testOutput.WriteLine($"Result: {result}");

            Assert.Equal(expr.ToString(), result.ToString());
            Assert.Equal(expr.Compile().Invoke(), result.Compile().Invoke());
        }

        public static IEnumerable<object[]> ParameterlessExprTestMemberData()
        {
            return new List<Expression<Func<int>>[]>
            {
                new Expression<Func<int>>[] { () => 1 },
                new Expression<Func<int>>[] { () => 0 + 1 + 1 },
                new Expression<Func<int>>[] { () => 1 - 1 },
                new Expression<Func<int>>[] { () => 1 + 0 },
                new Expression<Func<int>>[] { () => 0 },
            };
        }

        [Theory]
        [MemberData(nameof(OneParameterExprForModificationTestMemberData))]
        public void GivenFormatter_WhenOneParamExprProvided_ThenModifiedTreeReturned(Expression<Func<int, int>> expr)
        {
            // Arrange
            _testOutput.WriteLine($"Initial: {expr}");

            // Act
            var result = _formatter.Format(expr);

            // Assert
            _testOutput.WriteLine($"Result: {result}");

            Assert.NotEqual(expr.ToString(), result.ToString());

            Assert.Equal(expr.Compile().Invoke(1), result.Compile().Invoke(1));
        }

        public static IEnumerable<object[]> OneParameterExprForModificationTestMemberData()
        {
            var list = new List<Expression<Func<int, int>>[]>
            {
                new Expression<Func<int, int>>[] { a => a + 1 },
                new Expression<Func<int, int>>[] { a => 1 + (a + 1) },
                new Expression<Func<int, int>>[] { a => a + 1 + 1 + 1 },
                new Expression<Func<int, int>>[] { a => a + 1 + 1 + 1 },
                new Expression<Func<int, int>>[] { a => a - 1 },
                new Expression<Func<int, int>>[] { a => 1 - (a - 1) },
                new Expression<Func<int, int>>[] { a => a + 1 - 1 },
            };

            var p = Expression.Parameter(typeof(int), name: "a");
            var checkedAdd = Expression.Lambda<Func<int, int>>(
                Expression.AddChecked(
                    p,
                    Expression.Constant(1, typeof(int))),
                new List<ParameterExpression> { p });
            list.Add(new [] { checkedAdd });

            return list;
        }

        [Theory]
        [MemberData(nameof(OneParameterExprNotForModificationTestMemberData))]
        public void GivenFormatter_WhenOneParamExprProvided_ThenNotModifiedTreeReturned(Expression<Func<int, int>> expr)
        {
            // Arrange
            _testOutput.WriteLine($"Initial: {expr}");

            // Act
            var result = _formatter.Format(expr);

            // Assert
            _testOutput.WriteLine($"Result: {result}");

            Assert.Equal(expr.ToString(), result.ToString());
            Assert.Equal(expr.Compile().Invoke(1), result.Compile().Invoke(1));
        }

        public static IEnumerable<object[]> OneParameterExprNotForModificationTestMemberData()
        {
            return new List<Expression<Func<int, int>>[]>
            {
                new Expression<Func<int, int>>[] { a => 1 + a },
                new Expression<Func<int, int>>[] { a => 1 - a },
                new Expression<Func<int, int>>[] { a => 1 - 1 + a },
                new Expression<Func<int, int>>[] { a => a },
                new Expression<Func<int, int>>[] { a => a * 1 },
            };
        }

        [Theory]
        [MemberData(nameof(TwoParameterExprForModificationTestMemberData))]
        public void GivenFormatter_WhenTwoParamExprProvided_ThenModifiedTreeReturned(Expression<Func<int, int, int>> expr)
        {
            // Arrange
            _testOutput.WriteLine($"Initial: {expr}");

            // Act
            var result = _formatter.Format(expr);

            // Assert
            _testOutput.WriteLine($"Result: {result}");

            Assert.NotEqual(expr.ToString(), result.ToString());

            Assert.Equal(expr.Compile().Invoke(1, 2), result.Compile().Invoke(1, 2));
        }

        public static IEnumerable<object[]> TwoParameterExprForModificationTestMemberData()
        {
            return new List<Expression<Func<int, int, int>>[]>
            {
                new Expression<Func<int, int, int>>[] { (a, b) => (a + 1) - (b - 1) },
                new Expression<Func<int, int, int>>[] { (a, b) => (a + 1) + (b + 1) },
                new Expression<Func<int, int, int>>[] { (a, b) => (a + 1) - b },
                new Expression<Func<int, int, int>>[] { (a, b) => b - 1 - b },
            };
        }
    }
}
