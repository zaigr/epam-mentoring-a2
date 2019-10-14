using System;
using System.Net.Http;
using System.Threading.Tasks;
using CloudServices.Interfaces;

namespace AsyncAwait.Task2.CodeReviewChallenge.Services.Support
{
    public class ManualAssistanceService : IAssistanceService
    {
        private readonly ISupportService _supportService;

        public ManualAssistanceService(ISupportService supportService)
        {
            _supportService = supportService ?? throw new ArgumentNullException(nameof(supportService));
        }

        public async Task<string> RequestAssistanceAsync(string requestInfo)
        {
            try
            {
                await _supportService.RegisterSupportRequestAsync(requestInfo);

                return await _supportService.GetSupportInfoAsync(requestInfo);
            }
            catch (HttpRequestException ex)
            {
                return $"Failed to register assistance request. Please try later. {ex.Message}";
            }
        }
    }
}
