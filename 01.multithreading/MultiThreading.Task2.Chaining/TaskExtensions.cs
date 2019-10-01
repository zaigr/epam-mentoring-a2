using System;
using System.Threading.Tasks;

namespace MultiThreading.Task2.Chaining
{
    public static class TaskExtensions
    {
        public static Task<TResult> ContinueWithOnSuccess<TResult>(this Task<TResult> task, Func<TResult, TResult> function)
        {
            return task
                .ContinueWith(
                    (parent, state) => function(parent.Result),
                    TaskContinuationOptions.OnlyOnRanToCompletion);
        }

        public static Task<double> ContinueWithOnSuccess<TResult>(this Task<TResult> task, Func<TResult, double> function)
        {
            return task
                .ContinueWith(
                    (parent, state) => function(parent.Result),
                    TaskContinuationOptions.OnlyOnRanToCompletion);
        }
    }
}
