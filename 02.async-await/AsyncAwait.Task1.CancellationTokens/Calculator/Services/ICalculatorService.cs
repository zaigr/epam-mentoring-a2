using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwait.Task1.CancellationTokens.Calculator.Services
{
    public interface ICalculatorService
    {
        Task<long> GetIntegersSumAsync(int n, CancellationToken token);
    }
}
