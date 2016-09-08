﻿namespace StaticMocks
{
    using System;
    using System.Reflection;
    using System.Linq.Expressions;
    using System.Collections.Generic;

    public class StaticMock : IDisposable
    {
        Type targetType;
        IDictionary<MethodInfo, MockDelegate> mockDelegates;

        public StaticMock(Type targetType)
        {
            this.targetType = targetType;
            mockDelegates = new Dictionary<MethodInfo, MockDelegate>();
        }

        public void Dispose()
        {
            foreach(var mockTarget in mockDelegates.Values)
            {
                mockTarget.Dispose();
            }

            mockDelegates = null;
        }

        internal MockDelegate GetMockDelegate(MethodInfo method)
        {
            MockDelegate mockDelegate;
            if (!mockDelegates.TryGetValue(method, out mockDelegate))
            {
                mockDelegate = new MockDelegate(targetType, method);
                mockDelegates[method] = mockDelegate;
            }

            return mockDelegate;
        }
    }
}
