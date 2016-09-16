namespace StaticMocks
{
    using NSubstitute;
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;
    using ExpressionUtilities = StaticMock.ExpressionUtilities;

    public static class NSubstituteSupport
    {
        public static R For<R>(this StaticMock staticMock, Expression<Func<R>> target)
        {
            var methodCallExpression = (MethodCallExpression)target.Body;
            var method = methodCallExpression.Method;

            var mockDelegate = staticMock.GetMockDelegate(method);
            if (!mockDelegate.IsMocked)
            {
                mockDelegate.Delegate = createDelegateFor(mockDelegate.DelegateType);
            }

            var argList = new List<object>();
            foreach (var arg in methodCallExpression.Arguments)
            {
                var value = ExpressionUtilities.GetValue(arg);
                argList.Add(value);
            }

            return (R)mockDelegate.Delegate.DynamicInvoke(argList.ToArray());
        }

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
            var action = mockDelegate.Delegate.Target;
            action.ClearReceivedCalls();
            return action;
        }

        static Delegate createDelegateFor(Type delegateType)
        {
            var dele = quickCreateDelegateFor(delegateType);
            if(dele != null)
            {
                return dele;
            }

            return (Delegate)Substitute.For(new Type[] { delegateType }, new object[0]);
        }

        // TODO: Add support for all Actions and Funcs.
        static Delegate quickCreateDelegateFor(Type delegateType)
        {
            Type type;
            switch (delegateType.Name)
            {
                case "Action":
                    type = typeof(IAction);
                    break;
                case "Action`1":
                    type = typeof(IAction<>).MakeGenericType(delegateType.GenericTypeArguments);
                    break;
                case "Func`1":
                    type = typeof(IFunc<>).MakeGenericType(delegateType.GenericTypeArguments);
                    break;
                case "Func`2":
                    type = typeof(IFunc<,>).MakeGenericType(delegateType.GenericTypeArguments);
                    break;
                case "Func`3":
                    type = typeof(IFunc<,,>).MakeGenericType(delegateType.GenericTypeArguments);
                    break;
                case "Func`4":
                    type = typeof(IFunc<,,,>).MakeGenericType(delegateType.GenericTypeArguments);
                    break;
                case "Func`5":
                    type = typeof(IFunc<,,,,>).MakeGenericType(delegateType.GenericTypeArguments);
                    break;
                case "Func`6":
                    type = typeof(IFunc<,,,,,>).MakeGenericType(delegateType.GenericTypeArguments);
                    break;
                case "Func`7":
                    type = typeof(IFunc<,,,,,,>).MakeGenericType(delegateType.GenericTypeArguments);
                    break;
                case "Func`8":
                    type = typeof(IFunc<,,,,,,,>).MakeGenericType(delegateType.GenericTypeArguments);
                    break;
                case "Func`9":
                    type = typeof(IFunc<,,,,,,,,>).MakeGenericType(delegateType.GenericTypeArguments);
                    break;
                default:
                    throw new Exception("Unsupported delegate type: " + delegateType.Name);
                    //return null;
            }

            var target = Substitute.For(new Type[] { type }, new object[0]);
            var invokeMethod = type.GetTypeInfo().GetMethod("Invoke");
            return invokeMethod.CreateDelegate(delegateType, target);
        }

        public interface IAction { void Invoke(); }
        public interface IAction<T> { void Invoke(T t); }
        public interface IFunc<R> { R Invoke(); }
        public interface IFunc<T, R> { R Invoke(T t); }
        public interface IFunc<T1, T2, R> { R Invoke(T1 t1, T2 t2); }
        public interface IFunc<T1, T2, T3, R> { R Invoke(T1 t1, T2 t2, T3 t3); }
        public interface IFunc<T1, T2, T3, T4, R> { R Invoke(T1 t1, T2 t2, T3 t3, T4 t4); }
        public interface IFunc<T1, T2, T3, T4, T5, R> { R Invoke(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5); }
        public interface IFunc<T1, T2, T3, T4, T5, T6, R> { R Invoke(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6); }
        public interface IFunc<T1, T2, T3, T4, T5, T6, T7, R> { R Invoke(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7); }
        public interface IFunc<T1, T2, T3, T4, T5, T6, T7, T8, R> { R Invoke(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T7 t8); }
    }
}
