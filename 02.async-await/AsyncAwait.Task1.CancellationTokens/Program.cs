using AsyncAwait.Task1.CancellationTokens.Calculator;
using Microsoft.Extensions.DependencyInjection;

namespace AsyncAwait.Task1.CancellationTokens
{
    public static partial class Program
    {
        public static void Main(string[] args)
        {
            var calculator = ServiceLocator.Services.GetService<ICalculator>();

            calculator.PrintGreeting();

            calculator.StartPrompt();
        }
    }
}