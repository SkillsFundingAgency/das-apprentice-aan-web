using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;

namespace SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;

[Authorize]
[Route("onboarding/reason-to-join", Name = RouteNames.Onboarding.ReasonToJoin)]
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

        var model = new ReasonToJoinViewModel()
        {
            ReasonForJoiningTheNetwork = sessionModel.ApprenticeDetails.ReasonForJoiningTheNetwork,
            BackLink = Url.RouteUrl(@RouteNames.Onboarding.Regions)!
        };
        return View(ViewPath, model);
    }

    [HttpPost]
    public IActionResult Post(ReasonToJoinSubmitModel submitmodel)
    {
        var sessionModel = _sessionService.Get<OnboardingSessionModel>();
        var model = new ReasonToJoinViewModel()
        {
            BackLink = Url.RouteUrl(@RouteNames.Onboarding.Regions)!
        };

        ValidationResult result = _validator.Validate(submitmodel);
        if (!result.IsValid)
        {
            result.AddToModelState(this.ModelState);
            return View(ViewPath, model);
        }

        sessionModel.ApprenticeDetails.ReasonForJoiningTheNetwork = submitmodel.ReasonForJoiningTheNetwork!;
        _sessionService.Set(sessionModel);

        return View(ViewPath, model);
    }
}