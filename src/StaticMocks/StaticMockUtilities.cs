namespace StaticMocks
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;

    public static class StaticMockUtilities
    {
        internal static object GetValue(Expression expression)
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
    }
}
