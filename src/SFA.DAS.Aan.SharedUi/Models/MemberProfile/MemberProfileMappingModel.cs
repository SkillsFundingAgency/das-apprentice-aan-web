namespace SFA.DAS.Aan.SharedUi.Models;
public class MemberProfileMappingModel
{
    public int LinkedinProfileId { get; set; }
    public int JobTitleProfileId { get; set; }
    public int BiographyProfileId { get; set; }
    public List<int> EventsProfileIds { get; set; } = new List<int>();
    public List<int> PromotionsProfileIds { get; set; } = new List<int>();
    public List<int> AddressProfileIds { get; set; } = new List<int>();
    public bool IsLoggedInUserMemberProfile { get; set; } = false;

    public MemberProfileMappingModel(int sourceLinkedinProfileId, int sourceJobTitleProfileId, int sourceBiographyProfileId, List<int> sourceEventsProfileIds, List<int> sourcePromotionsProfileIds, List<int> sourceAddressProfileIds, bool sourceIsLoggedInUserMemberProfile)
    {
        LinkedinProfileId = sourceLinkedinProfileId;
        JobTitleProfileId = sourceJobTitleProfileId;
        BiographyProfileId = sourceBiographyProfileId;
        EventsProfileIds = sourceEventsProfileIds;
        PromotionsProfileIds = sourcePromotionsProfileIds;
        AddressProfileIds = sourceAddressProfileIds;
        IsLoggedInUserMemberProfile = sourceIsLoggedInUserMemberProfile;
    }

}
