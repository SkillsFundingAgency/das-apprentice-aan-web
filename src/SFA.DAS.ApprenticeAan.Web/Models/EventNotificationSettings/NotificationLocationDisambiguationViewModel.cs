using SFA.DAS.ApprenticeAan.Web.Models.Shared;

namespace SFA.DAS.ApprenticeAan.Web.Models.EventNotificationSettings;

public class NotificationLocationDisambiguationViewModel : NotificationLocationDisambiguationSubmitModel, INotificationLocationDisambiguationPartialViewModel
{
    public string BackLink { get; set; } = null!;
    public string Title { get; set; } = "Select Notifications Location";
    public List<LocationModel>? Locations { get; set; }
}

public class NotificationLocationDisambiguationSubmitModel : INotificationLocationDisambiguationPartialSubmitModel
{
    public string Location { get; set; } = string.Empty;
    public int Radius { get; set; }
    public string? SelectedLocation { get; set; }
}