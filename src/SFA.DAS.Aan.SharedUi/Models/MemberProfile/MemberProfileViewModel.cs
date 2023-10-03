using SFA.DAS.Aan.SharedUi.Constants;
using SFA.DAS.Aan.SharedUi.OuterApi.Responses;

namespace SFA.DAS.Aan.SharedUi.Models;
public class MemberProfileViewModel
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? OrganisationName { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int? RegionId { get; set; }
    public string RegionName { get; set; } = string.Empty;
    public Role UserRole { get; set; }
    public string JobTitle { get; set; } = string.Empty;
    public string Biography { get; set; } = string.Empty;
    public string LinkedinUrl { get; set; } = string.Empty;
    public List<string> EventProfiles { get; set; }
    public List<string> PromotionProfiles { get; set; }
    public string Address { get; set; } = string.Empty;
    public Apprenticeship? ApprenticeShip { get; set; }
    public bool IsLoggedInUserMemberProfile { get; set; } = false;
    public MemberProfileViewModel(MemberProfile source, MemberProfileMappingModel memberProfileMappingModel)
    {
        FullName = source.FullName;
        Email = source.Email;
        RegionId = source.RegionId;
        RegionName = (source.RegionId != null) ? source.RegionName : "Multi-regional";
        UserRole = (source.IsRegionalChair ?? false) ? Role.RegionalChair : source.UserType;
        JobTitle = GetValueOrDefault(source.Profiles.Where(x => x.ProfileId == memberProfileMappingModel.JobTitleProfileId).FirstOrDefault());
        Biography = GetValueOrDefault(source.Profiles.Where(x => x.ProfileId == memberProfileMappingModel.BiographyProfileId).FirstOrDefault());
        LinkedinUrl = GetValueOrDefault(source.Profiles.Where(x => x.ProfileId == memberProfileMappingModel.LinkedinProfileId).FirstOrDefault());
        EventProfiles = source.Profiles.Where(x => memberProfileMappingModel.EventsProfileIds.Contains(x.ProfileId)).Select(x => x.Value).ToList();
        PromotionProfiles = source.Profiles.Where(x => memberProfileMappingModel.PromotionsProfileIds.Contains(x.ProfileId)).Select(x => x.Value).ToList();
        Address = string.Join(",", source.Profiles.Where(x => memberProfileMappingModel.AddressProfileIds.Contains(x.ProfileId)).Select(x => x.Value).ToList());
        IsLoggedInUserMemberProfile = memberProfileMappingModel.IsLoggedInUserMemberProfile;
        ApprenticeShip = source.Apprenticeship;
        OrganisationName = source.OrganisationName;
        FirstName = source.FirstName;
        LastName = source.LastName;
    }

    private string GetValueOrDefault(Profile? profile)
    {
        return profile != null ? profile.Value : string.Empty;
    }
}
