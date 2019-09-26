/*
 * 5. Write a program which creates two threads and a shared collection:
 * the first one should add 10 elements into the collection and the second should print all elements
 * in the collection after each adding.
 * Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.
 */
using System;
using System.Collections.Concurrent;
using System.Threading;

namespace MultiThreading.Task5.Threads.SharedCollection
{
    class Program
    {
        private static readonly SemaphoreSlim Semaphore = new SemaphoreSlim(0);

        private static readonly ConcurrentQueue<string> ConcurrentQueue = new ConcurrentQueue<string>();

        static void Main(string[] args)
        {
            var producerThread = new Thread(() => Produce(numberOfItems: 10));
            var consumerThread = new Thread(() => Consume(numberOfItems: 10));

            producerThread.Start();
            consumerThread.Start();

            producerThread.Join();
            consumerThread.Join();
        }

        private static void Produce(int numberOfItems)
        {
            for (var item = 0; item < numberOfItems; item++)
            {
                ConcurrentQueue.Enqueue(item.ToString());
                Semaphore.Release();
            }
        }

        private static void Consume(int numberOfItems)
        {
            var consumedItems = 0;
            while (true)
            {
                Semaphore.Wait();

                if (ConcurrentQueue.TryDequeue(out var item))
                {
                    Console.WriteLine(item);
                    consumedItems++;
                }

                if (consumedItems == numberOfItems)
                {
                    return;
                }
            }
        }
    }
}
