namespace StaticMocks
{
    using System;
    using System.Reflection;
    using System.Linq.Expressions;
    using System.Collections.Generic;
    using ExpressionUtilities = StaticMock.ExpressionUtilities;

    public static class BasicSupport
    {
        public static MockTarget Setup(this StaticMock staticMock, Expression<Action> target)
        {
            var methodCallExpression = (MethodCallExpression)target.Body;
            var method = methodCallExpression.Method;

            var mockDelegate = staticMock.GetMockDelegate(method);

            MockDelegateTarget mockDelegateTarget;
            if (!mockDelegate.IsMocked)
            {
                mockDelegateTarget = new MockDelegateTarget(method);
                mockDelegate.Delegate = mockDelegateTarget.CreateDelegateFor(mockDelegate.DelegateType);
            }
            else
            {
                mockDelegateTarget = (MockDelegateTarget)mockDelegate.Delegate.Target;
            }

            return new MockTarget(mockDelegateTarget, methodCallExpression);
        }

        internal class MockDelegateTarget
        {
            MethodInfo method;
            IDictionary<object, object> methodReturns = new Dictionary<object, object>();

            internal MockDelegateTarget(MethodInfo method)
            {
                this.method = method;
            }

            public Delegate CreateDelegateFor(Type delegateType)
            {
                string methodName;
                switch (delegateType.Name)
                {
                    case "Action`1":
                        methodName = nameof(action1);
                        break;
                    case "Action`2":
                        methodName = nameof(action2);
                        break;
                    case "Func`1":
                        methodName = nameof(func1);
                        break;
                    case "Func`2":
                        methodName = nameof(func2);
                        break;
                    case "Func`3":
                        methodName = nameof(func3);
                        break;
                    case "Func`4":
                        methodName = nameof(func4);
                        break;
                    default:
                        throw new Exception("Unsupported delegate type: " + delegateType.Name);
                }

                var mockMethodGeneric = GetType().GetTypeInfo().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
                var mockMethod = mockMethodGeneric.MakeGenericMethod(delegateType.GenericTypeArguments);
                return mockMethod.CreateDelegate(delegateType, this);
            }

            R func1<R>()
            {
                return (R)func(new object[] { });
            }

            R func2<T, R>(T t1)
            {
                return (R)func(new object[] { t1 });
            }

            R func3<T1, T2, R>(T1 t1, T2 t2)
            {
                return (R)func(new object[] { t1, t2 });
            }

            R func4<T1, T2, T3, R>(T1 t1, T2 t2, T3 t3)
            {
                return (R)func(new object[] { t1, t2, t3 });
            }

            void action1<T>(T t1)
            {
                func(new object[] { t1 });
            }

            void action2<T1, T2>(T1 t1, T2 t2)
            {
                func(new object[] { t1, t2 });
            }

            object func(object[] args)
            {
                var keyObject = getKeyObject(method, args);
                object value;
                if (methodReturns.TryGetValue(keyObject, out value))
                {
                    return value;
                }

                keyObject = getKeyObject(method);
                if (methodReturns.TryGetValue(keyObject, out value))
                {
                    return value;
                }

                var argText = "";
                foreach (var arg in args)
                {
                    if (argText != "")
                    {
                        argText += ", ";
                    }

                    argText += arg;
                }

                var message = string.Format("No return defined for: {0} ({1})", method, argText);
                throw new Exception(message);
            }

            internal void SetReturns(MethodCallExpression methodCallExpression, object ret)
            {
                var keyObject = getKeyObject(methodCallExpression);
                methodReturns[keyObject] = ret;
            }

            internal void SetDefault(MethodCallExpression methodCallExpression, object defaultRet)
            {
                var keyObject = getKeyObject(methodCallExpression.Method);
                methodReturns[keyObject] = defaultRet;
            }

            static object getKeyObject(MethodCallExpression methodCallExpression)
            {
                var argList = new List<object>();
                foreach (var arg in methodCallExpression.Arguments)
                {
                    var value = ExpressionUtilities.GetValue(arg);
                    argList.Add(value);
                }

                return getKeyObject(methodCallExpression.Method, argList.ToArray());
            }

            static object getKeyObject(MethodInfo method, params object[] args)
            {
                switch (args.Length)
                {
                    case 0:
                        return method;
                    case 1:
                        return Tuple.Create(method, args[0]);
                    case 2:
                        return Tuple.Create(method, args[0], args[1]);
                    case 3:
                        return Tuple.Create(method, args[0], args[1], args[2]);
                    case 4:
                        return Tuple.Create(method, args[0], args[1], args[2], args[3]);
                    default:
                        throw new Exception("Too many args: " + args.Length);
                }
            }
        }

        public class MockTarget
        {
            MockDelegateTarget mockDelegateTarget;
            MethodCallExpression methodCallExpression;

            internal MockTarget(MockDelegateTarget mockDelegateTarget, MethodCallExpression methodCallExpression)
            {
                this.mockDelegateTarget = mockDelegateTarget;
                this.methodCallExpression = methodCallExpression;
            }

            public MockTarget Returns(object ret)
            {
                mockDelegateTarget.SetReturns(methodCallExpression, ret);
                return this;
            }

            public MockTarget Returns(object ret, object defaultRet)
            {
                mockDelegateTarget.SetReturns(methodCallExpression, ret);
                mockDelegateTarget.SetDefault(methodCallExpression, defaultRet);
                return this;
            }

            public MockTarget Default(object defaultRet)
            {
                mockDelegateTarget.SetDefault(methodCallExpression, defaultRet);
                return this;
            }
        }
    }
}
