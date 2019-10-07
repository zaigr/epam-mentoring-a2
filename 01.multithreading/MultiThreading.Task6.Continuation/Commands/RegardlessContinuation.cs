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

            var child = mainTask
                .ContinueWith(_ => TaskActions.ContinuationTaskAction());

            child.GetAwaiter().GetResult();

            return 0;
        }
    }
}
