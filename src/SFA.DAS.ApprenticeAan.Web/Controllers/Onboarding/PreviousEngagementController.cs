using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Domain.Constants;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Filters;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;

namespace SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;

[Authorize]
[Route("onboarding/previous-engagement", Name = RouteNames.Onboarding.PreviousEngagement)]
[RequiredSessionModel(typeof(OnboardingSessionModel))]
public class PreviousEngagementController : Controller
{
    public const string ViewPath = "~/Views/Onboarding/PreviousEngagement.cshtml";
    private readonly ISessionService _sessionService;
    private readonly IValidator<PreviousEngagementSubmitModel> _validator;

    public PreviousEngagementController(ISessionService sessionService,
        IValidator<PreviousEngagementSubmitModel> validator)
    {
        _validator = validator;
        _sessionService = sessionService;
    }

    [HttpGet]
    public IActionResult Get()
    {
        var sessionModel = _sessionService.Get<OnboardingSessionModel>();
        var previousEngagement = sessionModel.GetProfileValue(ProfileDataId.EngagedWithAPreviousAmbassadorInTheNetwork);

        var model = new PreviousEngagementViewModel()
        {
            EngagedWithAPreviousAmbassadorInTheNetwork = previousEngagement == null ? null : bool.Parse(previousEngagement!),
            BackLink = Url.RouteUrl(@RouteNames.Onboarding.AreasOfInterest)!
        };
        return View(ViewPath, model);
    }

    [HttpPost]
    public IActionResult Post(PreviousEngagementSubmitModel submitmodel)
    {
        var sessionModel = _sessionService.Get<OnboardingSessionModel>();
        var model = new PreviousEngagementViewModel()
        {
            BackLink = Url.RouteUrl(@RouteNames.Onboarding.AreasOfInterest)!
        };

        ValidationResult result = _validator.Validate(submitmodel);
        if (!result.IsValid)
        {
            result.AddToModelState(this.ModelState);
            return View(ViewPath, model);
        }

        sessionModel.SetProfileValue(ProfileDataId.EngagedWithAPreviousAmbassadorInTheNetwork,
             submitmodel.EngagedWithAPreviousAmbassadorInTheNetwork.ToString()!);

        _sessionService.Set(sessionModel);

        return RedirectToRoute(RouteNames.Onboarding.CheckYourAnswers);
    }
}