using System;
using System.Threading;

namespace MultiThreading.Task4.Threads.Join.Runner
{
    public abstract class ParallelRunnerBase
    {
        private readonly int _runsCount;

        protected ParallelRunnerBase(int runsCount)
        {
            _runsCount = runsCount;
        }

        public void Run()
        {
            StartAction(1);
        }

        protected  abstract void StartAction(int actionNumber);

        protected abstract void HandleLastActionExecuted();

        protected void ActionToStart(int actionNumber)
        {
            Console.WriteLine($"Thread {actionNumber}; ThreadId - {Thread.CurrentThread.ManagedThreadId}");

            actionNumber++;

            if (actionNumber > _runsCount)
            {
                HandleLastActionExecuted();
                return;
            }

            StartAction(actionNumber);
        }
    }
}
