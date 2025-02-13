namespace SFA.DAS.ApprenticeAan.Web.Models.EventNotificationSettings;

public class EventNotificationSettingsViewModel : IBackLink
{
    public List<EventFormatViewModel> EventFormats { get; set; }
    public List<NotificationLocationsViewModel> EventNotificationLocations { get; set; }
    public bool? ReceiveMonthlyNotifications { get; set; }
    public string ReceiveMonthlyNotificationsText { get; set; }
    public bool UserNewToNotifications { get; set; }
    public string? ChangeMonthlyEmailUrl { get; set; }
    public string? ChangeEventTypeUrl { get; set; }
    public string? ChangeLocationsUrl { get; set; }
    public string BackLink { get; set; }
    public bool ShowLocationsSection { get; set; }
}

public class EventFormatViewModel
{
    public string EventFormat { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public int Ordering { get; set; }
    public bool ReceiveNotifications { get; set; }
}

public class NotificationLocationsViewModel
{
    public string LocationDisplayName { get; set; } = string.Empty;
    public int Radius { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}