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
    public List<string> FirstSectionProfiles { get; set; }
    public List<string> SecondSectionProfiles { get; set; }
    public string Address { get; set; }
    public Apprenticeship? ApprenticeShip { get; set; }
    public bool IsLoggedInUserMemberProfile { get; set; }
    public string AreasOfInterestTitle { get; set; }
    public string AreasOfInterestFirstSectionTitle { get; set; }
    public string AreasOfInterestSecondSectionTitle { get; set; }
    public string InformationSectionTitle { get; set; }
    public string ConnectSectionTitle { get; set; }
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
        FirstSectionProfiles = source.Profiles.Where(x => memberProfileMappingModel.FirstSectionProfileIds.Contains(x.ProfileId)).Select(x => x.Value).ToList();
        SecondSectionProfiles = source.Profiles.Where(x => memberProfileMappingModel.SecondSectionProfileIds.Contains(x.ProfileId)).Select(x => x.Value).ToList();
        Address = string.Join(",", source.Profiles.Where(x => memberProfileMappingModel.AddressProfileIds.Contains(x.ProfileId)).Select(x => x.Value).ToList());
        IsLoggedInUserMemberProfile = memberProfileMappingModel.IsLoggedInUserMemberProfile;
        ApprenticeShip = source.Apprenticeship;
        OrganisationName = source.OrganisationName;
        FirstName = source.FirstName;
        LastName = source.LastName;
        AreasOfInterestTitle = (source.UserType == Role.Apprentice) ? "Here are the areas I’m most interested in as an ambassador." : "Here are my reasons for becoming an ambassador, what support I need\r\nand how I can help other members. ";
        AreasOfInterestFirstSectionTitle = (source.UserType == Role.Apprentice) ? "Events:" : "Why I wanted to join the network:";
        AreasOfInterestSecondSectionTitle = (source.UserType == Role.Apprentice) ? "Promoting the network:" : "What support I need from the network:";
        InformationSectionTitle = (source.UserType == Role.Apprentice) ? $"{FirstName}’s apprenticeship information" : "Employer information";
        ConnectSectionTitle = (source.UserType == Role.Apprentice) ? $"You can connect with {FirstName} by email or LinkedIn." : $"You can contact {FirstName} using the form below or connect directly on LinkedIn.";
    }

    public static string GetValueOrDefault(Profile? profile)
    {
        return profile != null ? profile.Value : string.Empty;
    }
}
