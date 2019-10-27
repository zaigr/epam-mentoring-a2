using System;
using System.Linq;
using System.Linq.Expressions;
using Expressions.Task3.E3SQueryProvider.Models.Entitites;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QueryProvider.Processing.Translator;

namespace QueryProvider.Tests.Unit.Translator
{
    [TestClass]
    public class WhereExpressionTranslatorTests
    {
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
    }
}
