using System.Net;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Requests;
using SFA.DAS.ApprenticeAan.Web.Configuration;
using SFA.DAS.ApprenticeAan.Web.Extensions;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;

namespace SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;

[Authorize]
[Route("onboarding/line-manager", Name = RouteNames.Onboarding.LineManager)]
public class LineManagerController : Controller
{
    public const string ViewPath = "~/Views/Onboarding/LineManager.cshtml";
    public const string ShutterPageViewPath = "~/Views/Onboarding/ShutterPage.cshtml";
    private readonly ISessionService _sessionService;
    private readonly IOuterApiClient _apiClient;
    private readonly IValidator<LineManagerSubmitModel> _validator;
    private readonly ApplicationConfiguration _appplicationConfiguration;

    public LineManagerController(
        ISessionService sessionService,
        IOuterApiClient apiClient,
        IValidator<LineManagerSubmitModel> validator,
        ApplicationConfiguration appplicationConfiguration)
    {
        _apiClient = apiClient;
        _validator = validator;
        _sessionService = sessionService;
        _appplicationConfiguration = appplicationConfiguration;
    }

    [HttpGet]
    public IActionResult Get()
    {
        if (!TempData.ContainsKey(TempDataKeys.HasSeenTermsAndConditions))
        {
            return RedirectToRoute(RouteNames.Onboarding.BeforeYouStart);
        }
        return View(ViewPath, GetViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Post(LineManagerSubmitModel submitmodel, CancellationToken cancellationToken)
    {
        if (!TempData.ContainsKey(TempDataKeys.HasSeenTermsAndConditions))
        {
            return RedirectToRoute(RouteNames.Onboarding.BeforeYouStart);
        }

        ValidationResult result = _validator.Validate(submitmodel);
        if (!result.IsValid)
        {
            result.AddToModelState(this.ModelState);
            return View(ViewPath, GetViewModel());
        }

        if (!submitmodel.HasEmployersApproval.GetValueOrDefault())
        {
            ShutterPageViewModel shutterPageViewModel = new() { ApprenticeHomeUrl = _appplicationConfiguration.ApplicationUrls.ApprenticeHomeUrl.ToString() };
            _sessionService.Delete<OnboardingSessionModel>();
            TempData.Remove(TempDataKeys.HasSeenTermsAndConditions);
            return View(ShutterPageViewPath, shutterPageViewModel);
        }

        if (!_sessionService.Contains<OnboardingSessionModel>())
        {
            var profilesTask = _apiClient.GetProfilesByUserType("apprentice", cancellationToken);
            var accountTask = _apiClient.GetApprenticeAccount(User.GetApprenticeId(), cancellationToken);
            await Task.WhenAll(profilesTask, accountTask);

            var profiles = profilesTask.Result;
            TryCreateMyApprenticeshipRequest apprentice = accountTask.Result.GetContent()!;

            var myApprenticeship = await _apiClient.TryCreateMyApprenticeship(apprentice, cancellationToken);
            if (myApprenticeship.ResponseMessage.StatusCode == HttpStatusCode.NotFound)
            {
                return Redirect(@"/accessdenied");
            }

            OnboardingSessionModel sessionModel = new()
            {
                ProfileData = profiles.Profiles.Select(p => (ProfileModel)p).ToList(),
                HasAcceptedTerms = true,
                MyApprenticeship = myApprenticeship.GetContent()!
            };
            sessionModel.ApprenticeDetails.ApprenticeId = User.GetApprenticeId();
            sessionModel.ApprenticeDetails.Name = $"{apprentice.FirstName} {apprentice.LastName}";
            sessionModel.ApprenticeDetails.Email = apprentice.Email;
            _sessionService.Set(sessionModel);
        }

        return RedirectToRoute(RouteNames.Onboarding.EmployerSearch);
    }

    private LineManagerViewModel GetViewModel()
    {
        return new LineManagerViewModel()
        {
            BackLink = Url.RouteUrl(@RouteNames.Onboarding.TermsAndConditions)!,
            HasEmployersApproval = _sessionService.Get<OnboardingSessionModel>()?.HasAcceptedTerms
        };
    }
}