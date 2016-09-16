namespace StaticMocks.Benchmarks
{
    using System;
    using System.Linq.Expressions;
    using BenchmarkDotNet.Attributes;
    using BenchmarkDotNet.Attributes.Columns;
    using System.Text;

    [MinColumn, MaxColumn]
    public class ExpressionUtilitiesBenchmark
    {
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
