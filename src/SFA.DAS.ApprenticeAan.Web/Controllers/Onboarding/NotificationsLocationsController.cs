using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;
using SFA.DAS.ApprenticeAan.Web.Orchestrators.Shared;
using SFA.DAS.ApprenticePortal.SharedUi.Menu;
using SFA.DAS.Validation.Mvc.Filters;

namespace SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;

[Authorize]
[Route("onboarding/notifications-locations", Name = RouteNames.Onboarding.NotificationsLocations)]
[HideNavigationBar(true, true)]
public class NotificationsLocationsController : Controller
{
    public const string ViewPath = "~/Views/Onboarding/NotificationsLocations.cshtml";
    private readonly ISessionService _sessionService;
    private readonly INotificationsLocationsOrchestrator _orchestrator;
    private readonly IOuterApiClient _apiClient;

    public NotificationsLocationsController(ISessionService sessionService,
        INotificationsLocationsOrchestrator orchestrator,
        IOuterApiClient apiClient)
    {
        _sessionService = sessionService;
        _orchestrator = orchestrator;
        _apiClient = apiClient;
    }

    [HttpGet]
    [ValidateModelStateFilter]
    public IActionResult Get()
    {
        var sessionModel = _sessionService.Get<OnboardingSessionModel>();

        if (TempData.ContainsKey("SameLocationError"))
        {
            var errorMessage = TempData["SameLocationError"] as string;
            if (!string.IsNullOrEmpty(errorMessage))
            {
                ModelState.AddModelError(nameof(NotificationsLocationsViewModel.Location), errorMessage);
            }
        }

        var viewModel = _orchestrator.GetViewModel<NotificationsLocationsViewModel>(sessionModel, ModelState);

        viewModel.BackLink = sessionModel.HasSeenPreview
            ? Url.RouteUrl(RouteNames.Onboarding.CheckYourAnswers)!
            : Url.RouteUrl(RouteNames.Onboarding.SelectNotificationEvents)!;

        return View(ViewPath, viewModel);
    }

    [HttpPost]
    [ValidateModelStateFilter]
    public async Task<IActionResult> Post(NotificationsLocationsSubmitModel submitModel)
    {
        var result = await _orchestrator.ApplySubmitModel<OnboardingSessionModel>(
            submitModel,
            ModelState,
            async (location) => await _apiClient.GetOnboardingNotificationsLocations(location)
        );

        var sessionModel = _sessionService.Get<OnboardingSessionModel>();

        if (result == NotificationsLocationsOrchestrator.RedirectTarget.NextPage)
        {
            var routeName = sessionModel.HasSeenPreview
                ? RouteNames.Onboarding.CheckYourAnswers
                : RouteNames.Onboarding.PreviousEngagement;

            return RedirectToRoute(routeName);
        }

        return result switch
        {
            NotificationsLocationsOrchestrator.RedirectTarget.Disambiguation
                => RedirectToRoute(RouteNames.Onboarding.NotificationLocationDisambiguation,
                new { submitModel.Radius, submitModel.Location }),
            NotificationsLocationsOrchestrator.RedirectTarget.Self => RedirectToRoute(RouteNames.Onboarding.NotificationsLocations),
            _ => throw new InvalidOperationException("Unexpected redirect target from ApplySubmitModel"),
        };
    }
}
