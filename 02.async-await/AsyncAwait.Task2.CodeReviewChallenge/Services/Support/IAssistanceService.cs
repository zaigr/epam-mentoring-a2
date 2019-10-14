using System.Threading.Tasks;

namespace AsyncAwait.Task2.CodeReviewChallenge.Services.Support
{
    public interface IAssistanceService
    {
        Task<string> RequestAssistanceAsync(string requestInfo);
    }
}
