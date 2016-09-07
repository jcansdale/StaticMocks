namespace StaticMocks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;

    public class StaticMock : IDisposable
    {
        Type targetType;
        IDictionary<MethodInfo, MockDelegate> mockDelegates;

        public StaticMock(Type targetType)
        {
            this.targetType = targetType;
            mockDelegates = new Dictionary<MethodInfo, MockDelegate>();
        }

        public MockTarget Setup(Expression<Action> target)
        {
            var methodCallExpression = (MethodCallExpression)target.Body;
            var method = methodCallExpression.Method;

            MockDelegate mockDelegate;
            if(!mockDelegates.TryGetValue(method, out mockDelegate))
            {
                mockDelegate = new MockDelegate(targetType, method);
                mockDelegates[method] = mockDelegate;
            }

            return new MockTarget(mockDelegate, methodCallExpression);
        }

        public void Dispose()
        {
            foreach(var mockTarget in mockDelegates.Values)
            {
                mockTarget.Dispose();
            }

            mockDelegates = null;
        }

        public class MockDelegate : IDisposable
        {
            MethodInfo method;
            FieldInfo mockField;
            object savedValue;
            IDictionary<object, object> methodReturns = new Dictionary<object, object>();

            internal MockDelegate(Type targetType, MethodInfo method)
            {
                this.method = method;
                mockField = getMockField(targetType, method);

                savedValue = mockField.GetValue(null);
                var func = createDelegateFor(mockField.FieldType);
                mockField.SetValue(null, func);
            }

            static FieldInfo getMockField(Type targetType, MethodInfo method)
            {
                var mockTypeName = method.DeclaringType.Name;
                var mockType = targetType.GetTypeInfo().GetNestedType(mockTypeName, BindingFlags.Public | BindingFlags.NonPublic);
                if(mockType == null)
                {
                    throw new StaticMocksException(targetType, method);
                }

                var mockFieldName = method.Name + method.GetParameters().Length;
                var field = mockType.GetTypeInfo().GetField(mockFieldName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                if (field != null)
                {
                    return field;
                }

                mockFieldName = method.Name;
                field = mockType.GetTypeInfo().GetField(mockFieldName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                if (field != null)
                {
                    return field;
                }

                throw new StaticMocksException(targetType, method);
            }

            public void Dispose()
            {
                mockField.SetValue(null, savedValue);
            }

            Delegate createDelegateFor(Type delegateType)
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
                foreach(var arg in args)
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
                    var value = getValue(arg);
                    argList.Add(value);
                }

                return getKeyObject(methodCallExpression.Method, argList.ToArray());
            }

            static object getValue(Expression expression)
            {
                if (expression is ConstantExpression)
                {
                    var constantExpression = (ConstantExpression)expression;
                    return constantExpression.Value;
                }

                if (expression is MemberExpression)
                {
                    var memberExpression = (MemberExpression)expression;

                    if (memberExpression.Expression is ConstantExpression)
                    {
                        var constantExpression = (ConstantExpression)memberExpression.Expression;

                        var member = memberExpression.Member;
                        if (member is FieldInfo)
                        {
                            var fieldInfo = (FieldInfo)member;
                            return fieldInfo.GetValue(constantExpression.Value);
                        }

                        throw new Exception("Unsupported member type: " + member.GetType());
                    }

                    throw new Exception("Unsupported Expression type of MemberExpression: " +
                        memberExpression.Expression.GetType());
                }

                throw new Exception("Unsupported expression type: " + expression.GetType());
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
            MockDelegate mockDelegate;
            MethodCallExpression methodCallExpression;

            internal MockTarget(MockDelegate mockDelegate, MethodCallExpression methodCallExpression)
            {
                this.mockDelegate = mockDelegate;
                this.methodCallExpression = methodCallExpression;
            }

            public MockTarget Returns(object ret)
            {
                mockDelegate.SetReturns(methodCallExpression, ret);
                return this;
            }

            public MockTarget Returns(object ret, object defaultRet)
            {
                mockDelegate.SetReturns(methodCallExpression, ret);
                mockDelegate.SetDefault(methodCallExpression, defaultRet);
                return this;
            }

            public MockTarget Default(object defaultRet)
            {
                mockDelegate.SetDefault(methodCallExpression, defaultRet);
                return this;
            }
        }
    }
}
