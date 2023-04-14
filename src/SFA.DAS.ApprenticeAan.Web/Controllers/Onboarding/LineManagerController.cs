using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Configuration;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;

namespace SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;

[Authorize]
[Route("onboarding/line-manager", Name = RouteNames.Onboarding.LineManager)]
public class LineManagerController : Controller
{
    public const string ViewPath = "~/Views/Onboarding/LineManager.cshtml";
    private readonly ISessionService _sessionService;
    private readonly IValidator<LineManagerSubmitModel> _validator;

    public LineManagerController(ISessionService sessionService,
        IValidator<LineManagerSubmitModel> validator)
    {
        _validator = validator;
        _sessionService = sessionService;
    }

    [HttpGet]
    public IActionResult Get()
    {
        var sessionModel = _sessionService.Get<OnboardingSessionModel>();

        var model = new LineManagerViewModel()
        {
            HasEmployersApproval = sessionModel.HasEmployersApproval,
            BackLink = Url.RouteUrl(@RouteNames.Onboarding.TermsAndConditions)!
        };
        return View(ViewPath, model);
    }

    [HttpPost]
    public IActionResult Post(LineManagerSubmitModel submitmodel)
    {
        var sessionModel = _sessionService.Get<OnboardingSessionModel>();
        var model = new LineManagerViewModel()
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

        sessionModel.HasEmployersApproval = submitmodel.HasEmployersApproval!;
        _sessionService.Set(sessionModel);

        return RedirectToRoute(RouteNames.Onboarding.EmployerSearch);
    }
}