using System;
using System.Threading;

namespace MultiThreading.Task4.Threads.Join
{
    public static class StartAndJoin
    {
        private static int _threadsCount;

        public static void StartThreadsAndJoin(int threadsCount)
        {
            _threadsCount = threadsCount;

            var t = new Thread(() => StartSubThread(1));
            t.Start();

            t.Join();
        }

        private static void StartSubThread(int threadNumber)
        {
            Console.WriteLine($"Thread {threadNumber}; ThreadId - {Thread.CurrentThread.ManagedThreadId}");

            threadNumber++;

            if (threadNumber > _threadsCount)
            {
                return;
            }

            var thread = new Thread(() => StartSubThread(threadNumber));
            thread.Start();

            thread.Join();
        }
    }
}
