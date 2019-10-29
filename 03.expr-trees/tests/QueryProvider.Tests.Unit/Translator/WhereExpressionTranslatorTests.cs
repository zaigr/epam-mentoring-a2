using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Expressions.Task3.E3SQueryProvider.Models.Entitites;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QueryProvider.Processing.Translator;

namespace QueryProvider.Tests.Unit.Translator
{
    [TestClass]
    public class WhereExpressionTranslatorTests
    {
        [TestMethod]
        public void GivenQuery_WhenSelectProvided_ThenNotSupportedExceptionRaised()
        {
            // Arrange
            var translator = new ExpressionTranslator();
            Expression<Func<IQueryable<EmployeeEntity>, IQueryable<string>>> expression
                = query => query.Select(e => e.Workstation);

            Assert.ThrowsException<NotSupportedException>(
                () => translator.Translate(expression),
                "Method 'Select' is not supported.");
        }

        [TestMethod]
        public void GivenQuery_WhenNotUnaryExpressionProvided_ThenNotSupportedExceptionRaised()
        {
            // Arrange
            var translator = new ExpressionTranslator();
            Expression<Func<IQueryable<EmployeeEntity>, IQueryable<EmployeeEntity>>> expression
                = query => query.Where(e => false);

            Assert.ThrowsException<NotSupportedException>(
                () => translator.Translate(expression),
                $"'Constant' expression is not supported.");
        }

        [TestMethod]
        public void TestBinaryEquals()
        {
            // Arrange
            var translator = new ExpressionTranslator();
            Expression<Func<IQueryable<EmployeeEntity>, IQueryable<EmployeeEntity>>> expression
                = query => query.Where(e => e.Workstation == "EPRUIZHW006");

            // Act
            var result = translator.Translate(expression);

            // Assert
            Assert.AreEqual("Workstation:(EPRUIZHW006)", result);
        }

        [TestMethod]
        public void TestBinaryEqualsInverse()
        {
            // Arrange
            var translator = new ExpressionTranslator();
            Expression<Func<IQueryable<EmployeeEntity>, IQueryable<EmployeeEntity>>> expression
                = query => query.Where(e => "EPRUIZHW006" == e.Workstation);

            // Act
            var result = translator.Translate(expression);

            // Assert
            Assert.AreEqual("Workstation:(EPRUIZHW006)", result);
        }

        [TestMethod]
        public void GivenQuery_WhenTwoPropertiesComparing_ThenNotSupportedExceptionRaised()
        {
            // Arrange
            var translator = new ExpressionTranslator();
            Expression<Func<IQueryable<EmployeeEntity>, IQueryable<EmployeeEntity>>> expression
                = query => query.Where(e => e.Workstation == e.Manager);

            Assert.ThrowsException<NotSupportedException>(
                () => translator.Translate(expression),
                "Predicate should contain parameter and constant expression.");
        }

        [TestMethod]
        public void GivenQuery_WhenIntTypeProvided_ThenNotSupportedExceptionRaised()
        {
            // Arrange
            var translator = new ExpressionTranslator();
            Expression<Func<IQueryable<EmployeeEntity>, IQueryable<EmployeeEntity>>> expression
                = query => query.Where(e => e.NonBillable < 100);

            Assert.ThrowsException<NotSupportedException>(
                () => translator.Translate(expression),
                "Not supported expression type 'LessThan'.");
        }

        [TestMethod]
        public void TestMethodEquals()
        {
            // Arrange
            var translator = new ExpressionTranslator();
            Expression<Func<IQueryable<EmployeeEntity>, IQueryable<EmployeeEntity>>> expression
                = query => query.Where(e => e.Workstation.Equals("EPRUIZHW006"));

            // Act
            var result = translator.Translate(expression);

            // Assert
            Assert.AreEqual("Workstation:(EPRUIZHW006)", result);
        }

        [TestMethod]
        public void TestStartsWith()
        {
            var translator = new ExpressionTranslator();
            Expression<Func<IQueryable<EmployeeEntity>, IQueryable<EmployeeEntity>>> expression
                = query => query.Where(e => e.Workstation.StartsWith("EPRUIZHW006"));

            string translated = translator.Translate(expression);
            Assert.AreEqual("Workstation:(EPRUIZHW006*)", translated);
        }

        [TestMethod]
        public void TestEndsWith()
        {
            var translator = new ExpressionTranslator();
            Expression<Func<IQueryable<EmployeeEntity>, IQueryable<EmployeeEntity>>> expression
                = query => query.Where(e => e.Workstation.EndsWith("IZHW0060"));

            string translated = translator.Translate(expression);
            Assert.AreEqual("Workstation:(*IZHW0060)", translated);
        }

        [TestMethod]
        public void TestContains()
        {
            var translator = new ExpressionTranslator();
            Expression<Func<IQueryable<EmployeeEntity>, IQueryable<EmployeeEntity>>> expression
                = query => query.Where(e => e.Workstation.Contains("IZHW006"));

            string translated = translator.Translate(expression);
            Assert.AreEqual("Workstation:(*IZHW006*)", translated);
        }

        [TestMethod]
        public void GivenQuery_WhenIntMethodCalled_ThenNotSupportedExceptionRaised()
        {
            // Arrange
            var translator = new ExpressionTranslator();
            Expression<Func<IQueryable<EmployeeEntity>, IQueryable<EmployeeEntity>>> expression
                = query => query.Where(e => e.NonBillable.Equals(100));

            Assert.ThrowsException<NotSupportedException>(
                () => translator.Translate(expression),
                $"Not supported parameter type '{typeof(double)}'.");
        }

        [TestMethod]
        public void GivenStringQuery_WhenNotSupportedMethodCalled_ThenNotSupportedExceptionRaised()
        {
            // Arrange
            var translator = new ExpressionTranslator();
            Expression<Func<IQueryable<EmployeeEntity>, IQueryable<EmployeeEntity>>> expression
                = query => query.Where(e => e.NativeName.IsNormalized(NormalizationForm.FormC));

            Assert.ThrowsException<NotSupportedException>(
                () => translator.Translate(expression),
                $"Method 'IsNormalized' is not supported for type '{typeof(string)}'");
        }
    }
}
