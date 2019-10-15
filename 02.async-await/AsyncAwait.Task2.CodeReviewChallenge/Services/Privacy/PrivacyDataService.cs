using System.Threading.Tasks;

namespace AsyncAwait.Task2.CodeReviewChallenge.Services.Privacy
{
    public class PrivacyDataService : IPrivacyDataService
    {
        public Task<string> GetPrivacyDataAsync()
        {
            var response = "This Policy describes how async/await processes your personal data," +
                           "but it may not address all possible data processing scenarios.";

            return Task.FromResult(response);
        }
    }
}
