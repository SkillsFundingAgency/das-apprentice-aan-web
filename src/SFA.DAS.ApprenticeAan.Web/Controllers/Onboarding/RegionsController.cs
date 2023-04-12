using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;

namespace SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;

[Authorize]
[Route("onboarding/regions", Name = RouteNames.Onboarding.Regions)]
public class RegionsController : Controller
{
    public const string ViewPath = "~/Views/Onboarding/Regions.cshtml";
    private readonly IRegionsService _regionService;
    private readonly ISessionService _sessionService;
    private readonly IValidator<RegionsSubmitModel> _validator;

    public RegionsController(ISessionService sessionService, IRegionsService regionService, IValidator<RegionsSubmitModel> validator)
    {
        _sessionService = sessionService;
        _regionService = regionService;
        _validator = validator;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var sessionModel = _sessionService.Get<OnboardingSessionModel>();
        var model = await GetViewModel(sessionModel);
        model.SelectedRegionId = sessionModel.RegionId;

        return View(ViewPath, model);
    }

    [HttpPost]
    public async Task<IActionResult> Post(RegionsSubmitModel submitmodel)
    {
        var sessionModel = _sessionService.Get<OnboardingSessionModel>();
        var model = await GetViewModel(sessionModel);
        ValidationResult result = _validator.Validate(submitmodel);

        if (!result.IsValid)
        {
            sessionModel.RegionId = null;
            result.AddToModelState(ModelState);
            _sessionService.Set(sessionModel);
            return View(ViewPath, model);
        }

        sessionModel.RegionId = submitmodel.SelectedRegionId;
        sessionModel.RegionName = model.Regions.Find(x => x.Id == sessionModel.RegionId).Area;
        _sessionService.Set(sessionModel);

        return RedirectToRoute(RouteNames.Onboarding.ReasonToJoin);
    }

    private async Task<RegionsViewModel> GetViewModel(OnboardingSessionModel sessionModel)
    {
        return new RegionsViewModel
        {
            BackLink = sessionModel.HasSeenPreview ? Url.RouteUrl(@RouteNames.Onboarding.CheckYourAnswers)! : Url.RouteUrl(RouteNames.Onboarding.CurrentJobTitle)!,
            Regions = await _regionService.GetRegions()
        };
    }
}