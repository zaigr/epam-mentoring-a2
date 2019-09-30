/*
 * 1.	Write a program, which creates an array of 100 Tasks, runs them and waits all of them are not finished.
 * Each Task should iterate from 1 to 1000 and print into the console the following string:
 * “Task #0 – {iteration number}”.
 */
using System;
using System.Threading.Tasks;

namespace MultiThreading.Task1._100Tasks
{
    class Program
    {
        private const int TaskAmount = 100;
        private const int MaxIterationsCount = 1000;

        private static object _lock = new object();

        static void Main(string[] args)
        {
            var tasks = new Task[TaskAmount];
            for (int taskIndex = 0; taskIndex < TaskAmount; taskIndex++)
            {
                var taskIndexCopy = taskIndex;
                tasks[taskIndex] = Task.Run(() => TaskAction(taskIndexCopy));
            }

            Task.WaitAll(tasks);
        }

        static void TaskAction(int taskNumber)
        {
            for (int iteration = 1; iteration <= MaxIterationsCount; iteration++)
            {
                lock (_lock)
                {
                    Output(taskNumber, iteration);
                }
            }
        }

        static void Output(int taskNumber, int iterationNumber)
        {
            Console.WriteLine($"Task #{taskNumber} – {iterationNumber}");
        }
    }
}
