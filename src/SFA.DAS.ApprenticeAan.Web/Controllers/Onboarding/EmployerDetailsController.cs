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
[Route("onboarding/employer-details", Name = RouteNames.Onboarding.EmployerDetails)]
[HideNavigationBar(true, true)]
public class EmployerDetailsController : Controller
{
    public const string ViewPath = "~/Views/Onboarding/EmployerDetails.cshtml";

    private readonly ISessionService _sessionService;
    private readonly IValidator<EmployerDetailsSubmitModel> _validator;
    private readonly IOuterApiClient _outerApiClient;

    public EmployerDetailsController(
        ISessionService sessionService,
        IValidator<EmployerDetailsSubmitModel> validator,
        IOuterApiClient outerApiClient)
    {
        _validator = validator;
        _sessionService = sessionService;
        _outerApiClient = outerApiClient;
    }

    [HttpGet]
    public IActionResult GetEmployerDetails()
    {
        var sessionModel = _sessionService.Get<OnboardingSessionModel>();

        var model = new EmployerDetailsViewModel()
        {
            EmployerName = sessionModel.GetProfileValue(ProfileConstants.ProfileIds.EmployerName),
            AddressLine1 = sessionModel.GetProfileValue(ProfileConstants.ProfileIds.EmployerAddress1),
            AddressLine2 = sessionModel.GetProfileValue(ProfileConstants.ProfileIds.EmployerAddress2),
            Town = sessionModel.GetProfileValue(ProfileConstants.ProfileIds.EmployerTownOrCity),
            County = sessionModel.GetProfileValue(ProfileConstants.ProfileIds.EmployerCounty),
            Postcode = sessionModel.GetProfileValue(ProfileConstants.ProfileIds.EmployerPostcode),
            Longitude = sessionModel.GetProfileValue(ProfileConstants.ProfileIds.EmployerAddressLongitude) != null ? Convert.ToDouble(sessionModel.GetProfileValue(ProfileConstants.ProfileIds.EmployerAddressLongitude)) : null,
            Latitude = sessionModel.GetProfileValue(ProfileConstants.ProfileIds.EmployerAddressLatitude) != null ? Convert.ToDouble(sessionModel.GetProfileValue(ProfileConstants.ProfileIds.EmployerAddressLatitude)) : null,

            BackLink = Url.RouteUrl(@RouteNames.Onboarding.EmployerSearch)!
        };
        return View(ViewPath, model);
    }

    [HttpPost]
    public async Task<IActionResult> PostEmployerDetails(EmployerDetailsSubmitModel submitModel)
    {
        var model = new EmployerDetailsViewModel()
        {
            BackLink = Url.RouteUrl(RouteNames.Onboarding.EmployerSearch)!
        };

        FluentValidation.Results.ValidationResult result = _validator.Validate(submitModel);
        if (!result.IsValid)
        {
            result.AddToModelState(this.ModelState);
            return View(ViewPath, model);
        }

        var sessionModel = _sessionService.Get<OnboardingSessionModel>();

        sessionModel.SetProfileValue(ProfileConstants.ProfileIds.EmployerName, submitModel.EmployerName?.Trim()!);
        sessionModel.SetProfileValue(ProfileConstants.ProfileIds.EmployerAddress1, submitModel.AddressLine1?.Trim()!);
        sessionModel.SetProfileValue(ProfileConstants.ProfileIds.EmployerAddress2, submitModel.AddressLine2?.Trim()!);
        sessionModel.SetProfileValue(ProfileConstants.ProfileIds.EmployerCounty, submitModel.County?.Trim()!);
        sessionModel.SetProfileValue(ProfileConstants.ProfileIds.EmployerTownOrCity, submitModel.Town?.Trim()!);
        sessionModel.SetProfileValue(ProfileConstants.ProfileIds.EmployerPostcode, submitModel.Postcode?.Trim()!);

        var apiResponse = await _outerApiClient.GetCoordinates(submitModel.Postcode!);
        if (apiResponse.ResponseMessage.StatusCode == System.Net.HttpStatusCode.OK)
        {
            var coordinates = apiResponse.GetContent();
            sessionModel.SetProfileValue(ProfileConstants.ProfileIds.EmployerAddressLongitude, coordinates.Longitude.ToString());
            sessionModel.SetProfileValue(ProfileConstants.ProfileIds.EmployerAddressLatitude, coordinates.Latitude.ToString());
        }
        else
        {
            sessionModel.ClearProfileValue(ProfileConstants.ProfileIds.EmployerAddressLongitude);
            sessionModel.ClearProfileValue(ProfileConstants.ProfileIds.EmployerAddressLatitude);
        }

        _sessionService.Set(sessionModel);

        return RedirectToRoute(sessionModel.HasSeenPreview ? RouteNames.Onboarding.CheckYourAnswers : RouteNames.Onboarding.CurrentJobTitle);
    }
}