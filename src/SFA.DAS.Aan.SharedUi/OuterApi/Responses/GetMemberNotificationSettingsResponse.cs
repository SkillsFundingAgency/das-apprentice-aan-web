namespace SFA.DAS.Aan.SharedUi.OuterApi.Responses;

public class GetMemberNotificationSettingsResponse
{
    public IEnumerable<MemberNotificationEventFormatModel> MemberNotificationEventFormats { get; set; } = Enumerable.Empty<MemberNotificationEventFormatModel>();
    public IEnumerable<MemberNotificationLocationsModel> MemberNotificationLocations { get; set; } = Enumerable.Empty<MemberNotificationLocationsModel>();
    public bool? ReceiveMonthlyNotifications { get; set; }
    public bool UserNewToNotifications => !ReceiveMonthlyNotifications.HasValue;
}

public class MemberNotificationEventFormatModel
{
    public string EventFormat { get; set; } = string.Empty;
    public int Ordering { get; set; }
    public bool ReceiveNotifications { get; set; }
}

public class MemberNotificationLocationsModel
{
    public string Name { get; set; } = string.Empty;
    public int Radius { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}