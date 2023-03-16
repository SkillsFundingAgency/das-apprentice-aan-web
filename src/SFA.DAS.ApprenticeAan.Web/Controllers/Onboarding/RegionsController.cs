using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Filters;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;

namespace SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;

[Route("onboarding/regions", Name = RouteNames.Onboarding.Regions)]
[RequiredSessionModel(typeof(OnboardingSessionModel))]
public class RegionsController : Controller
{
    public const string ViewPath = "~/Views/Onboarding/Regions.cshtml";
    private readonly IRegionService _regionService;
    private readonly ISessionService _sessionService;
    private readonly IValidator<RegionsSubmitModel> _validator;

    public RegionsController(ISessionService sessionService, IRegionService regionService, IValidator<RegionsSubmitModel> validator)
    {
        _sessionService = sessionService;
        _regionService = regionService;
        _validator = validator;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var model = new RegionsViewModel
        {
            BackLink = Url.RouteUrl(RouteNames.Onboarding.CurrentJobTitle)!,
            Regions = await _regionService.GetRegions()
        };

        return View(ViewPath, model);
    }

    [HttpPost]
    public async Task<IActionResult> Post(RegionsSubmitModel submitmodel)
    {
        var sessionModel = _sessionService.Get<OnboardingSessionModel>();

        var model = new RegionsViewModel()
        {
            BackLink = Url.RouteUrl(@RouteNames.Onboarding.CurrentJobTitle)!,
            Regions = await _regionService.GetRegions()
        };

        ValidationResult result = _validator.Validate(submitmodel);

        if (!result.IsValid)
        {
            sessionModel.RegionId = null;
            _sessionService.Set(sessionModel);
            return View(ViewPath, model);
        }

        sessionModel.RegionId = submitmodel.SelectedRegionId;
        _sessionService.Set(sessionModel);

        return View(ViewPath, model);
    }
}