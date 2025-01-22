using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Constant;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;
using SFA.DAS.ApprenticePortal.SharedUi.Menu;

namespace SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;

[Authorize]
[Route("onboarding/select-notifications", Name = RouteNames.Onboarding.SelectNotificationEvents)]
[HideNavigationBar(true, true)]
public class SelectNotificationsController : Controller
{
    public const string ViewPath = "~/Views/Onboarding/SelectNotifications.cshtml";
    private readonly ISessionService _sessionService;
    private readonly IValidator<SelectNotificationsSubmitModel> _validator;

    public SelectNotificationsController(
        ISessionService sessionService,
        IValidator<SelectNotificationsSubmitModel> validator)
    {
        _sessionService = sessionService;
        _validator = validator;
    }

    [HttpGet]
    public IActionResult Get()
    {
        var sessionModel = _sessionService.Get<OnboardingSessionModel>();
        var model = GetViewModel(sessionModel);
        return View(ViewPath, model);
    }

    [HttpPost]
    public IActionResult Post(SelectNotificationsSubmitModel submitModel, CancellationToken cancellationToken)
    {
        var result = _validator.Validate(submitModel);
        var sessionModel = _sessionService.Get<OnboardingSessionModel>();

        if (!result.IsValid)
        {
            var model = GetViewModel(sessionModel);
            result.AddToModelState(ModelState);
            return View(ViewPath, model);
        }

        //For Javascript disabled browser.Deselect other event types if 'All' is selected

        if (submitModel.EventTypes.Any(e => e.EventType == EventType.All && e.IsSelected))
        {
            submitModel.EventTypes.ForEach(e => e.IsSelected = e.EventType == EventType.All);
        }

        if (submitModel.EventTypes.Count(x => x.IsSelected) == 1 &&
            submitModel.EventTypes.Any(x => x.IsSelected && x.EventType == EventType.Online))
        {
            sessionModel.NotificationLocations = new List<NotificationLocation>();
        }

        var originalValue = sessionModel.EventTypes;
        var newValue = submitModel.EventTypes;

        sessionModel.EventTypes = submitModel.EventTypes;

        _sessionService.Set(sessionModel);

        var areEventTypesMatching = sessionModel.HasSeenPreview &&
                                    AreEventTypesMatching(originalValue, newValue);

        return RedirectAccordingly(areEventTypesMatching, sessionModel.EventTypes, sessionModel.HasSeenPreview);
    }

    private IActionResult RedirectAccordingly(
        bool areEventTypesMatching,
        List<EventTypeModel> eventTypes,
        bool hasSeenPreview)
    {
        if (areEventTypesMatching)
            return RedirectToRoute(RouteNames.Onboarding.CheckYourAnswers);

        if (hasSeenPreview && eventTypes.All(e => e.EventType == EventType.Online || !e.IsSelected))
            return RedirectToRoute(RouteNames.Onboarding.CheckYourAnswers);

        if (eventTypes.Any(e => e.IsSelected &&
                                (e.EventType == EventType.Hybrid ||
                                 e.EventType == EventType.InPerson ||
                                 e.EventType == EventType.All)))
        {
            return RedirectToRoute(RouteNames.Onboarding.NotificationsLocations);
        }

        return RedirectToRoute(RouteNames.Onboarding.PreviousEngagement);
    }

    private SelectNotificationsViewModel GetViewModel(OnboardingSessionModel sessionModel)
    {
        if (sessionModel.EventTypes == null || !sessionModel.EventTypes.Any())
        {
            sessionModel.EventTypes = InitializeDefaultEventTypes();
        }

        return new SelectNotificationsViewModel
        {
            BackLink = sessionModel.HasSeenPreview
                ? Url.RouteUrl(@RouteNames.Onboarding.CheckYourAnswers)!
                : Url.RouteUrl(@RouteNames.Onboarding.ReceiveNotifications)!,
            EventTypes = sessionModel.EventTypes.OrderBy(e => e.Ordering).ToList()
        };
    }

    private List<EventTypeModel> InitializeDefaultEventTypes() => new()
    {
        new EventTypeModel { EventType = EventType.InPerson, IsSelected = false, Ordering = 1 },
        new EventTypeModel { EventType = EventType.Hybrid, IsSelected = false, Ordering = 3 },
        new EventTypeModel { EventType = EventType.Online, IsSelected = false, Ordering = 2 },
        new EventTypeModel { EventType = EventType.All, IsSelected = false, Ordering = 4 }
    };

    private bool AreEventTypesMatching(List<EventTypeModel>? originalValue, List<EventTypeModel>? newValue)
    {
        if (originalValue == null && newValue == null)
            return true;

        if (originalValue == null || newValue == null)
            return false;

        if (originalValue.Count != newValue.Count)
            return false;

        return originalValue.OrderBy(e => e.Ordering).SequenceEqual(
            newValue.OrderBy(e => e.Ordering),
            new EventTypeModelComparer()
        );
    }
}
