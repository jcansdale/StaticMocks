namespace StaticMocks
{
    using System;
    using System.Reflection;
    using System.Linq.Expressions;

    public class MockDelegate : IDisposable
    {
        FieldInfo mockField;
        Delegate savedValue;

        internal MockDelegate(Type targetType, MethodInfo method)
        {
            mockField = getMockField(targetType, method);
            savedValue = Delegate;
        }

        public void Dispose()
        {
            Delegate = savedValue;
        }

        public bool IsMocked
        {
            get
            {
                return Delegate != savedValue;
            }
        }

        public Delegate Delegate
        {
            get
            {
                return (Delegate)mockField.GetValue(null);
            }

            set
            {
                mockField.SetValue(null, value);
            }
        }

        public Type DelegateType
        {
            get
            {
                return mockField.FieldType;
            }
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
