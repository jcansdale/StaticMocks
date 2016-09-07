namespace StaticMocks
{
    using System;
    using System.Reflection;

    public class StaticMocksException : Exception
    {
        public StaticMocksException(Type targetType, MethodInfo method) :
            base(createMessage(targetType, method))
        {
        }

        static string createMessage(Type targetType, MethodInfo method)
        {
            var fieldDefiniton = createMockFieldDefinition(method);
            var className = method.DeclaringType.Name;
            var classDefinition = string.Format(
@"partial class {0}
{{
    {1}
}}", className, fieldDefiniton);
            return @"Plase add the following:
" + classDefinition;
        }

        private static string createMockFieldDefinition(MethodInfo method)
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
            if(method.ReturnType == typeof(void))
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
            if(type == typeof(string))
            {
                return "string";
            }

            return type.FullName;
        }
    }
}
