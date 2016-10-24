### A mocking library capable of mocking .NET and .NET Core static methods 

#### Getting started

*For convenience I've put the sample code [here](https://raw.githubusercontent.com/jcansdale/StaticMocks/master/example.cs).*

* Take the following target code.

 ```c#
    using System.IO;

    public class TargetClass
    {
        public static string ShoutFile(string path)
        {
            var text = File.ReadAllText(path);
            return text.ToUpperInvariant();
        }
    }
 ```

* Install the *StaticMocks* package from NuGet (check 'Incude prerelease'). *StaticMocks* works nicely with [NSubstitute 2.0](http://nsubstitute.github.io/) and will automatically pull in that package. It also has a simple Moq-like interface if you need to remove the *NSubstitute* dependency.

* Write the following test code.

```c#
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
```

* Run the `ShoutFile` test. If you're using [TestDriven.Net](http://testdriven.net) you will see the following.

```
------ Test started: Assembly: Target.Tests.dll ------

Test 'Target.Tests.TargetClassTests.ShoutFile' failed: StaticMocks.StaticMockException:

Please add the following as a nested class to 'Target.Tests.TargetClass':

class File
{
    internal static Func<string, string> ReadAllText = (string path) => System.IO.File.ReadAllText(path);
}
	Samples\Sample1.cs(17,0): at Samples.Tests.TargetClassTests.ShoutFile()

0 passed, 1 failed, 0 skipped, took 0.67 seconds (NUnit 3.4.1).
```

* Do as the exception message suggests and change your target code to this.

```c#
    using System;

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
```

* Run the `ShoutFile` test again and rejoice when the test passes!

#### How can you verify that a static method was called?

You can do this using `StaticMock.Received`. For example, the following method:

```c#
[Test]
public void ShoutFile()
{
    using (var staticMock = new StaticMock(typeof(TargetClass)))
    {
        staticMock.For(() => File.ReadAllText("foo.txt")).Returns("bar");

        var text = TargetClass.ShoutFile("foo.txt");

        staticMock.Received(1, () => File.ReadAllText("foo.txt"));
    }
}
```

Will fail with the following:

```
Test 'ShoutFile' failed: NSubstitute.Exceptions.ReceivedCallsException : Expected to receive exactly 1 call matching:
	Invoke("foo.txt")
Actually received no matching calls.
Received 1 non-matching call (non-matching arguments indicated with '*' characters):
	Invoke(*"oops.txt"*)
```

#### How do I specify default values or exceptions?

For methods that return a simple value, you can specify the default `Returns` value before the specific values. For example:

```c#
staticMock.For(() => File.Exists(null)).ReturnsForAnyArgs(false);
staticMock.For(() => File.Exists("foo.txt")).Returns(true);
```

Alternatively, you can call `ReturnsForAll` *after* specifying your `Returns`.

```c#
staticMock.For(() => File.Exists("foo.txt")).Returns(true);
staticMock.ReturnsForAll(false);
```

If you need to throw an exception in the default case, you can call `ThrowsForAll` *after* specifying your `Returns`.

```c#
staticMock.For(() => File.ReadAllText("foo.txt")).Returns("bar");
staticMock.ThrowsForAll(new FileNotFoundException());
```

#### What happens if tests are being run in parallel?

*StaticMocks* enforces a rule that a target type can only be mocked by one `StaticMock` at a time.
If a parallel test attempts to mock a type more than once, the test will fail with a `StaticMockException`.

For example, the tests below will fail with the following message:

```
------ Test started: Assembly: StaticMocks.ParallelTests.dll ------

Test 'ParallelTests.FooTests.Test' failed: StaticMocks.StaticMockException : There is already an active `StaticMock` for type `Reader`.
If you're using xUnit, ensure that test classes that create a `StaticMock` for `Reader` belong to the same collection.
For example, you could add [Collection("ReaderTests")] to these classes.
	StaticMocks.ParallelTests\Class1.cs(15,0): at ParallelTests.FooTests.Test()

1 passed, 1 failed, 0 skipped, took 0.71 seconds (xUnit.net 2.2.0 build 3402).
```

To fix this error, uncomment the `[Collection("ReaderTests")]` attributes.

```c#
namespace ParallelTests
{
    using System;
    using System.IO;
    using Xunit;
    using NSubstitute;
    using StaticMocks;

    // [Collection("ReaderTests")]
    public class FooTests
    {
        [Fact]
        public void Test()
        {
            using (var staticMock = new StaticMock(typeof(Reader)))
            {
                staticMock.For(() => File.ReadAllText("readme.txt")).Returns("foo");
                Assert.Equal("foo", Reader.ReadAllText("readme.txt"));
            }
        }
    }

    // [Collection("ReaderTests")]
    public class BarTests
    {
        [Fact]
        public void Test()
        {
            using (var staticMock = new StaticMock(typeof(Reader)))
            {
                staticMock.For(() => File.ReadAllText("readme.txt")).Returns("bar");
                Assert.Equal("bar", Reader.ReadAllText("readme.txt"));
            }
        }
    }

    public class Reader
    {
        public static string ReadAllText(string path)
        {
            return File.ReadAllText(path);
        }

        class File
        {
            internal static Func<string, string> ReadAllText = (string path) => System.IO.File.ReadAllText(path);
        }
    }
}
```

The `[Collection(...)]` attributes are only necessary when a target class is being mocked by multiple xUnit test fixtures.
By default NUnit doesn't do parallel testing of fixtures, so this shouldn't be an issue unless you explicitly enable it.

#### Conclusion

You've just tested a static method without touching the public interface of
its containing class or running your tests with a special tool. This was made
possible by adding a single auto-generated field/class to the class under test.
You could use `#if DEBUG` to remove this from the released assembly if you're
so inclined.

This is by no means perfect, but it's useful in a lot of common scenarios.
I've been dogfooding it for a little while now.

#### FAQ

Ask away in the issues section or tweet me! [jcansdale@twitter.com](https://twitter.com/jcansdale)
