namespace StaticMocks
{
    using NSubstitute;
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using ExpressionUtilities = StaticMock.ExpressionUtilities;

    public static class NSubstituteSupport
    {
        public static object For(this StaticMock staticMock, Expression<Action> target)
        {
            var methodCallExpression = (MethodCallExpression)target.Body;
            var method = methodCallExpression.Method;

            var mockDelegate = staticMock.GetMockDelegate(method);
            if(!mockDelegate.IsMocked)
            {
                mockDelegate.Delegate = createDelegateFor(mockDelegate.DelegateType);
            }

            var argList = new List<object>();
            foreach (var arg in methodCallExpression.Arguments)
            {
                var value = ExpressionUtilities.GetValue(arg);
                argList.Add(value);
            }

            mockDelegate.Delegate.DynamicInvoke(argList.ToArray());
            return null;
        }

        static Delegate createDelegateFor(Type delegateType)
        {
            return (Delegate)Substitute.For(new Type[] { delegateType }, new object[0]);
        }
    }
}
