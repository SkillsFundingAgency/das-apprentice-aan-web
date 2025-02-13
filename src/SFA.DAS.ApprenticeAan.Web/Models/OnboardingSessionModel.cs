using SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses;
using SFA.DAS.ApprenticeAan.Web.Models.Shared;

namespace SFA.DAS.ApprenticeAan.Web.Models;

public class OnboardingSessionModel : INotificationLocationsSessionModel
{
    public MyApprenticeship MyApprenticeship { get; set; } = null!;
    public bool HasSeenPreview { get; set; }
    public ApprenticeDetailsModel ApprenticeDetails { get; set; } = new();
    public bool HasAcceptedTerms { get; set; } = false;
    public List<ProfileModel> ProfileData { get; set; } = [];
    public List<EventTypeModel>? EventTypes { get; set; }
    public int? RegionId { get; set; }
    public string? RegionName { get; set; }
    public bool? ReceiveNotifications { get; set; }
    public bool IsValid => HasAcceptedTerms && ProfileData.Count > 0;
    public List<NotificationLocation> NotificationLocations { get; set; } = [];

    public string? GetProfileValue(int id) => ProfileData.Single(p => p.Id == id)?.Value;
    public void SetProfileValue(int id, string value) => ProfileData.Single(p => p.Id == id).Value = value;
    public void ClearProfileValue(int id) => ProfileData.Single(p => p.Id == id).Value = null;
}

public class ApprenticeDetailsModel
{
    public Guid ApprenticeId { get; set; }
    public string Email { get; set; } = null!;
    public string Name { get; set; } = null!;
}
//public class NotificationLocation
//{
//    public string LocationName { get; set; }
//    public double[] GeoPoint { get; set; }
//    public int Radius { get; set; }
//}