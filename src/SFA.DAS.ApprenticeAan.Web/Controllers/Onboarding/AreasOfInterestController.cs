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
[Route("onboarding/areas-of-interest", Name = RouteNames.Onboarding.AreasOfInterest)]
[RequiredSessionModel(typeof(OnboardingSessionModel))]
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
    public async Task<IActionResult> Get()
    {
        var model = GetViewModel();

        return View(ViewPath, model);
    }


    [HttpPost]
    public async Task<IActionResult> Post(AreasOfInterestSubmitModel submitmodel)
    {
        var sessionModel = _sessionService.Get<OnboardingSessionModel>();
        var model = GetViewModel();
        ValidationResult result = _validator.Validate(submitmodel);

        if (!result.IsValid)
        {
            sessionModel.RegionId = null;
            result.AddToModelState(ModelState);
            _sessionService.Set(sessionModel);
            return View(ViewPath, model);
        }

        //.Where(x => x.IsSelected).ToList()
        submitmodel.Events.ForEach(x =>
        {
            sessionModel.SetProfileValue(x.Id, x.IsSelected.ToString());
        });

        submitmodel.Promotions.ForEach(x =>
        {
            sessionModel.SetProfileValue(x.Id, x.IsSelected.ToString());
        });

        _sessionService.Set(sessionModel);

        return View(ViewPath, model);
    }

    private AreasOfInterestViewModel GetViewModel()
    {
        var sessionModel = _sessionService.Get<OnboardingSessionModel>();

        return new AreasOfInterestViewModel
        {
            BackLink = Url.RouteUrl(RouteNames.Onboarding.ReasonToJoinTheNetwork)!,
            Events = new List<SelectProfileModel>(sessionModel.ProfileData.Where(x => x.Category == "Events").OrderBy(x => x.Ordering).Select(x => (SelectProfileModel)x)),
            Promotions = new List<SelectProfileModel>(sessionModel.ProfileData.Where(x => x.Category == "Promotions").OrderBy(x => x.Ordering).Select(x => (SelectProfileModel)x))
        };
    }
}