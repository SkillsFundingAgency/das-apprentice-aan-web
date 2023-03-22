using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Domain.Constants;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Filters;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;

namespace SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;

[Route("onboarding/current-job-title", Name = RouteNames.Onboarding.CurrentJobTitle)]
[RequiredSessionModel(typeof(OnboardingSessionModel))]
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
        var model = GetViewModel();

        var sessionModel = _sessionService.Get<OnboardingSessionModel>();
        model.EnteredJobTitle = sessionModel.ProfileData.GetProfileModelValue(ProfileDataId.JobTitle);

        return View(ViewPath, model);
    }

    [HttpPost]
    public IActionResult Post(CurrentJobTitleSubmitModel submitmodel)
    {
        var sessionModel = _sessionService.Get<OnboardingSessionModel>();
        var model = GetViewModel();

        ValidationResult result = _validator.Validate(submitmodel);

        if (!result.IsValid)
        {
            sessionModel.ProfileData.UpdateProfileModelValue(ProfileDataId.JobTitle, null);
            _sessionService.Set(sessionModel);
            result.AddToModelState(ModelState);
            return View(ViewPath, model);
        }

        sessionModel.ProfileData.UpdateProfileModelValue(ProfileDataId.JobTitle, submitmodel.EnteredJobTitle!);
        _sessionService.Set(sessionModel);

        return View(ViewPath, model);
    }

    private CurrentJobTitleViewModel GetViewModel()
    {
        return new CurrentJobTitleViewModel()
        {
            BackLink = Url.RouteUrl(@RouteNames.Onboarding.NameOfEmployer)!,
        };
    }
}