using System.Threading.Tasks;

namespace AsyncAwait.Task2.CodeReviewChallenge.Services.Privacy
{
    public interface IPrivacyDataService
    {
        Task<string> GetPrivacyDataAsync();
    }
}
