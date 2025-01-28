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
[Route("onboarding/notification-disambiguation", Name = RouteNames.Onboarding.NotificationLocationDisambiguation)]
[HideNavigationBar(true, true)]
public class NotificationLocationDisambiguationController : Controller
{
    public const string ViewPath = "~/Views/Onboarding/NotificationLocationDisambiguation.cshtml";
    private readonly ISessionService _sessionService;
    private readonly INotificationLocationDisambiguationOrchestrator _orchestrator;

    public NotificationLocationDisambiguationController(
        ISessionService sessionService,
        INotificationLocationDisambiguationOrchestrator orchestrator)
    {
        _sessionService = sessionService;
        _orchestrator = orchestrator;
    }

    [HttpGet]
    [ValidateModelStateFilter]
    public async Task<IActionResult> Get(int radius, string location)
    {
        var model = await _orchestrator.GetViewModel<NotificationLocationDisambiguationViewModel>(radius, location);

        model.BackLink = Url.RouteUrl(@RouteNames.Onboarding.NotificationsLocations)!;

        return View(ViewPath, model);
    }

    [HttpPost]
    [ValidateModelStateFilter]
    public async Task<IActionResult> Post(NotificationLocationDisambiguationSubmitModel submitModel, CancellationToken cancellationToken)
    {
        var result = await _orchestrator.ApplySubmitModel<OnboardingSessionModel>(submitModel, ModelState);

        var routeValues = new { submitModel.Radius, submitModel.Location };

        switch (result)
        {
            case NotificationLocationDisambiguationOrchestrator.RedirectTarget.NextPage:
                return RedirectToRoute(RouteNames.Onboarding.NotificationsLocations);
            case NotificationLocationDisambiguationOrchestrator.RedirectTarget.Self:
                return RedirectToRoute(RouteNames.Onboarding.NotificationLocationDisambiguation, routeValues);
            default:
                throw new InvalidOperationException("Unexpected redirect target from ApplySubmitModel");
        }
    }
}
