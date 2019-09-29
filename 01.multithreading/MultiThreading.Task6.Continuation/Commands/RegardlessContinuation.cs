using System.Threading.Tasks;
using CommandLine;

namespace MultiThreading.Task6.Continuation.Commands
{
    [Verb("a")]
    public class RegardlessContinuation
    {
        public static int Run()
        {
            var mainTask = Task.Run(TaskActions.MainTaskAction);

            mainTask
                .ContinueWith(_ => TaskActions.ContinuationTaskAction());

            mainTask.GetAwaiter().GetResult();

            return 0;
        }
    }
}
