using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Filters;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;

namespace SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;

[Authorize]
[Route("onboarding/employer-search", Name = RouteNames.Onboarding.EmployerSearch)]
[RequiredSessionModel(typeof(OnboardingSessionModel))]
public class EmployerSearchController : Controller
{
    public const string ViewPath = "~/Views/Onboarding/EmployerSearch.cshtml";
    private readonly ISessionService _sessionService;
    private readonly IValidator<EmployerSearchSubmitModel> _validator;

    public EmployerSearchController(ISessionService sessionService, IValidator<EmployerSearchSubmitModel> validator)
    {
        _sessionService = sessionService;
        _validator = validator;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return View(ViewPath, GetViewModel());
    }

    [HttpPost]
    public IActionResult Post(EmployerSearchSubmitModel model)
    {
        var result = _validator.Validate(model);
        if (!result.IsValid)
        {
            result.AddToModelState(ModelState);
            return View(ViewPath, GetViewModel());
        }

        return View(ViewPath, GetViewModel()); // temporary
        // Save employer details
        // check if employer name is missing in which case navigate to Manual entry
    }

    private EmployerSearchViewModel GetViewModel()
    {
        var onboardingSessionModel = _sessionService.Get<OnboardingSessionModel>();
        return new()
        {
            BackLink = onboardingSessionModel.HasSeenPreview ? Url.RouteUrl(RouteNames.Onboarding.CheckYourAnswers)! : Url.RouteUrl(RouteNames.Onboarding.LineManager)!,
            ManualEntryLink = Url.RouteUrl(RouteNames.Onboarding.EmployerDetails)!
        };
    }
}
