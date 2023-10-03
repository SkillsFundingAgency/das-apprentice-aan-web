using SFA.DAS.Aan.SharedUi.Constants;
using SFA.DAS.Aan.SharedUi.OuterApi.Responses;

namespace SFA.DAS.Aan.SharedUi.Models;
public class MemberProfileViewModel
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? OrganisationName { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public int? RegionId { get; set; }
    public string RegionName { get; set; }
    public Role UserRole { get; set; }
    public string JobTitle { get; set; }
    public string Biography { get; set; }
    public string LinkedinUrl { get; set; }
    public List<string> EventProfiles { get; set; }
    public List<string> PromotionProfiles { get; set; }
    public string Address { get; set; }
    public Apprenticeship? ApprenticeShip { get; set; }
    public bool IsLoggedInUserMemberProfile { get; set; }
    public MemberProfileViewModel(MemberProfile source, MemberProfileMappingModel memberProfileMappingModel)
    {
        FullName = source.FullName;
        Email = source.Email;
        RegionId = source.RegionId;
        RegionName = (source.RegionId != null) ? source.RegionName : "Multi-regional";
        UserRole = (source.IsRegionalChair ?? false) ? Role.RegionalChair : source.UserType;
        JobTitle = GetValueOrDefault(source.Profiles.FirstOrDefault(x => x.ProfileId == memberProfileMappingModel.JobTitleProfileId));
        Biography = GetValueOrDefault(source.Profiles.FirstOrDefault(x => x.ProfileId == memberProfileMappingModel.BiographyProfileId));
        LinkedinUrl = GetValueOrDefault(source.Profiles.FirstOrDefault(x => x.ProfileId == memberProfileMappingModel.LinkedinProfileId));
        EventProfiles = source.Profiles.Where(x => memberProfileMappingModel.EventsProfileIds.Contains(x.ProfileId)).Select(x => x.Value).ToList();
        PromotionProfiles = source.Profiles.Where(x => memberProfileMappingModel.PromotionsProfileIds.Contains(x.ProfileId)).Select(x => x.Value).ToList();
        Address = string.Join(",", source.Profiles.Where(x => memberProfileMappingModel.AddressProfileIds.Contains(x.ProfileId)).Select(x => x.Value).ToList());
        IsLoggedInUserMemberProfile = memberProfileMappingModel.IsLoggedInUserMemberProfile;
        ApprenticeShip = source.Apprenticeship;
        OrganisationName = source.OrganisationName;
        FirstName = source.FirstName;
        LastName = source.LastName;
    }

    public static string GetValueOrDefault(Profile? profile)
    {
        return profile != null ? profile.Value : string.Empty;
    }
}
