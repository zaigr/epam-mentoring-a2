/*
 * 1.	Write a program, which creates an array of 100 Tasks, runs them and waits all of them are not finished.
 * Each Task should iterate from 1 to 1000 and print into the console the following string:
 * “Task #0 – {iteration number}”.
 */
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task1._100Tasks
{
    class Program
    {
        private static object _lock = new object();

        static void Main(string[] args)
        {
            var taskIndex = 0;
            var tasks = Task.Factory.StartNewTasks(
                () =>
                {
                    Interlocked.Increment(ref taskIndex);
                    TaskAction(taskIndex);
                },
                numberOfTasks: 100);

            Task.WaitAll(tasks);
        }

        static void TaskAction(int taskNumber)
        {
            for (int iteration = 1; iteration <= 1000; iteration++)
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
