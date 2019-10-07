using System;
using System.Threading;

namespace MultiThreading.Task6.Continuation
{
    public static class TaskActions
    {
        public static void MainTaskAction()
        {
            Console.WriteLine($"Main task started. ThreadId - {Thread.CurrentThread.ManagedThreadId}");

            // throw new Exception("A");

            Thread.Sleep(3000);

            Console.WriteLine("Main task executed.");
        }

        public static void MainTaskAction(CancellationToken cancellationToken)
        {
            Console.WriteLine($"Main task started. ThreadId - {Thread.CurrentThread.ManagedThreadId}");

            // throw new Exception("A");

            for (int i = 0; i < 3; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();
                Thread.Sleep(1000);
            }

            Console.WriteLine("Main task executed.");
        }

        public static void ContinuationTaskAction()
        {
            Console.WriteLine($"Continuation task started. ThreadId - {Thread.CurrentThread.ManagedThreadId}");

            Thread.Sleep(1000);

            Console.WriteLine("Continuation task executed.");
        }
    }
}
