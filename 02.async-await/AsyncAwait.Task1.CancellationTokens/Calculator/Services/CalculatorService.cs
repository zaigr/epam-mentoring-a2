using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwait.Task1.CancellationTokens.Calculator.Services
{
    public class CalculatorService : ICalculatorService
    {
        public async Task<long> GetIntegersSumAsync(int n, CancellationToken token)
        {
            return await Task
                .Run(() =>
                    {
                        long sum = 0;
                        for (var i = 0; i < n; i++)
                        {
                            token.ThrowIfCancellationRequested();

                            sum += (i + 1);
                            Thread.Sleep(10);
                        }

                        // throw new Exception("A");

                        return sum;
                    },
                    token);
        }
    }
}
