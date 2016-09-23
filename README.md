### A mocking library capable of mocking .NET and .NET Core static methods 

#### Getting started

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

* Install the *StaticMocks* package from NuGet (check 'Incude prerelease'). *StaticMocks* works nicely with [NSubstitute 2.0](http://nsubstitute.github.io/) and will automatically pull in that package. It also has a simple Moq-like interface if you need to remove the *NSubstitute* dependancy.

* Write the following test code.

```c#
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
