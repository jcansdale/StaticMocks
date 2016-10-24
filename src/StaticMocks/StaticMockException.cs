namespace StaticMocks
{
    using System;
    using System.Reflection;

    public class StaticMockException : Exception
    {
        public StaticMockException(string message) :
            base(message)
        {
        }

        public static string AlreadyActiveMessage(Type targetType)
        {
            return string.Format(@"There is already an active `StaticMock` for type `{0}`.
If you're using xUnit, ensure that test classes that create a `StaticMock` for `{0}` belong to the same collection.
For example, you could add [Collection(""{0}Tests"")] to these classes.", targetType.Name);
        }

        public static string SuggestSourceMessage(Type targetType, MethodInfo method)
        {
            var classDefinition = StaticMock.Utilities.CreateMockClassDefinition(method);
            return string.Format(
@"

Please add the following as a nested class to '{0}':

{1}", targetType.FullName, classDefinition);
        }

        public static string ReturnsForAllMessage()
        {
            return "StaticMock.For must be called before ReturnsForAll.";
        }
    }
}
