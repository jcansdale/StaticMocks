namespace StaticMocks
{
    using System;
    using System.Reflection;

    public class MockDelegate : IDisposable
    {
        FieldInfo mockField;
        Delegate savedValue;

        internal MockDelegate(FieldInfo field)
        {
            mockField = field;
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
    }
}
