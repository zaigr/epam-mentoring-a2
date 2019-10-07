/*
 * 3. Write a program, which multiplies two matrices and uses class Parallel.
 * a. Implement logic of MatricesMultiplierParallel.cs
 *    Make sure that all the tests within MultiThreading.Task3.MatrixMultiplier.Tests.csproj run successfully.
 * b. Create a test inside MultiThreading.Task3.MatrixMultiplier.Tests.csproj to check which multiplier runs faster.
 *    Find out the size which makes parallel multiplication more effective than the regular one.
 */

using System;
using MultiThreading.Task3.MatrixMultiplier.Matrices;
using MultiThreading.Task3.MatrixMultiplier.Multipliers;

namespace MultiThreading.Task3.MatrixMultiplier
{
    class Program
    {
        static void Main(string[] args)
        {
            var matrixSize = 10;

            var firstMatrix = new Matrix(matrixSize, matrixSize * 2);
            var secondMatrix = new Matrix(matrixSize, matrixSize * 2);

            Console.WriteLine("Multiplying...");
            IMatrix resultMatrix = Multiply(firstMatrix, secondMatrix);

            Console.WriteLine("firstMatrix:");
            firstMatrix.Print();

            Console.WriteLine("secondMatrix:");
            secondMatrix.Print();

            Console.WriteLine("resultMatrix:");
            resultMatrix.Print();
        }

        private static IMatrix Multiply(IMatrix left, IMatrix right)
        {
            IMatricesMultiplier multiplier = new MatricesMultiplier();

            return multiplier.Multiply(left, right);
        }
    }
}
