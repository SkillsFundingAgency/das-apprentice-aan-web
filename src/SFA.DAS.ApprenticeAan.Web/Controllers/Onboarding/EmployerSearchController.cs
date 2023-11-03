using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Aan.SharedUi.Constants;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;
using SFA.DAS.ApprenticePortal.SharedUi.Menu;

namespace SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;

[Authorize]
[Route("onboarding/employer-search", Name = RouteNames.Onboarding.EmployerSearch)]
[HideNavigationBar(true, true)]
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
    public IActionResult Post(EmployerSearchSubmitModel submitModel)
    {
        var result = _validator.Validate(submitModel);
        if (!result.IsValid)
        {
            result.AddToModelState(ModelState);
            return View(ViewPath, GetViewModel());
        }

        var sessionModel = _sessionService.Get<OnboardingSessionModel>();

        sessionModel.SetProfileValue(ProfileConstants.ProfileIds.EmployerName, submitModel.OrganisationName!);
        sessionModel.SetProfileValue(ProfileConstants.ProfileIds.EmployerAddress1, submitModel.AddressLine1!);
        sessionModel.SetProfileValue(ProfileConstants.ProfileIds.EmployerAddress2, submitModel.AddressLine2!);
        sessionModel.SetProfileValue(ProfileConstants.ProfileIds.EmployerCounty, submitModel.County!);
        sessionModel.SetProfileValue(ProfileConstants.ProfileIds.EmployerTownOrCity, submitModel.Town!);
        sessionModel.SetProfileValue(ProfileConstants.ProfileIds.EmployerPostcode, submitModel.Postcode!);
        sessionModel.SetProfileValue(ProfileConstants.ProfileIds.EmployerAddressLongitude, submitModel.Longitude.ToString()!);
        sessionModel.SetProfileValue(ProfileConstants.ProfileIds.EmployerAddressLatitude, submitModel.Latitude.ToString()!);

        _sessionService.Set(sessionModel);

        return RedirectToRoute(RouteNames.Onboarding.EmployerDetails);
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
