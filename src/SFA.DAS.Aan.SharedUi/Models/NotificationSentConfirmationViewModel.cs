namespace SFA.DAS.Aan.SharedUi.Models;

public class NotificationSentConfirmationViewModel(string networkDirectoryUrl) : INetworkHubLink
{
    public string NetworkDirectoryUrl { get; set; } = networkDirectoryUrl;
    public string? NetworkHubLink { get; set; }
};
