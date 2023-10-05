namespace SFA.DAS.Aan.SharedUi.Models;
public class MemberProfileMappingModel
{
    public int LinkedinProfileId { get; set; }
    public int JobTitleProfileId { get; set; }
    public int BiographyProfileId { get; set; }
    public List<int> FirstSectionProfileIds { get; set; }
    public List<int> SecondSectionProfileIds { get; set; }
    public List<int> AddressProfileIds { get; set; }
    public bool IsLoggedInUserMemberProfile { get; set; }

    public MemberProfileMappingModel(int sourceLinkedinProfileId, int sourceJobTitleProfileId, int sourceBiographyProfileId, List<int> sourceEventsProfileIds, List<int> sourcePromotionsProfileIds, List<int> sourceAddressProfileIds, bool sourceIsLoggedInUserMemberProfile)
    {
        LinkedinProfileId = sourceLinkedinProfileId;
        JobTitleProfileId = sourceJobTitleProfileId;
        BiographyProfileId = sourceBiographyProfileId;
        FirstSectionProfileIds = sourceEventsProfileIds;
        SecondSectionProfileIds = sourcePromotionsProfileIds;
        AddressProfileIds = sourceAddressProfileIds;
        IsLoggedInUserMemberProfile = sourceIsLoggedInUserMemberProfile;
    }

}
