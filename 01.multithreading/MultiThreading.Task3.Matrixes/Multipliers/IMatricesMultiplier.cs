﻿using MultiThreading.Task3.MatrixMultiplier.Matrices;

namespace MultiThreading.Task3.MatrixMultiplier.Multipliers
{
    public interface IMatricesMultiplier
    {
        IMatrix Multiply(IMatrix left, IMatrix right);
    }
}
