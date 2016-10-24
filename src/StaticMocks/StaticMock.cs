namespace StaticMocks
{
    using System;
    using System.Reflection;
    using System.Collections.Generic;

    public partial class StaticMock : IDisposable
    {
        static IDictionary<Type, StaticMock> staticMocks = new Dictionary<Type, StaticMock>();

        Type targetType;
        IDictionary<FieldInfo, MockDelegate> mockDelegates;

        public StaticMock(Type targetType)
        {
            activate(targetType, this);
            this.targetType = targetType;
            mockDelegates = new Dictionary<FieldInfo, MockDelegate>();
        }

        public void Dispose()
        {
            if (mockDelegates == null)
            {
                return;
            }

            foreach (var mockTarget in mockDelegates.Values)
            {
                mockTarget.Dispose();
            }

            mockDelegates = null;
            deactivate(targetType);
        }

        static void activate(Type type, StaticMock staticMock)
        {
            lock (staticMocks)
            {
                if (staticMocks.ContainsKey(type))
                {
                    throw new StaticMockException(StaticMockException.AlreadyActiveMessage(type));
                }

                staticMocks.Add(type, staticMock);
            }
        }

        static void deactivate(Type type)
        {
            lock (staticMocks)
            {
                staticMocks.Remove(type);
            }
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

            LastMockDelegate = mockDelegate;
            return mockDelegate;
        }

        public MockDelegate LastMockDelegate { get; private set; }

        static FieldInfo getMockField(Type targetType, MethodInfo method)
        {
            var mockTypeName = method.DeclaringType.Name;
            var mockType = targetType.GetTypeInfo().GetNestedType(mockTypeName, BindingFlags.Public | BindingFlags.NonPublic);
            if (mockType == null)
            {
                throw new StaticMockException(StaticMockException.SuggestSourceMessage(targetType, method));
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

            throw new StaticMockException(StaticMockException.SuggestSourceMessage(targetType, method));
        }
    }
}
