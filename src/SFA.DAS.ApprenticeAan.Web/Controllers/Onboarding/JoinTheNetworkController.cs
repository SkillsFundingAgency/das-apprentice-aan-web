using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Filters;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;

namespace SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;

[Authorize]
[Route("onboarding/join-the-network", Name = RouteNames.Onboarding.JoinTheNetwork)]
[RequiredSessionModel(typeof(OnboardingSessionModel))]
public class JoinTheNetworkController : Controller
{
    public const string ViewPath = "~/Views/Onboarding/JoinTheNetwork.cshtml";
    private readonly ISessionService _sessionService;
    private readonly IValidator<JoinTheNetworkSubmitModel> _validator;

    public JoinTheNetworkController(ISessionService sessionService,
        IValidator<JoinTheNetworkSubmitModel> validator)
    {
        _validator = validator;
        _sessionService = sessionService;
    }

    [HttpGet]
    public IActionResult Get()
    {
        var sessionModel = _sessionService.Get<OnboardingSessionModel>();

        var model = new JoinTheNetworkViewModel()
        {
            ReasonForJoiningTheNetwork = sessionModel.ApprenticeDetails.ReasonForJoiningTheNetwork,
            BackLink = Url.RouteUrl(@RouteNames.Onboarding.TermsAndConditions)!
        };
        return View(ViewPath, model);
    }

    [HttpPost]
    public IActionResult Post(JoinTheNetworkSubmitModel submitmodel)
    {
        var sessionModel = _sessionService.Get<OnboardingSessionModel>();
        var model = new JoinTheNetworkViewModel()
        {
            BackLink = Url.RouteUrl(@RouteNames.Onboarding.TermsAndConditions)!
        };

        ValidationResult result = _validator.Validate(submitmodel);
        if (!result.IsValid)
        {
            sessionModel.HasEmployersApproval = null;
            _sessionService.Set(sessionModel);

            result.AddToModelState(this.ModelState);
            return View(ViewPath, model);
        }

        sessionModel.ApprenticeDetails.ReasonForJoiningTheNetwork = submitmodel.ReasonForJoiningTheNetwork!;
        _sessionService.Set(sessionModel);

        return View(ViewPath, model);
    }
}