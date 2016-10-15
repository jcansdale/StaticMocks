// TODO: Install the StaticMocks and NUnit packages.

using System;
using System.IO;
using StaticMocks;
using NSubstitute;
using NUnit.Framework;

public class TargetClassTests
{
    [Test]
    public void MockStaticFunction()
    {
        using (var staticMock = new StaticMock(typeof(ClassUsesFile)))
        {
            staticMock.For(() => File.ReadAllText("foo.txt")).Returns("bar");

            var text = ClassUsesFile.ShoutFile("foo.txt");

            Assert.That(text, Is.EqualTo("BAR"));
        }
    }

    [Test]
    public void ValidateStaticAction()
    {
        using (var staticMock = new StaticMock(typeof(ClassUsesConsole)))
        {
            var text = "Hello, World!";
            staticMock.For(() => Console.WriteLine(Arg.Any<string>()));

            ClassUsesConsole.WriteLine(text);

            staticMock.Received(1, () => Console.WriteLine(text));
        }
    }

    [Test]
    public void MockStaticDefaultThrows()
    {
        using (var staticMock = new StaticMock(typeof(ClassUsesFile)))
        {
            staticMock.For(() => File.ReadAllText(null)).ReturnsForAnyArgs(x =>
            {
                switch(x.ArgAt<string>(0))
                {
                    case "foo.txt":
                        return "bar";
                    default:
                        throw new FileNotFoundException();
                }
            });

            Assert.That(ClassUsesFile.ShoutFile("foo.txt"), Is.EqualTo("BAR"));
            Assert.Throws<FileNotFoundException>(() => ClassUsesFile.ShoutFile("boom.txt"));
        }
    }

}

public class ClassUsesFile
{
    public static string ShoutFile(string path)
    {
        var text = File.ReadAllText(path);
        return text.ToUpperInvariant();
    }

    class File
    {
        internal static Func<string, string> ReadAllText = (string path) => System.IO.File.ReadAllText(path);
    }
}

public class ClassUsesConsole
{
    public static void WriteLine(string text)
    {
        Console.WriteLine(text);
    }

    class Console
    {
        internal static Action<string> WriteLine = (string value) => System.Console.WriteLine(value);
    }
}
