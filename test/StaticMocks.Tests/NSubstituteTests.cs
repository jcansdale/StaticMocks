namespace StaticMocks.Tests.StaticMocks.Tests
{
    using NSubstitute;
    using NUnit.Framework;
    using System;

    public class NSubstituteTests
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

        partial class File
        {
            internal static Func<string, string> ReadAllText = (string path) => System.IO.File.ReadAllText(path);
        }
    }
}
