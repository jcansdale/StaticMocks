﻿namespace StaticMocks.Tests.StaticMocks.Tests
{
    using NSubstitute;
    using NUnit.Framework;
    using System;

    public class NSubstituteSupportTests
    {
        StaticMock staticMock;

        [SetUp]
        public void SetUp()
        {
            staticMock = new StaticMock(GetType());
        }

        [TearDown]
        public void TearDown()
        {
            staticMock.Dispose();
        }

        [Test]
        public void For_Action0()
        {
            var action = staticMock.For(() => Tests.StaticClass.ActionMethod());

            StaticClass.ActionMethod0();

            action.Received(1);
            staticMock.For(() => Tests.StaticClass.ActionMethod());
        }

        [Test]
        public void For_Action1()
        {
            var action = staticMock.For(() => Tests.StaticClass.ActionMethod(0));

            StaticClass.ActionMethod1(1);

            action.Received(1);
            staticMock.For(() => Tests.StaticClass.ActionMethod(1));
        }

        [Test]
        public void For_Action2()
        {
            var action = staticMock.For(() => Tests.StaticClass.ActionMethod(0, 0));

            StaticClass.ActionMethod2(1, 2);

            action.Received(1);
            staticMock.For(() => Tests.StaticClass.ActionMethod(1, 2));
        }

        [Test]
        public void For_Action3()
        {
            var action = staticMock.For(() => Tests.StaticClass.ActionMethod(0, 0, 0));

            StaticClass.ActionMethod3(1, 2, 3);

            action.Received(1);
            staticMock.For(() => Tests.StaticClass.ActionMethod(1, 2, 3));
        }

        [Test]
        public void For_Action4()
        {
            var action = staticMock.For(() => Tests.StaticClass.ActionMethod(0, 0, 0, 0));

            StaticClass.ActionMethod4(1, 2, 3, 4);

            action.Received(1);
            staticMock.For(() => Tests.StaticClass.ActionMethod(1, 2, 3, 4));
        }

        [Test]
        public void For_Action5()
        {
            var action = staticMock.For(() => Tests.StaticClass.ActionMethod(0, 0, 0, 0, 0));

            StaticClass.ActionMethod5(1, 2, 3, 4, 5);

            action.Received(1);
            staticMock.For(() => Tests.StaticClass.ActionMethod(1, 2, 3, 4, 5));
        }

        [Test]
        public void For_Action6()
        {
            var action = staticMock.For(() => Tests.StaticClass.ActionMethod(0, 0, 0, 0, 0, 0));

            StaticClass.ActionMethod6(1, 2, 3, 4, 5, 6);

            action.Received(1);
            staticMock.For(() => Tests.StaticClass.ActionMethod(1, 2, 3, 4, 5, 6));
        }

        [Test]
        public void For_Action7()
        {
            var action = staticMock.For(() => Tests.StaticClass.ActionMethod(0, 0, 0, 0, 0, 0, 0));

            StaticClass.ActionMethod7(1, 2, 3, 4, 5, 6, 7);

            action.Received(1);
            staticMock.For(() => Tests.StaticClass.ActionMethod(1, 2, 3, 4, 5, 6, 7));
        }

        [Test]
        public void For_Action8()
        {
            var action = staticMock.For(() => Tests.StaticClass.ActionMethod(0, 0, 0, 0, 0, 0, 0, 0));

            StaticClass.ActionMethod8(1, 2, 3, 4, 5, 6, 7, 8);

            action.Received(1);
            staticMock.For(() => Tests.StaticClass.ActionMethod(1, 2, 3, 4, 5, 6, 7, 8));
        }

        [Test]
        public void For_Func1_ExpectReturns()
        {
            var expectText = "__ExpectText1__";
            staticMock.For(() => Tests.StaticClass.FuncMethod()).Returns(expectText);

            var text = StaticClass.FuncMethod0();
            StaticClass.FuncMethod0();

            Assert.That(text, Is.EqualTo(expectText));
        }

        [Test]
        public void For_Func2_ExpectReturns()
        {
            var expectText = "__ExpectText2__";
            staticMock.For(() => Tests.StaticClass.FuncMethod(1)).Returns(expectText);

            var text = StaticClass.FuncMethod1(1);

            Assert.That(text, Is.EqualTo(expectText));
        }

        [Test]
        public void For_Func3_ExpectReturns()
        {
            var expectText = "__ExpectText3__";
            staticMock.For(() => Tests.StaticClass.FuncMethod(1, 2)).Returns(expectText);

            var text = StaticClass.FuncMethod2(1, 2);

            Assert.That(text, Is.EqualTo(expectText));
        }

        [Test]
        public void For_Func4_ExpectReturns()
        {
            var expectText = "__ExpectText4__";
            staticMock.For(() => Tests.StaticClass.FuncMethod(1, 2, 3)).Returns(expectText);

            var text = StaticClass.FuncMethod3(1, 2, 3);

            Assert.That(text, Is.EqualTo(expectText));
        }

        [Test]
        public void For_Func5_ExpectReturns()
        {
            var expectText = "__ExpectText5__";
            staticMock.For(() => Tests.StaticClass.FuncMethod(1, 2, 3, 4)).Returns(expectText);

            var text = StaticClass.FuncMethod4(1, 2, 3, 4);

            Assert.That(text, Is.EqualTo(expectText));
        }

        [Test]
        public void For_Func6_ExpectReturns()
        {
            var expectText = "__ExpectText6__";
            staticMock.For(() => Tests.StaticClass.FuncMethod(1, 2, 3, 4, 5)).Returns(expectText);

            var text = StaticClass.FuncMethod5(1, 2, 3, 4, 5);

            Assert.That(text, Is.EqualTo(expectText));
        }

        [Test]
        public void For_Func7_ExpectReturns()
        {
            var expectText = "__ExpectText7__";
            staticMock.For(() => Tests.StaticClass.FuncMethod(1, 2, 3, 4, 5, 6)).Returns(expectText);

            var text = StaticClass.FuncMethod6(1, 2, 3, 4, 5, 6);

            Assert.That(text, Is.EqualTo(expectText));
        }

        [Test]
        public void For_Func8_ExpectReturns()
        {
            var expectText = "__ExpectText8__";
            staticMock.For(() => Tests.StaticClass.FuncMethod(1, 2, 3, 4, 5, 6, 7)).Returns(expectText);

            var text = StaticClass.FuncMethod7(1, 2, 3, 4, 5, 6, 7);

            Assert.That(text, Is.EqualTo(expectText));
        }

        [Test]
        public void For_Func9_ExpectReturns()
        {
            var expectText = "__ExpectText9__";
            staticMock.For(() => Tests.StaticClass.FuncMethod(1, 2, 3, 4, 5, 6, 7, 8)).Returns(expectText);

            var text = StaticClass.FuncMethod8(1, 2, 3, 4, 5, 6, 7, 8);

            Assert.That(text, Is.EqualTo(expectText));
        }

        [Test]
        public void ReadAllText_MockReturns_ExpectText()
        {
            var path = "__PATH__";
            var expectText = "__EXPECT_TEXT__";
            staticMock.For(() => System.IO.File.ReadAllText(path)).Returns(expectText);

            var text = File.ReadAllText(path);

            Assert.That(text, Is.EqualTo(expectText));
        }

        [Test]
        public void Setup_Twice_ExpectBoth()
        {
            var path1 = "__PATH1__";
            var path2 = "__PATH2__";
            var expect1 = "__EXPECT1__";
            var expect2 = "__EXPECT2__";

            staticMock.For(() => System.IO.File.ReadAllText(path1)).Returns(expect1);
            staticMock.For(() => System.IO.File.ReadAllText(path2)).Returns(expect2);

            var text1 = File.ReadAllText(path1);
            var text2 = File.ReadAllText(path2);
            Assert.That(text1, Is.EqualTo(expect1));
            Assert.That(text2, Is.EqualTo(expect2));
        }

        [Test]
        public void Setup_Multi_ExpectBoth()
        {
            var path1 = "__PATH1__";
            var expect1 = "__EXPECT1__";
            var expect2 = "__EXPECT2__";

            staticMock.For(() => System.IO.File.ReadAllText(path1)).Returns(expect1, expect2);

            var text1 = File.ReadAllText(path1);
            var text2 = File.ReadAllText(path1);
            Assert.That(text1, Is.EqualTo(expect1));
            Assert.That(text2, Is.EqualTo(expect2));
        }

        class StaticClass
        {
            internal static Action ActionMethod0 = () => Tests.StaticClass.ActionMethod();
            internal static Action<int> ActionMethod1 = (t1) => Tests.StaticClass.ActionMethod(t1);
            internal static Action<int, int> ActionMethod2 = (t1, t2) => Tests.StaticClass.ActionMethod(t1, t2);
            internal static Action<int, int, int> ActionMethod3 = (t1, t2, t3) => Tests.StaticClass.ActionMethod(t1, t2, t3);
            internal static Action<int, int, int, int> ActionMethod4 = (t1, t2, t3, t4) => Tests.StaticClass.ActionMethod(t1, t2, t3, t4);
            internal static Action<int, int, int, int, int> ActionMethod5 = (t1, t2, t3, t4, t5) => Tests.StaticClass.ActionMethod(t1, t2, t3, t4, t5);
            internal static Action<int, int, int, int, int, int> ActionMethod6 = (t1, t2, t3, t4, t5, t6) => Tests.StaticClass.ActionMethod(t1, t2, t3, t4, t5, t6);
            internal static Action<int, int, int, int, int, int, int> ActionMethod7 = (t1, t2, t3, t4, t5, t6, t7) => Tests.StaticClass.ActionMethod(t1, t2, t3, t4, t5, t6, t7);
            internal static Action<int, int, int, int, int, int, int, int> ActionMethod8 = (t1, t2, t3, t4, t5, t6, t7, t8) => Tests.StaticClass.ActionMethod(t1, t2, t3, t4, t5, t6, t7, t8);

            internal static Func<string> FuncMethod0 = () => Tests.StaticClass.FuncMethod();
            internal static Func<int, string> FuncMethod1 = (t1) => Tests.StaticClass.FuncMethod(t1);
            internal static Func<int, int, string> FuncMethod2 = (t1, t2) => Tests.StaticClass.FuncMethod(t1, t2);
            internal static Func<int, int, int, string> FuncMethod3 = (t1, t2, t3) => Tests.StaticClass.FuncMethod(t1, t2, t3);
            internal static Func<int, int, int, int, string> FuncMethod4 = (t1, t2, t3, t4) => Tests.StaticClass.FuncMethod(t1, t2, t3, t4);
            internal static Func<int, int, int, int, int, string> FuncMethod5 = (t1, t2, t3, t4, t5) => Tests.StaticClass.FuncMethod(t1, t2, t3, t4, t5);
            internal static Func<int, int, int, int, int, int, string> FuncMethod6 = (t1, t2, t3, t4, t5, t6) => Tests.StaticClass.FuncMethod(t1, t2, t3, t4, t5, t6);
            internal static Func<int, int, int, int, int, int, int, string> FuncMethod7 = (t1, t2, t3, t4, t5, t6, t7) => Tests.StaticClass.FuncMethod(t1, t2, t3, t4, t5, t6, t7);
            internal static Func<int, int, int, int, int, int, int, int, string> FuncMethod8 = (t1, t2, t3, t4, t5, t6, t7, t8) => Tests.StaticClass.FuncMethod(t1, t2, t3, t4, t5, t6, t7, t8);
        }

        class File
        {
            internal static Func<string, string> ReadAllText = (string path) => System.IO.File.ReadAllText(path);
        }
    }

    static class StaticClass
    {
        internal static void ActionMethod() { }
        internal static void ActionMethod(int t) { }
        internal static void ActionMethod(int t1, int t2) { }
        internal static void ActionMethod(int t1, int t2, int t3) { }
        internal static void ActionMethod(int t1, int t2, int t3, int t4) { }
        internal static void ActionMethod(int t1, int t2, int t3, int t4, int t5) { }
        internal static void ActionMethod(int t1, int t2, int t3, int t4, int t5, int t6) { }
        internal static void ActionMethod(int t1, int t2, int t3, int t4, int t5, int t6, int t7) { }
        internal static void ActionMethod(int t1, int t2, int t3, int t4, int t5, int t6, int t7, int t8) { }
        internal static string FuncMethod() => "FuncMethod";
        internal static string FuncMethod(int t1) => "FuncMethod";
        internal static string FuncMethod(int t1, int t2) => "FuncMethod";
        internal static string FuncMethod(int t1, int t2, int t3) => "FuncMethod";
        internal static string FuncMethod(int t1, int t2, int t3, int t4) => "FuncMethod";
        internal static string FuncMethod(int t1, int t2, int t3, int t4, int t5) => "FuncMethod";
        internal static string FuncMethod(int t1, int t2, int t3, int t4, int t5, int t6) => "FuncMethod";
        internal static string FuncMethod(int t1, int t2, int t3, int t4, int t5, int t6, int t7) => "FuncMethod";
        internal static string FuncMethod(int t1, int t2, int t3, int t4, int t5, int t6, int t7, int t8) => "FuncMethod";
    }
}
