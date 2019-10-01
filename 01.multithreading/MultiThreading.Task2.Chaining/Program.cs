/*
 * 2.	Write a program, which creates a chain of four Tasks.
 * First Task – creates an array of 10 random integer.
 * Second Task – multiplies this array with another random integer.
 * Third Task – sorts this array by ascending.
 * Fourth Task – calculates the average value. All this tasks should print the values to console.
 */
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MultiThreading.Task2.Chaining
{
    class Program
    {
        static void Main(string[] args)
        {
            var chain = Task
                .Run(CreateArray)
                .ContinueWithOnSuccess(MultiplyArray)
                .ContinueWithOnSuccess(SortArray)
                .ContinueWithOnSuccess((array) => array.Average());

            var avgSum = chain.GetAwaiter().GetResult();

            Console.WriteLine($"Average sum is: {avgSum}");
        }

        private static int[] CreateArray()
        {
            var arraySize = 10;
            Console.WriteLine($"Create array of size {arraySize}.");

            var array = RandomizeArray(arraySize, maxInt: 10);

            PrintArrayToConsole(array);

            return array;
        }

        private static int[] MultiplyArray(int[] array)
        {
            var multiplier = new Random().Next(maxValue: 10);
            Console.WriteLine($"Array multiplier: {multiplier}.");

            for (var i = 0; i < array.Length; i++)
            {
                array[i] = array[i] * multiplier;
            }

            PrintArrayToConsole(array);

            return array;
        }

        private static int[] SortArray(int[] array)
        {
            Array.Sort(array);

            PrintArrayToConsole(array);

            return array;
        }

        private static int[] RandomizeArray(int size, int maxInt)
        {
            var random = new Random();
            return Enumerable.Range(0, size)
                .Select(_ => random.Next(maxInt))
                .ToArray();
        }

        private static void PrintArrayToConsole(int[] array)
        {
            foreach (var item in array)
            {
                Console.Write($"{item} ");
            }

            Console.WriteLine();
        }
    }
}
