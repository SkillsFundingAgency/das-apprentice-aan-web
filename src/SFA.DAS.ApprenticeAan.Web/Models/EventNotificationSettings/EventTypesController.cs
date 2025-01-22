using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Constant;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;
using SFA.DAS.ApprenticeAan.Web.Orchestrators;

namespace SFA.DAS.ApprenticeAan.Web.Models.EventNotificationSettings;

[Authorize]
[Route("event-notification-settings/event-formats", Name = RouteNames.EventNotificationSettings.EventTypes)]
public class EventTypesController : Controller
{
    public const string ViewPath = "~/Views/Onboarding/SelectNotifications.cshtml";
    private readonly ISessionService _sessionService;
    private readonly IEventNotificationSettingsOrchestrator _settingsOrchestrator;
    private readonly IValidator<SelectNotificationsSubmitModel> _validator;

    public EventTypesController(ISessionService sessionService, IOuterApiClient apiClient, IValidator<SelectNotificationsSubmitModel> validator, IEventNotificationSettingsOrchestrator settingsOrchestrator)
    {
        _sessionService = sessionService;
        _validator = validator;
        _settingsOrchestrator = settingsOrchestrator;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromRoute] string employerAccountId, CancellationToken cancellationToken)
    {
        var sessionModel = _sessionService.Get<NotificationSettingsSessionModel?>();

        if (sessionModel == null)
        {
            return RedirectToRoute(RouteNames.EventNotificationSettings.Settings);
        }

        var model = GetViewModel(sessionModel);
        return View(ViewPath, model);
    }


    private SelectNotificationsViewModel GetViewModel(NotificationSettingsSessionModel sessionModel)
    {
        var vm = new SelectNotificationsViewModel() { };

        vm.EventTypes = InitializeDefaultEventTypes();
        vm.BackLink = sessionModel.LastPageVisited == RouteNames.EventNotificationSettings.MonthlyNotifications ?
            Url.RouteUrl(@RouteNames.EventNotificationSettings.MonthlyNotifications) :
            Url.RouteUrl(@RouteNames.EventNotificationSettings.Settings);

        if (sessionModel.EventTypes.Count == 3)
        {
            var allOption = vm.EventTypes.Single(x => x.EventType == EventType.All);
            allOption.IsSelected = true;
        }
        else
        {
            foreach (var e in vm.EventTypes)
            {
                foreach (var ev in sessionModel.EventTypes!.Where(ev => ev.EventType.Equals(e.EventType)))
                {
                    e.IsSelected = true;
                }
            }
        }

        return vm;
    }

    private List<EventTypeModel> InitializeDefaultEventTypes() => new()
    {
        new EventTypeModel { EventType = EventType.InPerson, IsSelected = false, Ordering = 1 },
        new EventTypeModel { EventType = EventType.Online, IsSelected = false, Ordering = 2 },
        new EventTypeModel { EventType = EventType.Hybrid, IsSelected = false, Ordering = 3 },
        new EventTypeModel { EventType = EventType.All, IsSelected = false, Ordering = 4 }
    };

    private IActionResult RedirectAccordingly(List<EventTypeModel> newEvents)
    {
        if (newEvents.Count == 1 && newEvents.First().EventType == EventType.Online)
        {
            return RedirectToRoute(RouteNames.EventNotificationSettings.Settings);
        }

        return RedirectToRoute(RouteNames.EventNotificationSettings.NotificationLocations);
    }
}