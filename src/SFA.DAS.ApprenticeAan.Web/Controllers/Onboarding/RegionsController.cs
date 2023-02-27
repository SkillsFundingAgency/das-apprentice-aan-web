using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Filters;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;

namespace SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding
{
    [ExcludeFromCodeCoverage]
    [Route("onboarding/regions", Name = RouteNames.Onboarding.Regions)]
    [RequiredSessionModel(typeof(OnboardingSessionModel))]
    public class RegionsController : Controller
    {
        public const string ViewPath = "~/Views/Onboarding/Regions.cshtml";
        private readonly IRegionService _regionService;
        private readonly ISessionService _sessionService;

        public RegionsController(ISessionService sessionService, IRegionService regionService)
        {
            _sessionService = sessionService;
            _regionService = regionService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var model = new RegionsViewModel
            {
                //TODO: Needs to be configured to the correct page in sequence
                BackLink = Url.RouteUrl(RouteNames.Onboarding.TermsAndConditions)!,
                Regions = await _regionService.GetRegions()
            };

            return View(ViewPath, model);
        }

        [HttpPost]
        //TODO: Variables not being passed to controller from View.
        public IActionResult Post(string value)
        {
            var sessionModel = _sessionService.Get<OnboardingSessionModel>();

            _sessionService.Set(sessionModel);
            return Ok(sessionModel);

            //TODO: Redirect to "Why do you want to join the network" page, when developed
            //return RedirectToRoute(RouteNames.Onboarding.TermsAndConditions);
        }
    }
}