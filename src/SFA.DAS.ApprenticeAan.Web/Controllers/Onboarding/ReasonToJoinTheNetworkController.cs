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
[Route("onboarding/reason-to-join-the-network", Name = RouteNames.Onboarding.ReasonToJoinTheNetwork)]
[RequiredSessionModel(typeof(OnboardingSessionModel))]
public class ReasonToJoinTheNetworkController : Controller
{
    public const string ViewPath = "~/Views/Onboarding/ReasonToJoinTheNetwork.cshtml";
    private readonly ISessionService _sessionService;
    private readonly IValidator<ReasonToJoinTheNetworkSubmitModel> _validator;

    public ReasonToJoinTheNetworkController(ISessionService sessionService,
        IValidator<ReasonToJoinTheNetworkSubmitModel> validator)
    {
        _validator = validator;
        _sessionService = sessionService;
    }

    [HttpGet]
    public IActionResult Get()
    {
        var sessionModel = _sessionService.Get<OnboardingSessionModel>();
        var reasonToJoinTheNetwork = sessionModel.GetProfileValue(ProfileDataId.EngagedWithAPreviousAmbassadorInTheNetwork);

        var model = new ReasonToJoinTheNetworkViewModel()
        {
            EngagedWithAPreviousAmbassadorInTheNetwork = reasonToJoinTheNetwork == null ?null :bool.Parse(reasonToJoinTheNetwork!),
            BackLink = Url.RouteUrl(@RouteNames.Onboarding.AreasOfInterest)!
        };
        return View(ViewPath, model);
    }

    [HttpPost]
    public IActionResult Post(ReasonToJoinTheNetworkSubmitModel submitmodel)
    {
        var sessionModel = _sessionService.Get<OnboardingSessionModel>();
        var model = new ReasonToJoinTheNetworkViewModel()
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

        return View(ViewPath, model);
    }
}