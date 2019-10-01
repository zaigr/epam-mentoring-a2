/*
 * 4.	Write a program which recursively creates 10 threads.
 * Each thread should be with the same body and receive a state with integer number, decrement it,
 * print and pass as a state into the newly created thread.
 * Use Thread class for this task and Join for waiting threads.
 * 
 * Implement all of the following options:
 * - a) Use Thread class for this task and Join for waiting threads.
 * - b) ThreadPool class for this task and Semaphore for waiting threads.
 */

using MultiThreading.Task4.Threads.Join.Runner;

namespace MultiThreading.Task4.Threads.Join
{
    public static class Program
    {
        static void Main(string[] args)
        {
            ParallelRunnerBase runner = new ThreadPoolRunner(runsCount: 10);
            // ParallelRunnerBase runner = new ThreadRunner(runsCount: 10);

            runner.Run();
        }
    }
}
