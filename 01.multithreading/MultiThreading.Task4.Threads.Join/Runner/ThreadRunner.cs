using System.Threading;

namespace MultiThreading.Task4.Threads.Join.Runner
{
    public class ThreadRunner : ParallelRunnerBase
    {
        public ThreadRunner(int runsCount)
            : base(runsCount)
        {
        }

        protected override void StartAction(int actionNumber)
        {
            var thread = new Thread(() => ActionToStart(actionNumber));
            thread.Start();

            thread.Join();
        }

        protected override void HandleLastActionExecuted()
        {
        }
    }
}
