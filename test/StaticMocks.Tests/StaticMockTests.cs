namespace StaticMocks.Tests.StaticMocks.Tests
{
    using System;
    using System.Reflection;
    using NUnit.Framework;

    public class StaticMockTests
    {
        [Test]
        public void GetMockDelegate_DelegateType_IsFuncInt()
        {
            using (var staticMock = new StaticMock(typeof(TargetNamespace.TargetClass)))
            {
                var method = new Func<int>(StaticNamespace.StaticClass.FuncInt).GetMethodInfo();

                var mockDelegate = staticMock.GetMockDelegate(method);

                Assert.That(mockDelegate.DelegateType, Is.EqualTo(typeof(Func<int>)));
            }
        }

        [Test]
        public void GetMockDelegate_IsMocked_IsFalse()
        {
            using (var staticMock = new StaticMock(typeof(TargetNamespace.TargetClass)))
            {
                var method = new Func<int>(StaticNamespace.StaticClass.FuncInt).GetMethodInfo();

                var mockDelegate = staticMock.GetMockDelegate(method);

                Assert.That(mockDelegate.IsMocked, Is.False);
            }
        }

        [Test]
        public void GetMockDelegate_IsMocked_IsTrueAfterDelegateSet()
        {
            using (var staticMock = new StaticMock(typeof(TargetNamespace.TargetClass)))
            {
                var method = new Func<int>(StaticNamespace.StaticClass.FuncInt).GetMethodInfo();

                var mockDelegate = staticMock.GetMockDelegate(method);
                mockDelegate.Delegate = new Func<int>(() => 1);

                Assert.That(mockDelegate.IsMocked, Is.True);
            }
        }

        [Test]
        public void Dispose_CalledMoreThanOnce_DoesNotThrow()
        {
            using (var staticMock = new StaticMock(typeof(TargetNamespace.TargetClass)))
            {
                Assert.DoesNotThrow(() => staticMock.Dispose());
            }
        }
    }

    namespace StaticNamespace
    {
        class StaticClass
        {
            internal static int FuncInt() => 0;
        }
    }

    namespace TargetNamespace
    {
        class TargetClass
        {
            partial class StaticClass
            {
                internal static Func<int> FuncInt = () => StaticNamespace.StaticClass.FuncInt();
            }
        }
    }
}
