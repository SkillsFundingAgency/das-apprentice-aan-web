using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;
using SFA.DAS.ApprenticePortal.Authentication;

namespace SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;

[Authorize]
[Route("onboarding/check-your-answers", Name = RouteNames.Onboarding.CheckYourAnswers)]
public class CheckYourAnswersController : Controller
{
    public const string ViewPath = "~/Views/Onboarding/CheckYourAnswers.cshtml";
    private readonly ISessionService _sessionService;
    private readonly IOuterApiClient _outerApiClient;

    public CheckYourAnswersController(ISessionService sessionService, IOuterApiClient outerApiClient)
    {
        _sessionService = sessionService;
        _outerApiClient = outerApiClient;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var sessionModel = _sessionService.Get<OnboardingSessionModel>();
        sessionModel.HasSeenPreview = true;
        _sessionService.Set(sessionModel);

        var myApprenticeship = await _outerApiClient.GetMyApprenticeship(Guid.Parse(User.ApprenticeIdClaim()!.Value));

        CheckYourAnswersViewModel model = new(Url, sessionModel, User, myApprenticeship);
        return View(ViewPath, model);
    }

    [HttpPost]
    public IActionResult Post(CheckYourAnswersViewModel model)
    {
        var sessionModel = _sessionService.Get<OnboardingSessionModel>();
        /// CheckYourAnswersViewModel model = new(Url, sessionModel, User, myApprenticeship);
        /// Transform OnboardinSessionModel to create apprentice request model
        /// validate create apprentice request model (or forward validations from inner api) 
        /// Show validation errors
        /// Post and navigate to complete page 
        /// Trim all user inputs

        return RedirectToRoute(RouteNames.NetworkHub);
    }

}