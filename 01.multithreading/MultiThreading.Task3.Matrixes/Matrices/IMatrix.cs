namespace MultiThreading.Task3.MatrixMultiplier.Matrices
{
    public interface IMatrix
    {
        #region properties

        int RowCount { get; }

        int ColCount { get; }

        #endregion

        #region methods

        void SetElement(int row, int col, long value);

        long GetElement(int row, int col);

        long[] GetRow(int rowIndex);

        long[] GetColumn(int index);

        void Print();

        #endregion
    }
}