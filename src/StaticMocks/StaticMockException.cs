namespace StaticMocks
{
    using System;
    using System.Reflection;

    public class StaticMockException : Exception
    {
        internal StaticMockException(Type targetType, MethodInfo method) :
            base(createMessage(targetType, method))
        {
        }

        static string createMessage(Type targetType, MethodInfo method)
        {
            var classDefinition = StaticMock.Utilities.CreateMockClassDefinition(method);
            return string.Format(
@"Please add the following as a nested class to '{0}':

{1}", targetType.FullName, classDefinition);
        }
    }
}
