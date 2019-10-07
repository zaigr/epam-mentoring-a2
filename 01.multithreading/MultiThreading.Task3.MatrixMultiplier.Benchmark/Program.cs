using BenchmarkDotNet.Running;

namespace MultiThreading.Task3.MatrixMultiplier.Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<ParallelVsNonParallelBenchmark>();
        }
    }
}
