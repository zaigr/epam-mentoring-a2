using MultiThreading.Task3.MatrixMultiplier.Matrices;

namespace MultiThreading.Task3.MatrixMultiplier.Multipliers
{
    public class MatricesMultiplier : MatricesMultiplierBase
    {
        protected override IMatrix GetProduct(IMatrix left, IMatrix right)
        {
            var resultMatrix = new Matrix(left.RowCount, right.ColCount);
            for (var i = 0; i < left.RowCount; i++)
            {
                for (byte j = 0; j < right.ColCount; j++)
                {
                    long sum = 0;
                    for (byte k = 0; k < left.ColCount; k++)
                    {
                        sum += left.GetElement(i, k) * right.GetElement(k, j);
                    }

                    resultMatrix.SetElement(i, j, sum);
                }
            };

            return resultMatrix;
        }
    }
}
