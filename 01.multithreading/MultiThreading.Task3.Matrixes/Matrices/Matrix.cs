using System;
using System.Buffers;
using System.Text;

namespace MultiThreading.Task3.MatrixMultiplier.Matrices
{
    /// <summary>
    /// The Matrix class. It shouldn't be changed during the task implementation.
    /// </summary>
    public sealed class Matrix : IMatrix
    {
        private const byte RandomMax = 100;

        private const byte MaxPrintElements = 5;

        private readonly long[,] _matrix;

        #region public properties

        public int RowCount { get; }

        public int ColCount { get; }

        #endregion

        public Matrix(long[,] matrix)
        {
            _matrix = matrix;
        }

        public Matrix(int rows, int cols, bool randomInit = false)
        {
            if (rows < 1 || cols < 1)
            {
                throw new ArgumentException($"The matrix should have at least 1 row and 1 column");
            }

            RowCount = rows;
            ColCount = cols;

            _matrix = new long[RowCount, ColCount];
            Initialize();

            void Initialize()
            {
                if (randomInit)
                {
                    var r = new Random();
                    for (var i = 0; i < RowCount; i++)
                    {
                        for (var j = 0; j < ColCount; j++)
                        {
                            _matrix[i, j] = r.Next(RandomMax);
                        }
                    }
                }
            }
        }

        #region public methods

        public void SetElement(int row, int col, long value)
        {
            _matrix[row, col] = value;
        }

        public long GetElement(int row, int col)
        {
            return _matrix[row, col];
        }

        public long[] GetRow(int rowIndex)
        {
            var row = ArrayPool<long>.Shared.Rent(ColCount);
            for (long colIndex = 0; colIndex < ColCount; colIndex++)
            {
                row[colIndex] = _matrix[rowIndex, colIndex];
            }

            return row;
        }

        public long[] GetColumn(int colIndex)
        {
            var column = ArrayPool<long>.Shared.Rent(RowCount);
            for (long rowIndex = 0; rowIndex < ColCount; rowIndex++)
            {
                column[rowIndex] = _matrix[rowIndex, colIndex];
            }

            return column;
        }

        public void Print()
        {
            for (var r = 0; r < RowCount; r++)
            {
                if (r >= MaxPrintElements)
                {
                    Console.WriteLine("...");
                    return;
                }

                var sb = new StringBuilder();
                for (var c = 0; c < ColCount; c++)
                {
                    if (c >= MaxPrintElements)
                    {
                        sb.Append("...".PadRight(7));
                        break;
                    }

                    sb.Append($"{this.GetElement(r, c)}".PadRight(7));
                }

                Console.WriteLine(sb.ToString());
            }
        }

        #endregion
    }
}