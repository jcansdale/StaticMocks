namespace StaticMocks.Benchmarks
{
    using System;
    using System.Linq.Expressions;
    using BenchmarkDotNet.Attributes;
    using BenchmarkDotNet.Attributes.Columns;
    using System.Text;

    [MinColumn, MaxColumn]
    public class LambdaExpressionCompileBenchmark
    {
        [Benchmark]
        public object LambdaExpressionCompile_ConstantExpression()
        {
            var constantExpression = (ConstantExpression)GetExpressionBody(() => 666);
            var lambdaExpression = Expression.Lambda(constantExpression);
            var dele = lambdaExpression.Compile();
            return dele.DynamicInvoke();
        }

        [Benchmark]
        public object LambdaExpressionCompile_MemberExpression()
        {
            var expression = (MemberExpression)GetExpressionBody(() => Encoding.ASCII);
            var lambdaExpression = Expression.Lambda(expression);
            var dele = lambdaExpression.Compile();
            return dele.DynamicInvoke();
        }

        static Expression GetExpressionBody<T>(Expression<Func<T>> expression)
        {
            return expression.Body;
        }
    }
}
