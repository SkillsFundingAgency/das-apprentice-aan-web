using FluentValidation;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses.Shared;
using SFA.DAS.ApprenticeAan.Web.Constant;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;
using SFA.DAS.ApprenticeAan.Web.Models.Shared;

namespace SFA.DAS.ApprenticeAan.Web.Orchestrators.Shared
{
    public interface INotificationsLocationsOrchestrator
    {
        INotificationsLocationsPartialViewModel GetViewModel<T>(INotificationLocationsSessionModel sessionModel, ModelStateDictionary modelState) where T : INotificationsLocationsPartialViewModel, new();

        Task<NotificationsLocationsOrchestrator.RedirectTarget> ApplySubmitModel<T>(
            INotificationsLocationsPartialSubmitModel submitModel,
            ModelStateDictionary modelState,
            Func<string, Task<GetNotificationsLocationSearchApiResponse>> getNotificationsLocations)
            where T : INotificationLocationsSessionModel;
    }

    public class NotificationsLocationsOrchestrator(ISessionService sessionService, IValidator<INotificationsLocationsPartialSubmitModel> validator, IOuterApiClient apiClient)
        : INotificationsLocationsOrchestrator
    {
        public INotificationsLocationsPartialViewModel GetViewModel<T>(INotificationLocationsSessionModel sessionModel, ModelStateDictionary modelState) where T : INotificationsLocationsPartialViewModel, new()
        {
            var result = new T();
            var eventTypeDescription = GetEventTypeDescription(sessionModel.EventTypes);

            result.Title = sessionModel.NotificationLocations.Any()
                ? $"Notifications for {eventTypeDescription}"
                : $"Add locations for {eventTypeDescription}";
            result.IntroText = $"Tell us where you want to hear about upcoming {eventTypeDescription}.";

            result.SubmittedLocations = sessionModel.NotificationLocations
                .Select(l => l.Radius == 0 ?
                    $"{l.LocationName}, Across England"
                    : $"{l.LocationName}, within {l.Radius} miles").ToList();

            result.HasSubmittedLocations = sessionModel.NotificationLocations.Any();

            if (modelState.ContainsKey(nameof(NotificationsLocationsViewModel.Location)) &&
                modelState[nameof(NotificationsLocationsViewModel.Location)].Errors.Any())
            {
                result.UnrecognisedLocation =
                    modelState[nameof(NotificationsLocationsViewModel.Location)].AttemptedValue;
            }

            return result;
        }

        public async Task<RedirectTarget> ApplySubmitModel<T>(
    INotificationsLocationsPartialSubmitModel submitModel,
    ModelStateDictionary modelState,
    Func<string, Task<GetNotificationsLocationSearchApiResponse>> getNotificationsLocations) where T : INotificationLocationsSessionModel
        {
            var sessionModel = sessionService.Get<T>();

            if (submitModel.SubmitButton == NotificationsLocationsSubmitButtonOption.Continue)
            {
                if (string.IsNullOrWhiteSpace(submitModel.Location) && sessionModel.NotificationLocations.Any())
                {
                    return RedirectTarget.NextPage;
                }
            }

            if (submitModel.SubmitButton.StartsWith(NotificationsLocationsSubmitButtonOption.Delete))
            {
                var deleteIndex = Convert.ToInt32(submitModel.SubmitButton.Split("-").Last());
                sessionModel.NotificationLocations.RemoveAt(deleteIndex);
                sessionService.Set(sessionModel);

                return RedirectTarget.Self;
            }

            var validationResult = await validator.ValidateAsync(submitModel);
            if (!validationResult.IsValid)
            {
                foreach (var e in validationResult.Errors)
                {
                    modelState.AddModelError(e.PropertyName, e.ErrorMessage);
                }

                return RedirectTarget.Self;
            }

            var apiResponse = await getNotificationsLocations(submitModel.Location);

            if (apiResponse.Locations.Count > 1)
            {
                return RedirectTarget.Disambiguation;
            }

            if (apiResponse.Locations.Count == 0)
            {
                modelState.AddModelError("Location", "We cannot find the location you entered");
                return RedirectTarget.Self;
            }

            sessionModel.NotificationLocations.Add(new NotificationLocation
            {
                LocationName = apiResponse.Locations.First().Name,
                GeoPoint = apiResponse.Locations.First().Coordinates,
                Radius = submitModel.Radius
            });

            sessionService.Set(sessionModel);

            return submitModel.SubmitButton == NotificationsLocationsSubmitButtonOption.Continue ? RedirectTarget.NextPage : RedirectTarget.Self;
        }

        private string GetEventTypeDescription(IEnumerable<EventTypeModel> eventTypes)
        {
            var selectedEventTypes = eventTypes.Where(x => x.IsSelected).ToList();

            if (selectedEventTypes.Any(t => t.EventType == EventType.All))
            {
                return "in-person and hybrid events";
            }

            if (selectedEventTypes.Any(t => t.EventType == EventType.Hybrid))
            {
                return selectedEventTypes.Any(e => e.EventType == EventType.InPerson) ?
                    "in-person and hybrid events" : "hybrid events";
            }

            if (selectedEventTypes.Any(e => e.EventType == EventType.InPerson))
            {
                return "in-person events";
            }

            throw new InvalidOperationException();
        }

        public enum RedirectTarget
        {
            Self,
            NextPage,
            Disambiguation
        }
    }
}
