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
public class RegionController : Controller
{
    public const string ViewPath = "~/Views/Onboarding/Regions.cshtml";
    private readonly IRegionService _regionService;
    private readonly ISessionService _sessionService;
    private readonly IValidator<RegionSubmitModel> _validator;

    public RegionController(ISessionService sessionService, IRegionService regionService, IValidator<RegionSubmitModel> validator)
    {
        _sessionService = sessionService;
        _regionService = regionService;
        _validator = validator;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var model = new RegionViewModel
        {
            BackLink = Url.RouteUrl(RouteNames.Onboarding.CurrentJobTitle)!,
            Regions = await _regionService.GetRegions()
        };

        return View(ViewPath, model);
    }

    [HttpPost]
    public async Task<IActionResult> Post(RegionSubmitModel submitmodel)
    {
        var sessionModel = _sessionService.Get<OnboardingSessionModel>();

        var model = new RegionViewModel()
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