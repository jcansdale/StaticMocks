namespace StaticMocks.Benchmarks
{
    using System;
    using BenchmarkDotNet.Attributes;
    using BenchmarkDotNet.Attributes.Columns;
    using NSubstitute;
    using System.Linq.Expressions;

    [MinColumn, MaxColumn]
    public class StaticMockBenchmark
    {
        [Benchmark(Baseline = true)]
        public object StaticMock_Setup()
        {
            using (var staticMock = new StaticMock(typeof(Program)))
            {
                staticMock.Setup(() => System.IO.File.ReadAllText("foo")).Returns("bar");

                return Program.Read("foo");
            }
        }

        [Benchmark]
        public object StaticMock_For()
        {
            using (var staticMock = new StaticMock(typeof(Program)))
            {
                staticMock.For(() => System.IO.File.ReadAllText("foo")).Returns("bar");

                return Program.Read("foo");
            }
        }

        [Benchmark]
        public object StaticMock_For_NoReturns()
        {
            using (var staticMock = new StaticMock(typeof(Program)))
            {
                staticMock.For(() => System.IO.File.ReadAllText("foo"));

                return Program.Read("foo");
            }
        }

        Expression expression = GetExpressionBody(() => string.Format("{0} {1}", "foo", "bar"));

        [Benchmark]
        public object GetValueUsingCompile()
        {
            return StaticMock.ExpressionUtilities.GetValueUsingCompile(expression);
        }

        [Benchmark]
        public object GetValueWithoutCompiling()
        {
            return StaticMock.ExpressionUtilities.GetValueWithoutCompiling(expression);
        }

        static Expression GetExpressionBody<T>(Expression<Func<T>> expression)
        {
            return expression.Body;
        }
    }
}
