using MultiThreading.Task3.MatrixMultiplier.Matrices;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task3.MatrixMultiplier.Multipliers
{
    public class MatricesMultiplierParallel : MatricesMultiplierBase
    {
        private const int RowPartSize = 100000;

        protected override IMatrix GetProduct(IMatrix left, IMatrix right)
        {
            var resultMatrix = new long[left.RowCount, right.ColCount];

            Parallel.For(
                fromInclusive: 0,
                toExclusive: left.RowCount,
                (leftRowIndex) => MultiplyRowsInParallel(leftRowIndex, left, right, resultMatrix));

            return new Matrix(resultMatrix);
        }

        private void MultiplyRowsInParallel(int leftRowIndex, IMatrix left, IMatrix right, long[,] resultMatrix)
        {
            var row = left.GetRow(leftRowIndex);

            Parallel.For(0, GetRowPartsCount(row), (rowPartIdx) =>
            {
                var rowPart = row.Skip(rowPartIdx * RowPartSize).Take(RowPartSize);
                for (int rightColIdx = 0; rightColIdx < right.ColCount; rightColIdx++)
                {
                    var partOfSum = rowPart
                        .Zip(
                            right.GetColumn(rightColIdx)
                                .Skip(rowPartIdx * RowPartSize),
                            (r, c) => (r * c))
                        .Sum();

                    Interlocked.Add(ref resultMatrix[leftRowIndex, rightColIdx], partOfSum);
                }
            });
        }

        private int GetRowPartsCount(long[] row)
        {
            return (row.Length % RowPartSize == 0)
                ? row.Length / RowPartSize
                : (row.Length / RowPartSize) + 1;
        }
    }
}
