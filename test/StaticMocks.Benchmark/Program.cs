using BenchmarkDotNet.Running;
using System;

namespace StaticMocks.Benchmarks
{
    class Program
    {
        static void Main()
        {
            BenchmarkRunner.Run<ExpressionUtilitiesBenchmark>();
        }

        public static string Read(string path)
        {
            return File.ReadAllText(path);
        }

        public class File
        {
            internal static Func<string, string> ReadAllText = (string path) => System.IO.File.ReadAllText(path);
        }
    }
}
