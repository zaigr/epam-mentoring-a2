using BenchmarkDotNet.Attributes;
using MultiThreading.Task3.MatrixMultiplier.Matrices;
using MultiThreading.Task3.MatrixMultiplier.Multipliers;

namespace MultiThreading.Task3.MatrixMultiplier.Benchmark
{
    [RankColumn]
    public class ParallelVsNonParallelBenchmark
    {
        private readonly IMatricesMultiplier _nonParallelMultiplier = new MatricesMultiplier();
        private readonly IMatricesMultiplier _parallelMultiplier = new MatricesMultiplierParallel();

        private IMatrix _multiplicand;
        private IMatrix _multiplier;

        [Params(500, 1000, 2000)]
        public int MatrixSize { get; set; }

        [GlobalSetup]
        public void GlobalSetup()
        {
            _multiplicand = new Matrix(MatrixSize, MatrixSize, randomInit: true);
            _multiplier = new Matrix(MatrixSize, MatrixSize, randomInit: true);
        }

        [Benchmark]
        public IMatrix Parallel() => _parallelMultiplier.Multiply(_multiplicand, _multiplier);

        [Benchmark]
        public IMatrix NonParallel() => _nonParallelMultiplier.Multiply(_multiplicand, _multiplier);
    }
}
