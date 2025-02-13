using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Extensions;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Orchestrators;
namespace SFA.DAS.ApprenticeAan.Web.Controllers.EventNotificationSettings;

[Authorize]
[Route("event-notification-settings", Name = RouteNames.EventNotificationSettings.Settings)]
public class EventNotificationSettingsController(
    IEventNotificationSettingsOrchestrator orchestrator,
    ISessionService sessionService)
    : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var memberId = sessionService.GetMemberId();

        var sessionModel = await orchestrator.GetSettingsAsSessionModel(memberId, cancellationToken);
        sessionService.Set(sessionModel);

        var vm = orchestrator.GetViewModel(memberId, sessionModel, Url, cancellationToken);

        sessionModel.LastPageVisited = RouteNames.EventNotificationSettings.Settings;
        sessionService.Set(sessionModel);

        return View(vm);
    }
}