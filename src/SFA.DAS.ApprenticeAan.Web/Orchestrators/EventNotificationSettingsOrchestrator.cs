using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Aan.SharedUi.OuterApi.Requests;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.ApprenticeAan.Web.Models.EventNotificationSettings;

namespace SFA.DAS.ApprenticeAan.Web.Orchestrators;

public interface IEventNotificationSettingsOrchestrator
{
    Task<NotificationSettingsSessionModel> GetSettingsAsSessionModel(Guid memberId, CancellationToken cancellationToken);
    Task SaveSettings(Guid memberId, NotificationSettingsSessionModel settings);
    EventNotificationSettingsViewModel GetViewModel(Guid memberId, NotificationSettingsSessionModel sessionModel, IUrlHelper urlHelper, CancellationToken cancellationToken);
}


public class EventNotificationSettingsOrchestrator(IOuterApiClient outerApiClient)
    : IEventNotificationSettingsOrchestrator
{
    public async Task<NotificationSettingsSessionModel> GetSettingsAsSessionModel(Guid memberId, CancellationToken cancellationToken)
    {
        var apiResponse = await outerApiClient.GetMemberNotificationSettings(memberId, cancellationToken);

        var sessionModel = new NotificationSettingsSessionModel
        {
            ReceiveNotifications = apiResponse.ReceiveMonthlyNotifications,
            NotificationLocations = apiResponse.MemberNotificationLocations.Select(x => new NotificationLocation
            {
                LocationName = x.Name,
                GeoPoint = [x.Latitude, x.Longitude],
                Radius = x.Radius
            }).ToList(),
            EventTypes = apiResponse.MemberNotificationEventFormats.Where(x => x.ReceiveNotifications)
                .Select(x => new EventTypeModel
                {
                    EventType = x.EventFormat,
                    IsSelected = x.ReceiveNotifications,
                    Ordering = x.Ordering
                }).ToList()
        };

        return sessionModel;
    }

    public async Task SaveSettings(Guid memberId, NotificationSettingsSessionModel sessionModel)
    {
        var apiRequest = new NotificationsSettingsApiRequest
        {
            ReceiveNotifications = sessionModel.ReceiveNotifications ?? false,
            EventTypes = sessionModel.EventTypes!.Select(ev => new NotificationsSettingsApiRequest.NotificationEventType
            {
                EventType = ev.EventType,
                Ordering = ev.Ordering,
                ReceiveNotifications = ev.IsSelected
            }).ToList(),
            Locations = sessionModel.NotificationLocations.Select(x => new NotificationsSettingsApiRequest.Location
            {
                Name = x.LocationName,
                Radius = x.Radius,
                Latitude = x.GeoPoint[0],
                Longitude = x.GeoPoint[1]
            }).ToList()
        };

        await outerApiClient.PostMemberNotificationSettings(memberId, apiRequest);
    }

    public EventNotificationSettingsViewModel GetViewModel(Guid memberId, NotificationSettingsSessionModel sessionModel, IUrlHelper urlHelper, CancellationToken cancellationToken)
    {
        var eventFormats = new List<EventFormatViewModel>();
        var locations = new List<NotificationLocationsViewModel>();

        if (sessionModel.EventTypes.Any())
        {
            foreach (var format in sessionModel.EventTypes)
            {
                if (!format.EventType.Equals("All"))
                {
                    var eventFormatVm = new EventFormatViewModel
                    {
                        EventFormat = format.EventType,
                        DisplayName = GetEventTypeText(format.EventType),
                        Ordering = format.Ordering,
                        ReceiveNotifications = format.IsSelected
                    };

                    eventFormats.Add(eventFormatVm);
                }
            }
        }

        if (sessionModel.NotificationLocations.Any())
        {
            foreach (var location in sessionModel.NotificationLocations)
            {
                var locationVm = new NotificationLocationsViewModel
                {
                    LocationDisplayName = GetRadiusText(location.Radius, location.LocationName),
                    Radius = location.Radius,
                    Latitude = location.GeoPoint[0],
                    Longitude = location.GeoPoint[1]
                };

                locations.Add(locationVm);
            }
        }


        return new EventNotificationSettingsViewModel
        {
            EventFormats = eventFormats,
            EventNotificationLocations = locations,
            ReceiveMonthlyNotifications = sessionModel.ReceiveNotifications,
            ReceiveMonthlyNotificationsText = sessionModel.ReceiveNotifications == true ? "Yes" : "No",
            UserNewToNotifications = sessionModel.UserNewToNotifications,
            ChangeMonthlyEmailUrl = urlHelper.RouteUrl(RouteNames.EventNotificationSettings.MonthlyNotifications),
            ChangeEventTypeUrl = urlHelper.RouteUrl(RouteNames.EventNotificationSettings.EventTypes),
            ChangeLocationsUrl = urlHelper.RouteUrl(RouteNames.EventNotificationSettings.NotificationLocations),
            BackLink = urlHelper.RouteUrl(RouteNames.NetworkHub)!,
            ShowLocationsSection = !(sessionModel.SelectedEventTypes.Count == 1 && sessionModel.SelectedEventTypes.First().EventType == "Online")
        };
    }

    private string GetRadiusText(int radius, string location)
    {
        return radius == 0 ?
        $"{location}, Across England"
        : $"{location}, within {radius} miles";
    }

    private string GetEventTypeText(string eventFormat)
    {
        return (eventFormat.Equals("InPerson")) ? "In-person events" : $"{eventFormat} events";
    }
}
