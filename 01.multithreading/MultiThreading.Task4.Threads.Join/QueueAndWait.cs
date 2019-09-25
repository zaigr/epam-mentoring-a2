using System;
using System.Threading;

namespace MultiThreading.Task4.Threads.Join
{
    public static class QueueAndWait
    {
        private static int _treadsCount;

        private static SemaphoreSlim _lockedSemaphore;

        public static void QueueTasksAndWait(int threadsCount)
        {
            _treadsCount = threadsCount;

            _lockedSemaphore = new SemaphoreSlim(0, 1);

            ThreadPool.QueueUserWorkItem(_ => StartSubThread(1));

            _lockedSemaphore.Wait();
        }

        private static void StartSubThread(int threadNumber)
        {
            Console.WriteLine($"Thread {threadNumber}; ThreadId - {Thread.CurrentThread.ManagedThreadId}");

            threadNumber++;

            if (threadNumber > _treadsCount)
            {
                _lockedSemaphore.Release();
                return;
            }

            ThreadPool.QueueUserWorkItem(_ => StartSubThread(threadNumber));
        }
    }
}
