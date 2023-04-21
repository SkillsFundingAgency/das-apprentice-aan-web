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

[Authorize]
[Route("onboarding/areas-of-interest", Name = RouteNames.Onboarding.AreasOfInterest)]
public class AreasOfInterestController : Controller
{
    public const string ViewPath = "~/Views/Onboarding/AreasOfInterest.cshtml";
    private readonly ISessionService _sessionService;
    private readonly IValidator<AreasOfInterestSubmitModel> _validator;

    public AreasOfInterestController(ISessionService sessionService, IValidator<AreasOfInterestSubmitModel> validator)
    {
        _sessionService = sessionService;
        _validator = validator;
    }

    [HttpGet]
    public IActionResult Get()
    {
        var model = GetViewModel();
        return View(ViewPath, model);
    }

    [HttpPost]
    public IActionResult Post(AreasOfInterestSubmitModel submitmodel)
    {
        var sessionModel = _sessionService.Get<OnboardingSessionModel>();
        var model = GetViewModel();
        ValidationResult result = _validator.Validate(submitmodel);

        if (!result.IsValid)
        {
            result.AddToModelState(ModelState);
            return View(ViewPath, model);
        }

        submitmodel.AreasOfInterest.ToList().ForEach(x =>
        {
            sessionModel.SetProfileValue(x.Id, x.IsSelected ? true.ToString() : null!);
        });
        _sessionService.Set(sessionModel);

        return RedirectToRoute(sessionModel.HasSeenPreview ? RouteNames.Onboarding.CheckYourAnswers : RouteNames.Onboarding.PreviousEngagement);
    }

    private AreasOfInterestViewModel GetViewModel()
    {
        var sessionModel = _sessionService.Get<OnboardingSessionModel>();

        return new AreasOfInterestViewModel
        {
            BackLink = sessionModel.HasSeenPreview ? Url.RouteUrl(@RouteNames.Onboarding.CheckYourAnswers)! : Url.RouteUrl(RouteNames.Onboarding.ReasonToJoin)!,
            Events = new List<SelectProfileModel>(sessionModel.ProfileData.Where(x => x.Category == Category.Events).OrderBy(x => x.Ordering).Select(x => (SelectProfileModel)x)),
            Promotions = new List<SelectProfileModel>(sessionModel.ProfileData.Where(x => x.Category == Category.Promotions).OrderBy(x => x.Ordering).Select(x => (SelectProfileModel)x))
        };
    }
}