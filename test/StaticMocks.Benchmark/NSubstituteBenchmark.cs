namespace StaticMocks.Benchmarks
{
    using System;
    using NSubstitute;
    using BenchmarkDotNet.Attributes;
    using BenchmarkDotNet.Attributes.Columns;

    [MinColumn, MaxColumn]
    public class NSubstituteBenchmark
    {
        [Benchmark(Baseline = true)]
        public object SubstituteForInterface() => Substitute.For<IServiceProvider>();

        [Benchmark]
        public object SubstituteForClass() => Substitute.For<Type>();

        [Benchmark]
        public object SubstituteForDelegate() => Substitute.For<Action>();

        [Benchmark]
        public object SubstituteForGenericClass() => Substitute.For<System.Collections.Generic.IEnumerable<string>>();
    }
}
