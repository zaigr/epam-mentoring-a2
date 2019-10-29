using ExprTreeVisitor.LambdaFormatter;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Xunit;
using Xunit.Abstractions;

namespace ExprTreeVisitor.Tests.Unit.LambdaFormatter
{
    public class LambdaParametersFormatterTests
    {
        private readonly ITestOutputHelper _testOutput;

        private readonly ILambdaExpressionFormatters _formatter = new LambdaParametersFormatter();
        public LambdaParametersFormatterTests(ITestOutputHelper testOutput)
        {
            _testOutput = testOutput;
        }

        [Fact]
        public void GivenFormatter_WhenNonExistingParamNameProvided_ThenExceptionRaised()
        {
            // Arrange
            Expression<Func<int, string, int>> expr = (n, s) => n + s.Length;

            var paramConstDict = new Dictionary<string, ConstantExpression>
            {
                ["n"] = Expression.Constant(2),
                ["s"] = Expression.Constant("A"),
                ["b"] = Expression.Constant("A")
            };

            // Act
            var exception = Assert.Throws<ArgumentException>(
                () => _formatter
                    .ReplaceParameters<Func<int, string, int>, Func<int>>(expr, paramConstDict));

            // Assert
            Assert.Equal($"Cannot find parameter of name 'b'.", exception.Message);
        }

        [Fact]
        public void GivenFormatter_WhenInvalidTypeOfConstantProvided_ThenExceptionRaised()
        {
            // Arrange
            Expression<Func<int, string, int>> expr = (n, s) => n + s.Length;

            var paramConstDict = new Dictionary<string, ConstantExpression>
            {
                ["n"] = Expression.Constant(2f),
                ["s"] = Expression.Constant("A")
            };

            // Act
            var exception = Assert.Throws<ArgumentException>(
                () => _formatter
                    .ReplaceParameters<Func<int, string, int>, Func<int>>(expr, paramConstDict));

            // Assert
            Assert.Equal($"Type of constant '2' should be equal to 'n' parameter type.", exception.Message);
        }

        [Fact]
        public void GivenFormatter_WhenAllParametersProvided_ThenAllParametersReplaced()
        {
            // Arrange
            Expression<Func<int, string, int>> expr = (numb, str) => numb + str.Length;
            _testOutput.WriteLine($"Initial: {expr}");

            var paramConstDict = new Dictionary<string, ConstantExpression>
            {
                ["numb"] = Expression.Constant(3),
                ["str"] = Expression.Constant("123"),
            };

            // Act
            var result = _formatter
                .ReplaceParameters<Func<int, string, int>, Func<int>>(expr, paramConstDict);

            // Assert
            _testOutput.WriteLine($"Result: {result}");

            Assert.Equal(6, result.Compile().Invoke());
        }

        [Fact]
        public void GivenFormatter_WhenSomeParametersProvided_ThenAllParametersReplaced()
        {
            // Arrange
            Expression<Func<int, string, string>> expr = (numb, str) => numb + str;
            _testOutput.WriteLine($"Initial: {expr}");

            var paramConstDict = new Dictionary<string, ConstantExpression>
            {
                ["numb"] = Expression.Constant(123),
            };

            // Act
            var result = _formatter
                .ReplaceParameters<Func<int, string, string>, Func<string, string>>(expr, paramConstDict);

            // Assert
            _testOutput.WriteLine($"Result: {result}");

            Assert.Equal("123str", result.Compile().Invoke("str"));
        }

        [Fact]
        public void GivenFormatter_WhenInvalidResultType_ThenExceptionRaised()
        {
            // Arrange
            Expression<Func<int, string, int>> expr = (numb, str) => numb + str.Length;
            _testOutput.WriteLine($"Initial: {expr}");

            var paramConstDict = new Dictionary<string, ConstantExpression>
            {
                ["numb"] = Expression.Constant(3),
                ["str"] = Expression.Constant("123"),
            };

            // Act & Assert
            Assert.Throws<InvalidCastException>(() => _formatter
                .ReplaceParameters<Func<int, string, int>, Func<int, string>>(expr, paramConstDict));
        }

        [Fact]
        public void GivenFormatter_WhenExprContainsIncrementAndDecrement_ThenUnaryOperationsReplaced()
        {
            // Arrange
            var left = Expression.Parameter(typeof(int), name: "a");
            var right = Expression.Parameter(typeof(int), name: "b");

            var expr = Expression.Lambda<Func<int, int, int>>(
                Expression.Add(
                    Expression.Increment(left),
                    Expression.Decrement(right)),
                new List<ParameterExpression> { left, right });
            _testOutput.WriteLine($"Initial: {expr}");

            var paramConstDict = new Dictionary<string, ConstantExpression>
            {
                [left.Name] = Expression.Constant(3),
                [right.Name] = Expression.Constant(5),
            };

            // Act
            var result = _formatter
                .ReplaceParameters<Func<int, int, int>, Func<int>>(expr, paramConstDict);

            // Assert
            _testOutput.WriteLine($"Result: {result}");

            Assert.Equal(8, result.Compile().Invoke());
        }
    }
}
