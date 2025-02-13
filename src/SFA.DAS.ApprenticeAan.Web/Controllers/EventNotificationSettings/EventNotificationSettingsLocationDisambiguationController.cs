using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Constant;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models.EventNotificationSettings;
using SFA.DAS.ApprenticeAan.Web.Orchestrators.Shared;
using SFA.DAS.Validation.Mvc.Filters;

namespace SFA.DAS.ApprenticeAan.Web.Controllers.EventNotificationSettings
{
    [Authorize]
    [Route("event-notification-settings/locations/disambiguation", Name = RouteNames.EventNotificationSettings.SettingsNotificationLocationDisambiguation)]
    public class EventNotificationSettingsLocationDisambiguationController(
        ISessionService sessionService,
        INotificationLocationDisambiguationOrchestrator orchestrator)
        : Controller
    {
        public const string ViewPath = "~/Views/EventNotificationSettings/NotificationLocationDisambiguation.cshtml";

        [HttpGet]
        [ValidateModelStateFilter]
        public async Task<IActionResult> Get(int radius, string location)
        {
            var model = await orchestrator.GetViewModel<NotificationLocationDisambiguationViewModel>(radius, location);

            model.BackLink = Url.RouteUrl(@RouteNames.EventNotificationSettings.NotificationLocations)!;

            return View(ViewPath, model);
        }

        [HttpPost]
        [ValidateModelStateFilter]
        public async Task<IActionResult> Post(NotificationLocationDisambiguationSubmitModel submitModel, CancellationToken cancellationToken)
        {

            var sessionModel = sessionService.Get<NotificationSettingsSessionModel>();

            var routeValues = new { submitModel.Radius, submitModel.Location };

            if ((submitModel.SelectedLocation != null) && sessionModel.NotificationLocations.Any(n => n.LocationName.Equals(submitModel.SelectedLocation, StringComparison.OrdinalIgnoreCase)))
            {
                TempData["SameLocationError"] = ErrorMessages.SameLocationErrorMessage;
                return RedirectToRoute(RouteNames.EventNotificationSettings.NotificationLocations);
            }

            var result = await orchestrator.ApplySubmitModel<NotificationSettingsSessionModel>(submitModel, ModelState);

            switch (result)
            {
                case NotificationLocationDisambiguationOrchestrator.RedirectTarget.NextPage:
                    return RedirectToRoute(RouteNames.EventNotificationSettings.NotificationLocations);
                case NotificationLocationDisambiguationOrchestrator.RedirectTarget.Self:
                    return RedirectToRoute(RouteNames.EventNotificationSettings.SettingsNotificationLocationDisambiguation, routeValues);
                default:
                    throw new InvalidOperationException("Unexpected redirect target from ApplySubmitModel");
            }
        }
    }
}
