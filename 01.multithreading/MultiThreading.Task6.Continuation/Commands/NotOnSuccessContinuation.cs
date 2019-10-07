using System.Threading;
using CommandLine;
using System.Threading.Tasks;

namespace MultiThreading.Task6.Continuation.Commands
{
    [Verb("b")]
    public class NotOnSuccessContinuation
    {
        public static int Run()
        {
            var cts = new CancellationTokenSource();

            var token = cts.Token;
            var mainTask = Task.Run(() => TaskActions.MainTaskAction(token), token);

            Thread.Sleep(700);
            cts.Cancel();

            mainTask
                .ContinueWith(
                    _ => TaskActions.ContinuationTaskAction(),
                    TaskContinuationOptions.NotOnRanToCompletion);

            mainTask.GetAwaiter().GetResult();

            return 0;
        }
    }
}
