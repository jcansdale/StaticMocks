﻿namespace StaticMocks
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;

    public partial class StaticMock
    {
        public static class Utilities
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

            internal static string CreateMockClassDefinition(MethodInfo method)
            {
                var fieldDefiniton = CreateMockFieldDefinition(method);
                var className = method.DeclaringType.Name;
                var classDefinition = string.Format(
    @"partial class {0}
{{
    {1}
}}", className, fieldDefiniton);
                return classDefinition;
            }

            internal static string CreateMockFieldDefinition(MethodInfo method)
            {
                var delegateName = getDelegateName(method);
                var methodName = method.Name;
                var targetClassName = method.DeclaringType.FullName;

                var paramsText = "";
                foreach (var param in method.GetParameters())
                {
                    if (paramsText != "")
                    {
                        paramsText += ", ";
                    }

                    paramsText += getShortName(param.ParameterType) + " " + param.Name;
                }

                var argsText = "";
                foreach (var param in method.GetParameters())
                {
                    if (argsText != "")
                    {
                        argsText += ", ";
                    }

                    argsText += param.Name;
                }

                var fieldDefinition = string.Format("internal static {0} {1} = ({2}) => {3}.{1}({4});",
                    delegateName, methodName, paramsText, targetClassName, argsText);
                return fieldDefinition;
            }

            static string getDelegateName(MethodInfo method)
            {
                var typesText = "";
                foreach (var param in method.GetParameters())
                {
                    if (typesText != "")
                    {
                        typesText += ", ";
                    }

                    typesText += getShortName(param.ParameterType);
                }

                string delegateName;
                if (method.ReturnType == typeof(void))
                {
                    delegateName = "Action";
                }
                else
                {
                    delegateName = "Func";
                    if (typesText != "")
                    {
                        typesText += ", ";
                    }

                    typesText += getShortName(method.ReturnType);
                }

                return string.Format("{0}<{1}>", delegateName, typesText);
            }

            //static string getShortName(Type type)
            //{
            //    var compiler = new CSharpCodeProvider();
            //    var typeRef = new CodeTypeReference(type);
            //    return compiler.GetTypeOutput(typeRef);
            //}

            static string getShortName(Type type)
            {
                if (type == typeof(string))
                {
                    return "string";
                }

                return type.FullName;
            }
        }
    }
}
