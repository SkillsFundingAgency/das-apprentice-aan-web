namespace SFA.DAS.Aan.SharedUi.Models;
public class ProfileSettingsViewModel : INetworkHubLink
{
    public string YourAmbassadorProfileUrl { get; set; } = "#";
    public string LeaveTheNetworkUrl { get; set; } = "#";
    public string UpcomingEventsNotificationsUrl { get; set; } = "#";
    public string? NetworkHubLink { get; set; }
}