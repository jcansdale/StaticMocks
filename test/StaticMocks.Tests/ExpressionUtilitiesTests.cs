namespace StaticMocks.Tests
{
    using NUnit.Framework;
    using System;
    using System.Text;
    using System.Linq.Expressions;
    using ExpressionUtilities = StaticMock.ExpressionUtilities;

    public class ExpressionUtilitiesTests
    {
        [Test]
        public void GetValue_ConstantExpression_ReturnValue()
        {
            var expression = ExpressionBody(() => "__String__");
            var expectedValue = ExpressionUtilities.GetValueUsingCompile(expression);

            var value = ExpressionUtilities.GetValueWithoutCompiling(expression);

            Assert.That(value, Is.EqualTo(expectedValue));
        }

        [Test]
        public void GetValue_Null_ReturnNull()
        {
            var expression = ExpressionBody(() => (string)null);
            var expectedValue = ExpressionUtilities.GetValueUsingCompile(expression);

            var value = ExpressionUtilities.GetValueWithoutCompiling(expression);

            Assert.That(value, Is.EqualTo(expectedValue));
        }

        [Test]
        public void GetValue_MemberExpression_ReturnProperty()
        {
            var expression = ExpressionBody(() => Encoding.ASCII);
            var expectedValue = ExpressionUtilities.GetValueUsingCompile(expression);

            var value = ExpressionUtilities.GetValueWithoutCompiling(expression);

            Assert.That(value, Is.EqualTo(expectedValue));
        }

        [Test]
        public void GetValue_PropertyExpression_ReturnValue()
        {
            var expression = ExpressionBody(() => InstanceProp);
            var expectedValue = ExpressionUtilities.GetValueUsingCompile(expression);

            var value = ExpressionUtilities.GetValueWithoutCompiling(expression);

            Assert.That(value, Is.EqualTo(expectedValue));
        }

        string InstanceProp { get; } = "__InstanceProp__";

        [Test]
        public void GetValue_StaticProperty_ReturnValue()
        {
            var expression = ExpressionBody(() => StaticProperty);
            var expectedValue = ExpressionUtilities.GetValueUsingCompile(expression);

            var value = ExpressionUtilities.GetValueWithoutCompiling(expression);

            Assert.That(value, Is.EqualTo(expectedValue));
        }

        static string StaticProperty { get; } = "__StaticProperty__";

        [Test]
        public void GetValue_MethodCallWithArguments_ReturnValue()
        {
            var expression = ExpressionBody(() => string.Format("{0} {1} {2}", "foo", "bar", 1));
            var expectedValue = ExpressionUtilities.GetValueUsingCompile(expression);

            var value = ExpressionUtilities.GetValueWithoutCompiling(expression);

            Assert.That(value, Is.EqualTo(expectedValue));
        }

        [Test]
        public void GetValue_MethodExpression_ReturnValue()
        {
            var expression = ExpressionBody(() => 666.ToString());
            var expectedValue = ExpressionUtilities.GetValueUsingCompile(expression);

            var value = ExpressionUtilities.GetValueWithoutCompiling(expression);

            Assert.That(value, Is.EqualTo(expectedValue));
        }

        [Test]
        public void GetValue_RecursiveMethodCalls_ReturnValue()
        {
            var expression = ExpressionBody(() => 666.ToString().ToString());
            var expectedValue = ExpressionUtilities.GetValueUsingCompile(expression);

            var value = ExpressionUtilities.GetValueWithoutCompiling(expression);

            Assert.That(value, Is.EqualTo(expectedValue));
        }

        [Test]
        public void GetValue_Local_ReturnLocal()
        {
            var local = "__Local__";
            var expression = ExpressionBody(() => local);
            var expectedValue = ExpressionUtilities.GetValueUsingCompile(expression);

            var value = ExpressionUtilities.GetValueWithoutCompiling(expression);

            Assert.That(value, Is.EqualTo(expectedValue));
        }

        [Test]
        public void GetValue_Add_GetValueWithoutCompilingThrowsException()
        {
            var x = 42;
            var expression = ExpressionBody(() => x + 666);
            var expectedValue = ExpressionUtilities.GetValueUsingCompile(expression);

            var value = ExpressionUtilities.GetValue(expression);

            Assert.That(value, Is.EqualTo(expectedValue));
            Assert.Throws<Exception>(
                () => ExpressionUtilities.GetValueWithoutCompiling(expression));
        }

        [Test]
        public void GetValue_MethodThrowsException_ThrowsException()
        {
            var expression = ExpressionBody(() => methodThrowsException());

            Assert.Throws<Exception>(
                () => ExpressionUtilities.GetValue(expression));
        }

        static int methodThrowsException()
        {
            throw new Exception("Boom!");
        }

        [Test]
        public void GetValue_PropertyThrowsException_ThrowsException()
        {
            var expression = ExpressionBody(() => PropertyThrowsException);

            Assert.Throws<Exception>(
                () => ExpressionUtilities.GetValue(expression));
        }

        static int PropertyThrowsException
        {
            get
            {
                throw new Exception("Boom!");
            }
        }

        static Expression ExpressionBody<T>(Expression<Func<T>> expression)
        {
            return expression.Body;
        }
    }
}