namespace StaticMocks
{
    using System;
    using System.Reflection;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    public partial class StaticMock
    {
        public static class ExpressionUtilities
        {
            public static object GetValue(Expression expression)
            {
                return getValue(expression, true);
            }

            public static object GetValueWithoutCompiling(Expression expression)
            {
                return getValue(expression, false);
            }

            static object getValue(Expression expression, bool allowCompile)
            {
                if(expression == null)
                {
                    return null;
                }

                if (expression is ConstantExpression)
                {
                    var constantExpression = (ConstantExpression)expression;
                    return getValue(constantExpression);
                }

                if (expression is MemberExpression)
                {
                    var memberExpression = (MemberExpression)expression;
                    return getValue(memberExpression, allowCompile);
                }

                if (expression is MethodCallExpression)
                {
                    var methodCallExpression = (MethodCallExpression)expression;
                    return getValue(methodCallExpression, allowCompile);
                }

                if (allowCompile)
                {
                    return GetValueUsingCompile(expression);
                }

                throw new Exception("Couldn't evaluate Expression without compiling: " + expression);
            }

            static object getValue(ConstantExpression constantExpression)
            {
                return constantExpression.Value;
            }

            private static object getValue(MemberExpression memberExpression, bool allowCompile)
            {
                var value = getValue(memberExpression.Expression, allowCompile);

                var member = memberExpression.Member;
                if (member is FieldInfo)
                {
                    var fieldInfo = (FieldInfo)member;
                    return fieldInfo.GetValue(value);
                }

                if (member is PropertyInfo)
                {
                    var propertyInfo = (PropertyInfo)member;

                    try
                    {
                        return propertyInfo.GetValue(value);
                    }
                    catch(TargetInvocationException e)
                    {
                        throw e.InnerException;
                    }
                }

                throw new Exception("Unknown member type: " + member.GetType());
            }

            private static object getValue(MethodCallExpression methodCallExpression, bool allowCompile)
            {
                var paras = getArray(methodCallExpression.Arguments, true);
                var obj = getValue(methodCallExpression.Object, allowCompile);

                try
                {
                    return methodCallExpression.Method.Invoke(obj, paras);
                }
                catch (TargetInvocationException e)
                {
                    throw e.InnerException;
                }
            }

            static object[] getArray(IEnumerable<Expression> expressions, bool allowCompile)
            {
                var list = new List<object>();
                foreach(var expression in expressions)
                {
                    var value = getValue(expression, allowCompile);
                    list.Add(value);
                }

                return list.ToArray();
            }

            public static object GetValueUsingCompile(Expression expression)
            {
                var lambdaExpression = Expression.Lambda(expression);
                var dele = lambdaExpression.Compile();
                return dele.DynamicInvoke();
            }
        }
    }
}
