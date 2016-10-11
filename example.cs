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
            staticMock.For(() => Console.WriteLine(""));

            ClassUsesConsole.WriteLine(text);

            staticMock.Received(1, () => Console.WriteLine(text));
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
