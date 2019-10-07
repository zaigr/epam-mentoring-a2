using CommandLine;
using System.Threading.Tasks;

namespace MultiThreading.Task6.Continuation.Commands
{
    [Verb("c")]
    public class OnFailureSynchronouslyContinuation
    {
        public static int Run()
        {
            var mainTask = Task.Run(TaskActions.MainTaskAction);

            mainTask
                .ContinueWith(
                    _ => TaskActions.ContinuationTaskAction(),
                    TaskContinuationOptions.OnlyOnFaulted |
                    TaskContinuationOptions.ExecuteSynchronously);

            mainTask.GetAwaiter().GetResult();

            return 0;
        }
    }
}
