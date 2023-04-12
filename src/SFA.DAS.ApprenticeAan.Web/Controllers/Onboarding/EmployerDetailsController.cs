using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Domain.Constants;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Filters;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;

namespace SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;

[Authorize]
[Route("onboarding/employer-details", Name = RouteNames.Onboarding.EmployerDetails)]
[RequiredSessionModel(typeof(OnboardingSessionModel))]
public class EmployerDetailsController : Controller
{
    public const string ViewPath = "~/Views/Onboarding/EmployerDetails.cshtml";

    private readonly ISessionService _sessionService;
    private readonly IValidator<EmployerDetailsSubmitModel> _validator;

    public EmployerDetailsController(ISessionService sessionService,
        IValidator<EmployerDetailsSubmitModel> validator)
    {
        _validator = validator;
        _sessionService = sessionService;
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

            BackLink = Url.RouteUrl(@RouteNames.Onboarding.LineManager)!
        };
        return View(ViewPath, model);
    }

    [HttpPost]
    public IActionResult PostEmployerDetails(EmployerDetailsSubmitModel submitmodel)
    {
        var model = new EmployerDetailsViewModel()
        {
            BackLink = Url.RouteUrl(RouteNames.Onboarding.LineManager)!
        };

        FluentValidation.Results.ValidationResult result = _validator.Validate(submitmodel);
        if (!result.IsValid)
        {
            result.AddToModelState(this.ModelState);
            return View(ViewPath, model);
        }

        var sessionModel = _sessionService.Get<OnboardingSessionModel>();

        sessionModel.SetProfileValue(ProfileDataId.EmployerName, submitmodel.EmployerName!);
        sessionModel.SetProfileValue(ProfileDataId.AddressLine1, submitmodel.AddressLine1!);
        sessionModel.SetProfileValue(ProfileDataId.AddressLine2, submitmodel.AddressLine2!);
        sessionModel.SetProfileValue(ProfileDataId.County, submitmodel.County!);
        sessionModel.SetProfileValue(ProfileDataId.Town, submitmodel.Town!);
        sessionModel.SetProfileValue(ProfileDataId.Postcode, submitmodel.Postcode!);

        _sessionService.Set(sessionModel);

        return RedirectToRoute(RouteNames.Onboarding.CurrentJobTitle);
    }
}