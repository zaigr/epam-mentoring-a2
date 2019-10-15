using System;
using System.Threading;
using System.Threading.Tasks;
using AsyncAwait.Task1.CancellationTokens.Calculator.Services;

namespace AsyncAwait.Task1.CancellationTokens.Calculator
{
    public class ConsoleCalculator : ICalculator
    {
        private readonly ICalculatorService _calculatorService;

        public ConsoleCalculator(ICalculatorService calculatorService)
        {
            _calculatorService = calculatorService;
        }

        public void PrintGreeting()
        {
            Console.WriteLine("Mentoring program L2. Async/await.V1. Task 1");
            Console.WriteLine("Calculating the sum of integers from 0 to N.");
        }

        public void StartPrompt()
        {
            Console.WriteLine("Use 'q' key to exit...");
            Console.WriteLine();

            var input = PromptInput();
            while (IsNotExit(input))
            {
                var cts = new CancellationTokenSource();
                if (int.TryParse(input, out var n))
                {
                    StartAsyncSumCalculation(n, cts.Token);
                }
                else
                {
                    Console.WriteLine($"Invalid integer: '{input}'. Please try again.");
                }

                input = PromptInput();
                cts.Cancel();
            }
        }

        private void StartAsyncSumCalculation(int n, CancellationToken token)
        {
            var calculationTask = _calculatorService.GetIntegersSumAsync(n, token);

            calculationTask
                .ContinueWith(HandleCalculationCancellation, TaskContinuationOptions.OnlyOnCanceled);
            calculationTask
                .ContinueWith(task => HandleCalculationSucceeded(n, task), TaskContinuationOptions.OnlyOnRanToCompletion);
            calculationTask
                .ContinueWith(HandleCalculationFaulted, TaskContinuationOptions.OnlyOnFaulted);
        }

        private static string PromptInput()
        {
            Console.WriteLine("Enter N: ");
            return Console.ReadLine();
        }

        private static bool IsNotExit(string input)
            => input.Trim().ToUpper() != "Q";

        private static void HandleCalculationCancellation(Task<long> task)
            => Console.WriteLine("Task cancelled");

        private static void HandleCalculationSucceeded(int n, Task<long> task)
            => Console.WriteLine($"Sum for {n} is {task.Result}");

        private static void HandleCalculationFaulted(Task<long> task)
            => Console.WriteLine($"Calculation failed: {task.Exception.InnerExceptions[0].Message}");
    }
}
