using MultiThreading.Task3.MatrixMultiplier.Matrices;
using System;

namespace MultiThreading.Task3.MatrixMultiplier.Multipliers
{
    public abstract class MatricesMultiplierBase : IMatricesMultiplier
    {
        public IMatrix Multiply(IMatrix left, IMatrix right)
        {
            if (left.ColCount != right.RowCount)
            {
                throw new InvalidOperationException(
                    $"Cannot multiply matrix with {left.ColCount} columns on matrix with {right.RowCount} rows.");
            }

            return GetProduct(left, right);
        }

        protected abstract IMatrix GetProduct(IMatrix left, IMatrix right);
    }
}
