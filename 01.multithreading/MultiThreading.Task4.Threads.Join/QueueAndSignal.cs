using System;
using System.Threading;

namespace MultiThreading.Task4.Threads.Join
{
    public static class QueueAndSignal
    {
        private static readonly AutoResetEvent Signal = new AutoResetEvent(false);

        private static int _threadCount;

        public static void Start(int threadsCount)
        {
            _threadCount = threadsCount;

            ThreadPool.QueueUserWorkItem(_ => StartSubThread(1));

            Signal.WaitOne();
        }

        private static void StartSubThread(int threadNumber)
        {
            Console.WriteLine($"Thread {threadNumber}; ThreadId - {Thread.CurrentThread.ManagedThreadId}");

            threadNumber++;

            if (threadNumber > _threadCount)
            {
                Signal.Set();
            }

            ThreadPool.QueueUserWorkItem(_ => StartSubThread(threadNumber));
        }
    }
}
