using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Domain.Constants;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;

namespace SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;

[Route("onboarding/current-job-title", Name = RouteNames.Onboarding.CurrentJobTitle)]
[Authorize]
public class CurrentJobTitleController : Controller
{
    public const string ViewPath = "~/Views/Onboarding/CurrentJobTitle.cshtml";
    private readonly ISessionService _sessionService;
    private readonly IValidator<CurrentJobTitleSubmitModel> _validator;

    public CurrentJobTitleController(ISessionService sessionService,
    IValidator<CurrentJobTitleSubmitModel> validator)
    {
        _sessionService = sessionService;
        _validator = validator;
    }

    [HttpGet]
    public IActionResult Get()
    {
        var sessionModel = _sessionService.Get<OnboardingSessionModel>();
        var model = GetViewModel(sessionModel);

        model.JobTitle = sessionModel.GetProfileValue(ProfileDataId.JobTitle);

        return View(ViewPath, model);
    }

    [HttpPost]
    public IActionResult Post(CurrentJobTitleSubmitModel submitmodel)
    {
        var sessionModel = _sessionService.Get<OnboardingSessionModel>();

        ValidationResult result = _validator.Validate(submitmodel);

        if (!result.IsValid)
        {
            result.AddToModelState(ModelState);
            var model = GetViewModel(sessionModel);
            return View(ViewPath, model);
        }

        sessionModel.SetProfileValue(ProfileDataId.JobTitle, submitmodel.JobTitle!);
        _sessionService.Set(sessionModel);

        return RedirectToRoute(sessionModel.HasSeenPreview ? RouteNames.Onboarding.CheckYourAnswers : RouteNames.Onboarding.Regions);
    }

    private CurrentJobTitleViewModel GetViewModel(OnboardingSessionModel sessionModel)
    {
        return new CurrentJobTitleViewModel()
        {
            BackLink = sessionModel.HasSeenPreview ? Url.RouteUrl(@RouteNames.Onboarding.CheckYourAnswers)! : Url.RouteUrl(@RouteNames.Onboarding.EmployerSearch)!,
        };
    }
}