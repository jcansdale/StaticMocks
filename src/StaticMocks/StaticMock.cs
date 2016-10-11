namespace StaticMocks
{
    using System;
    using System.Reflection;
    using System.Collections.Generic;

    public partial class StaticMock : IDisposable
    {
        Type targetType;
        IDictionary<FieldInfo, MockDelegate> mockDelegates;

        public StaticMock(Type targetType)
        {
            this.targetType = targetType;
            mockDelegates = new Dictionary<FieldInfo, MockDelegate>();
        }

        public void Dispose()
        {
            foreach(var mockTarget in mockDelegates.Values)
            {
                mockTarget.Dispose();
            }

            mockDelegates = null;
        }

        public MockDelegate GetMockDelegate(MethodInfo method)
        {
            var field = getMockField(targetType, method);

            MockDelegate mockDelegate;
            if (!mockDelegates.TryGetValue(field, out mockDelegate))
            {
                mockDelegate = new MockDelegate(field);
                mockDelegates[field] = mockDelegate;
            }

            return mockDelegate;
        }


        static FieldInfo getMockField(Type targetType, MethodInfo method)
        {
            var mockTypeName = method.DeclaringType.Name;
            var mockType = targetType.GetTypeInfo().GetNestedType(mockTypeName, BindingFlags.Public | BindingFlags.NonPublic);
            if (mockType == null)
            {
                throw new StaticMockException(targetType, method);
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

            throw new StaticMockException(targetType, method);
        }
    }
}
