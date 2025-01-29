using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models.Shared;

namespace SFA.DAS.ApprenticeAan.Web.Models.EventNotificationSettings;

public class NotificationSettingsSessionModel : INotificationLocationsSessionModel
{
    public List<NotificationLocation> NotificationLocations { get; set; } = [];
    public List<EventTypeModel> EventTypes { get; set; } = [];
    public bool UserNewToNotifications { get; set; }
    public bool? ReceiveNotifications { get; set; }
    public string LastPageVisited { get; set; } = RouteNames.EventNotificationSettings.Settings;
    public List<EventTypeModel> SelectedEventTypes => EventTypes.Where(x => x.IsSelected).ToList();
}
