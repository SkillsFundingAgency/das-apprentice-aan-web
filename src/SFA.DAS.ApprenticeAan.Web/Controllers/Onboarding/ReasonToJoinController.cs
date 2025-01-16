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
[Route("onboarding/reason-to-join", Name = RouteNames.Onboarding.ReasonToJoin)]
[HideNavigationBar(true, true)]
public class ReasonToJoinController : Controller
{
    public const string ViewPath = "~/Views/Onboarding/ReasonToJoin.cshtml";
    private readonly ISessionService _sessionService;
    private readonly IValidator<ReasonToJoinSubmitModel> _validator;

    public ReasonToJoinController(ISessionService sessionService,
        IValidator<ReasonToJoinSubmitModel> validator)
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
    public IActionResult Post(ReasonToJoinSubmitModel submitModel)
    {
        var sessionModel = _sessionService.Get<OnboardingSessionModel>();

        ValidationResult result = _validator.Validate(submitModel);
        if (!result.IsValid)
        {
            result.AddToModelState(ModelState);
            return View(ViewPath, GetViewModel(sessionModel));
        }
        sessionModel.SetProfileValue(ProfileConstants.ProfileIds.ReasonToJoinAmbassadorNetwork, submitModel.ReasonForJoiningTheNetwork!.Trim());

        _sessionService.Set(sessionModel);

        return RedirectToRoute(sessionModel.HasSeenPreview ? RouteNames.Onboarding.CheckYourAnswers : RouteNames.Onboarding.ReceiveNotifications);
    }

    private ReasonToJoinViewModel GetViewModel(OnboardingSessionModel sessionModel)
    {
        var model = new ReasonToJoinViewModel()
        {
            ReasonForJoiningTheNetwork = sessionModel.GetProfileValue(ProfileConstants.ProfileIds.ReasonToJoinAmbassadorNetwork),
            BackLink = sessionModel.HasSeenPreview ? Url.RouteUrl(@RouteNames.Onboarding.CheckYourAnswers)! : Url.RouteUrl(RouteNames.Onboarding.AreasOfInterest)!
        };
        return model;
    }
}