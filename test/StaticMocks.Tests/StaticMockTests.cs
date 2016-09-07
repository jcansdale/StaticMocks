namespace StaticMocks.Tests
{
    using System;
    using System.IO;
    using NUnit.Framework;
    using System.Text;

    public class StaticMockTests
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
        public void Setup_Twice_ExpectBoth()
        {
            var path1 = "__PATH1__";
            var path2 = "__PATH2__";
            var expect1 = "__EXPECT1__";
            var expect2 = "__EXPECT2__";

            staticMock.Setup(() => System.IO.File.ReadAllText(path1)).Returns(expect1);
            staticMock.Setup(() => System.IO.File.ReadAllText(path2)).Returns(expect2);

            var text1 = File.ReadAllText(path1);
            var text2 = File.ReadAllText(path2);
            Assert.That(text1, Is.EqualTo(expect1));
            Assert.That(text2, Is.EqualTo(expect2));
        }

        [Test]
        public void Setup_Overloads_ExpectBoth()
        {
            var path1 = "__PATH1__";
            var encoding1 = Encoding.ASCII;
            var expect1 = "__EXPECT1__";
            var path2 = "__PATH2__";
            var expect2 = "__EXPECT2__";

            staticMock.Setup(() => System.IO.File.ReadAllText(path1, encoding1)).Returns(expect1);
            staticMock.Setup(() => System.IO.File.ReadAllText(path2)).Returns(expect2);

            var text1 = File.ReadAllText2(path1, encoding1);
            var text2 = File.ReadAllText(path2);
            Assert.That(text1, Is.EqualTo(expect1));
            Assert.That(text2, Is.EqualTo(expect2));
        }

        [Test]
        public void Setup_NoFuncFieldDefined_ExpectException()
        {
            try
            {
                staticMock.Setup(() => System.IO.Directory.CreateDirectory("___"));
            }
            catch (StaticMocksException e)
            {
                var classDefinition =
@"partial class Directory
{
    internal static Func<string, System.IO.DirectoryInfo> CreateDirectory = (string path) => System.IO.Directory.CreateDirectory(path);
}";
                Assert.That(e.Message, Does.Contain(classDefinition));
                return;
            }

            Assert.Fail("Expected StaticMocksException");
        }

        [Test]
        public void Setup_NoActionFieldDefined_ExpectException()
        {
            try
            {
                staticMock.Setup(() => System.Console.WriteLine("Hello, World!"));
            }
            catch (StaticMocksException e)
            {
                var classDefinition =
@"internal static Action<string> WriteLine = (string value) => System.Console.WriteLine(value);";
                Assert.That(e.Message, Does.Contain(classDefinition));
                return;
            }

            Assert.Fail("Expected StaticMocksException");
        }

        [Test]
        public void ReadAllText_MockReturns_ExpectText()
        {
            var path = "__PATH__";
            var expectText = "__EXPECT_TEXT__";
            staticMock.Setup(() => System.IO.File.ReadAllText(path)).Returns(expectText);

            var text = File.ReadAllText(path);

            Assert.That(text, Is.EqualTo(expectText));
        }

        [Test]
        public void Exists_MockReturns_True()
        {
            var path = "__PATH__";
            var expectExists = true;
            staticMock.Setup(() => System.IO.File.Exists(path)).Returns(expectExists);

            var exists = File.Exists(path);

            Assert.That(exists, Is.EqualTo(expectExists));
        }

        [Test]
        public void CurrentDirectory_MockReturns_ExpectDirectory()
        {
            var expectDir = "__DIR__";
            staticMock.Setup(() => System.IO.Directory.GetCurrentDirectory()).Returns(expectDir);

            var dir = Directory.GetCurrentDirectory();

            Assert.That(dir, Is.EqualTo(expectDir));
        }

        [Test]
        public void Copy_MockReturns_DoesntThrow()
        {
            var path1 = "__PATH1__";
            var path2 = "__PATH1__";
            staticMock.Setup(() => System.IO.File.Copy(path1, path2)).Returns(null);

            Assert.DoesNotThrow(() => File.Copy(path1, path2));
        }

        partial class File
        {
            internal static Action<string, string> Copy = (string sourceFileName, string destFileName) => System.IO.File.Copy(sourceFileName, destFileName);
        }

        [Test]
        public void ReadAllText_NullArgMockReturns_ExpectText()
        {
            string path = null;
            var expectText = "__EXPECT_TEXT__";
            staticMock.Setup(() => System.IO.File.ReadAllText(path)).Returns(expectText);

            var text = File.ReadAllText(path);

            Assert.That(text, Is.EqualTo(expectText));
        }

        [Test]
        public void ReadAllText_MockDefault_ExpectDefault()
        {
            var defaultText = "__DEFAULT_TEXT__";
            staticMock.Setup(() => System.IO.File.ReadAllText("__PATH__")).Default(defaultText);

            var text = File.ReadAllText("__OTHER_PATH__");

            Assert.That(text, Is.EqualTo(defaultText));
        }

        [Test]
        public void ReadAllText_MockReturnsNoDefault_ExpectException()
        {
            var path = "__NO_FILE_CALLED_THIS__";
            staticMock.Setup(() => System.IO.File.ReadAllText("__PATH__")).Returns("__TEXT__");

            Assert.Throws<Exception>(() => File.ReadAllText(path));
        }

        [Test]
        public void ReadAllText_NoMock_FileNotFoundException()
        {
            var path = "__NO_FILE_HERE__";

            Assert.Throws<FileNotFoundException>(() => File.ReadAllText(path));
        }

        partial class Directory
        {
            internal static Func<string> GetCurrentDirectory = () => System.IO.Directory.GetCurrentDirectory();
        }

        partial class File
        {
            internal static Func<string, string> ReadAllText = (string path) => System.IO.File.ReadAllText(path);
            internal static Func<string, Encoding, string> ReadAllText2 =
                (string path, Encoding encoding) => System.IO.File.ReadAllText(path, encoding);
            internal static Func<string, bool> Exists = (string path) => System.IO.File.Exists(path);
        }
    }
}
