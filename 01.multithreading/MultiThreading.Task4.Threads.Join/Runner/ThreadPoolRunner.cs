using System.Threading;

namespace MultiThreading.Task4.Threads.Join.Runner
{
    public class ThreadPoolRunner : ParallelRunnerBase
    {
        private readonly SemaphoreSlim _semaphore;

        public ThreadPoolRunner(int runsCount)
            : base(runsCount)
        {
            _semaphore = new SemaphoreSlim(0, 1);
        }

        protected override void StartAction(int actionNumber)
        {
            ThreadPool.QueueUserWorkItem(_ => ActionToStart(actionNumber));

            if (actionNumber == 1)
            {
                _semaphore.Wait();
            }
        }

        protected override void HandleLastActionExecuted()
        {
            _semaphore.Release();
        }
    }
}
