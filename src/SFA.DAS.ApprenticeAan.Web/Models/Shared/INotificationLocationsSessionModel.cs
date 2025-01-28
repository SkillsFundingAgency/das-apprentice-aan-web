namespace SFA.DAS.ApprenticeAan.Web.Models.Shared;

public interface INotificationLocationsSessionModel
{
    List<NotificationLocation> NotificationLocations { get; }
    List<EventTypeModel>? EventTypes { get; }
}