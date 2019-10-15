using System;
using AsyncAwait.Task1.CancellationTokens.Calculator;
using AsyncAwait.Task1.CancellationTokens.Calculator.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AsyncAwait.Task1.CancellationTokens
{
    public static partial class Program
    {
        private static class ServiceLocator
        {
            static ServiceLocator()
            {
                var services = new ServiceCollection();

                services.AddTransient<ICalculator, Calculator.ConsoleCalculator>();
                services.AddTransient<ICalculatorService, CalculatorService>();

                Services = services.BuildServiceProvider();
            }

            public static IServiceProvider Services { get; }
        }
    }
}