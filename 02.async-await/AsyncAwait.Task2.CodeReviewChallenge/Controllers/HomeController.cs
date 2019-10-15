using System;
using System.Diagnostics;
using System.Threading.Tasks;
using AsyncAwait.Task2.CodeReviewChallenge.Models.Shared;
using AsyncAwait.Task2.CodeReviewChallenge.Services.Privacy;
using AsyncAwait.Task2.CodeReviewChallenge.Services.Support;
using Microsoft.AspNetCore.Mvc;

namespace AsyncAwait.Task2.CodeReviewChallenge.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAssistanceService _assistanceService;

        private readonly IPrivacyDataService _privacyDataService;

        public HomeController(IAssistanceService assistanceService, IPrivacyDataService privacyDataService)
        {
            _assistanceService = assistanceService ?? throw new ArgumentNullException(nameof(assistanceService));
            _privacyDataService = privacyDataService ?? throw new ArgumentNullException(nameof(privacyDataService));
        }

        public ActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Privacy()
        {
            ViewBag.Message = await _privacyDataService.GetPrivacyDataAsync();

            return View();
        }

        public async Task<IActionResult> Help()
        {
            ViewBag.RequestInfo = await _assistanceService.RequestAssistanceAsync(requestInfo: "guest");

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
