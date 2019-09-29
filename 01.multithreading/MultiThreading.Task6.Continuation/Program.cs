/*
*  Create a Task and attach continuations to it according to the following criteria:
   a.   RegardlessContinuation - Continuation task should be executed regardless of the result of the parent task.
   b.   NotOnSuccessContinuation - Continuation task should be executed when the parent task finished without success.
   c.   OnFailureSynchronouslyContinuation -  Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation
   d.   OnCancelledAsynchronousContinuation - Continuation task should be executed outside of the thread pool when the parent task would be cancelled
   Demonstrate the work of the each case with console utility.
*/
using CommandLine;
using MultiThreading.Task6.Continuation.Commands;

namespace MultiThreading.Task6.Continuation
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default
                .ParseArguments<RegardlessContinuation, NotOnSuccessContinuation,
                    OnFailureSynchronouslyContinuation, OnCanceledAsynchronousContinuation>(args)
                .MapResult(
                    (RegardlessContinuation _) => RegardlessContinuation.Run(),
                    (NotOnSuccessContinuation _) => NotOnSuccessContinuation.Run(),
                    (OnFailureSynchronouslyContinuation _) => OnFailureSynchronouslyContinuation.Run(),
                    (OnCanceledAsynchronousContinuation _) => OnCanceledAsynchronousContinuation.Run(),
                    errors => 1);
        }
    }
}
