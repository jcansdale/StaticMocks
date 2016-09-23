// TODO: Install the StaticMocks and NUnit packages.

using System;
using System.IO;
using StaticMocks;
using NSubstitute;
using NUnit.Framework;

public class TargetClassTests
{
    [Test]
    public void ShoutFile()
    {
        using (var staticMock = new StaticMock(typeof(TargetClass)))
        {
            staticMock.For(() => File.ReadAllText("foo.txt")).Returns("bar");

            var text = TargetClass.ShoutFile("foo.txt");

            Assert.That(text, Is.EqualTo("BAR"));
        }
    }
}

public class TargetClass
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
