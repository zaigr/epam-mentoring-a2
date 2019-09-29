using CommandLine;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task6.Continuation.Commands
{
    [Verb("d")]
    public class OnCanceledAsynchronousContinuation
    {
        public static int Run()
        {
            var cts = new CancellationTokenSource();

            var token = cts.Token;
            var mainTask = Task.Run(() => TaskActions.MainTaskAction(token), token);

            Thread.Sleep(500);
            cts.Cancel();

            mainTask
                .ContinueWith(
                    _ => TaskActions.ContinuationTaskAction(),
                    TaskContinuationOptions.OnlyOnCanceled |
                    TaskContinuationOptions.RunContinuationsAsynchronously);

            mainTask.GetAwaiter().GetResult();

            return 0;
        }

    }
}
