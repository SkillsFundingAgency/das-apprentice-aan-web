using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Aan.SharedUi.Constants;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;
using SFA.DAS.ApprenticePortal.SharedUi.Menu;

namespace SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;

[Authorize]
[Route("onboarding/previous-engagement", Name = RouteNames.Onboarding.PreviousEngagement)]
[HideNavigationBar(true, true)]
public class PreviousEngagementController : Controller
{
    public const string ViewPath = "~/Views/Onboarding/PreviousEngagement.cshtml";
    private readonly ISessionService _sessionService;
    private readonly IValidator<PreviousEngagementSubmitModel> _validator;

    public PreviousEngagementController(
        ISessionService sessionService,
        IValidator<PreviousEngagementSubmitModel> validator)
    {
        _validator = validator;
        _sessionService = sessionService;
    }

    [HttpGet]
    public IActionResult Get()
    {
        var sessionModel = _sessionService.Get<OnboardingSessionModel>();

        var model = GetViewModel(sessionModel);
        return View(ViewPath, model);
    }

    [HttpPost]
    public IActionResult Post(PreviousEngagementSubmitModel submitModel)
    {
        var sessionModel = _sessionService.Get<OnboardingSessionModel>();

        ValidationResult result = _validator.Validate(submitModel);
        if (!result.IsValid)
        {
            var model = GetViewModel(sessionModel);

            result.AddToModelState(this.ModelState);
            return View(ViewPath, model);
        }

        sessionModel.SetProfileValue(ProfileConstants.ProfileIds.EngagedWithAPreviousAmbassadorInTheNetworkApprentice, submitModel.HasPreviousEngagement.ToString()!);

        sessionModel.HasSeenPreview = true;

        _sessionService.Set(sessionModel);

        return RedirectToRoute(RouteNames.Onboarding.CheckYourAnswers);
    }

    private PreviousEngagementViewModel GetViewModel(OnboardingSessionModel sessionModel)
    {
        var previousEngagement = sessionModel.GetProfileValue(ProfileConstants.ProfileIds.EngagedWithAPreviousAmbassadorInTheNetworkApprentice);
        return new PreviousEngagementViewModel()
        {
            HasPreviousEngagement = bool.TryParse(previousEngagement, out var result) ? result : null,
            BackLink = sessionModel.HasSeenPreview ? Url.RouteUrl(@RouteNames.Onboarding.CheckYourAnswers)! : Url.RouteUrl(@RouteNames.Onboarding.AreasOfInterest)!
        };
    }
}