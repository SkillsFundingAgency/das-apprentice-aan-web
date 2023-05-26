using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Domain.Constants;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;

namespace SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;

[Authorize]
[Route("onboarding/employer-details", Name = RouteNames.Onboarding.EmployerDetails)]
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
            EmployerName = sessionModel.GetProfileValue(ProfileDataId.EmployerName),
            AddressLine1 = sessionModel.GetProfileValue(ProfileDataId.AddressLine1),
            AddressLine2 = sessionModel.GetProfileValue(ProfileDataId.AddressLine2),
            Town = sessionModel.GetProfileValue(ProfileDataId.Town),
            County = sessionModel.GetProfileValue(ProfileDataId.County),
            Postcode = sessionModel.GetProfileValue(ProfileDataId.Postcode),
            Longitude = sessionModel.GetProfileValue(ProfileDataId.Longitude) != null ? Convert.ToDouble(sessionModel.GetProfileValue(ProfileDataId.Longitude)) : null,
            Latitude = sessionModel.GetProfileValue(ProfileDataId.Latitude) != null ? Convert.ToDouble(sessionModel.GetProfileValue(ProfileDataId.Latitude)) : null,

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

        sessionModel.SetProfileValue(ProfileDataId.EmployerName, submitModel.EmployerName?.Trim()!);
        sessionModel.SetProfileValue(ProfileDataId.AddressLine1, submitModel.AddressLine1?.Trim()!);
        sessionModel.SetProfileValue(ProfileDataId.AddressLine2, submitModel.AddressLine2?.Trim()!);
        sessionModel.SetProfileValue(ProfileDataId.County, submitModel.County?.Trim()!);
        sessionModel.SetProfileValue(ProfileDataId.Town, submitModel.Town?.Trim()!);
        sessionModel.SetProfileValue(ProfileDataId.Postcode, submitModel.Postcode?.Trim()!);

        var apiResponse = await _outerApiClient.GetCoordinates(submitModel.Postcode!);
        if (apiResponse.ResponseMessage.StatusCode == System.Net.HttpStatusCode.OK)
        {
            var coordinates = apiResponse.GetContent();
            sessionModel.SetProfileValue(ProfileDataId.Longitude, coordinates.Longitude.ToString());
            sessionModel.SetProfileValue(ProfileDataId.Latitude, coordinates.Latitude.ToString());
        }
        else
        {
            sessionModel.ClearProfileValue(ProfileDataId.Longitude);
            sessionModel.ClearProfileValue(ProfileDataId.Latitude);
        }

        _sessionService.Set(sessionModel);

        return RedirectToRoute(sessionModel.HasSeenPreview ? RouteNames.Onboarding.CheckYourAnswers : RouteNames.Onboarding.CurrentJobTitle);
    }
}