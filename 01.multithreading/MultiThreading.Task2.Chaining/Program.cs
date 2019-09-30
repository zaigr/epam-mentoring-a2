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
        private const int MaxInt = 100;
        private const int ArraySize = 10;

        static void Main(string[] args)
        {
            var createArrayTask = Task.Run(() =>
            {
                Console.WriteLine($"Create array of size {ArraySize}.");

                var random = new Random();
                var array = Enumerable.Range(0, ArraySize)
                    .Select(_ => random.Next(MaxInt))
                    .ToArray();

                PrintArrayToConsole(array);

                return array;
            });

            var multiplyTask = createArrayTask
                .ContinueWith((task, state) =>
                    {
                        var array = task.Result;

                        var multiplier = new Random().Next(MaxInt);
                        Console.WriteLine($"Array multiplier: {multiplier}.");

                        for (var i = 0; i < array.Length; i++)
                        {
                            array[i] = array[i] * multiplier;
                        }

                        PrintArrayToConsole(array);

                        return array;
                    },
                    TaskContinuationOptions.OnlyOnRanToCompletion);

            var sortTask = multiplyTask
                .ContinueWith((task, state) =>
                    {
                        Console.WriteLine("Sort array ascending.");

                        var array = task.Result;
                        Array.Sort(array);

                        PrintArrayToConsole(array);

                        return array;
                    },
                    TaskContinuationOptions.OnlyOnRanToCompletion);

            var avgTask = sortTask
                .ContinueWith((task, state) =>
                    {
                        var array = task.Result;

                        var avg = array.Average();
                        Console.WriteLine($"Average is {avg}");
                    },
                    TaskContinuationOptions.OnlyOnRanToCompletion);

            avgTask.Wait();
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
