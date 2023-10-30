using SFA.DAS.Aan.SharedUi.Infrastructure;
using SFA.DAS.Aan.SharedUi.Models.AmbassadorProfile;
using SFA.DAS.Aan.SharedUi.Services;

namespace SFA.DAS.Aan.SharedUi.Models;
public class MemberProfileViewModel : INetworkHubLink
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? EmployerName { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public int? RegionId { get; set; }
    public string RegionName { get; set; }
    public MemberUserType UserRole { get; set; }
    public string? JobTitle { get; set; }
    public string? Biography { get; set; }
    public string? LinkedinUrl { get; set; }
    public List<string> FirstSectionProfiles { get; set; }
    public List<string> SecondSectionProfiles { get; set; }
    public string Address { get; set; }
    public string Sector { get; set; }
    public string Programmes { get; set; }
    public string Level { get; set; }
    public List<string>? Sectors { get; set; }
    public int ActiveApprenticesCount { get; set; }
    public bool IsLoggedInUserMemberProfile { get; set; }
    public string AreasOfInterestTitle { get; set; }
    public string AreasOfInterestFirstSectionTitle { get; set; }
    public string AreasOfInterestSecondSectionTitle { get; set; }
    public string InformationSectionTitle { get; set; }
    public string ConnectSectionTitle { get; set; }
    public MemberUserType UserType { get; set; }
    public bool IsEmployerInformationAvailable { get; set; }
    public bool IsApprenticeshipInformationAvailable { get; set; }
    public string? NetworkHubLink { get; set; }
    public int ReasonToGetInTouch { get; set; }
    public bool DetailShareAllowed { get; set; }
    public bool CodeOfConduct { get; set; }


    public MemberProfileViewModel(MemberProfileDetail memberProfileDetail, IEnumerable<Profile> memberProfiles, MemberProfileMappingModel memberProfileMappingModel)
    {
        FullName = memberProfileDetail.FullName;
        Email = memberProfileDetail.Email;
        RegionId = memberProfileDetail.RegionId;
        RegionName = memberProfileDetail.RegionName;
        UserRole = (memberProfileDetail.IsRegionalChair) ? MemberUserType.RegionalChair : memberProfileDetail.UserType;
        JobTitle = MapProfilesAndPreferencesService.GetProfileValue(memberProfileMappingModel.JobTitleProfileId, memberProfileDetail.Profiles);
        Biography = MapProfilesAndPreferencesService.GetProfileValue(memberProfileMappingModel.BiographyProfileId, memberProfileDetail.Profiles);
        LinkedinUrl = MapProfilesAndPreferencesService.GetProfileValue(memberProfileMappingModel.LinkedinProfileId, memberProfileDetail.Profiles);
        FirstSectionProfiles = memberProfileDetail.Profiles.Where(x => memberProfileMappingModel.FirstSectionProfileIds.Contains(x.ProfileId)).Select(x => MapProfilesAndPreferencesService.GetProfileDescription(x, memberProfiles)).Where(x => !string.IsNullOrWhiteSpace(x)).ToList()!;
        SecondSectionProfiles = memberProfileDetail.Profiles.Where(x => memberProfileMappingModel.SecondSectionProfileIds.Contains(x.ProfileId)).Select(x => MapProfilesAndPreferencesService.GetProfileDescription(x, memberProfiles)).Where(x => !string.IsNullOrWhiteSpace(x)).ToList()!;
        Address = string.Join(",", memberProfileDetail.Profiles.Where(x => memberProfileMappingModel.AddressProfileIds.Contains(x.ProfileId) && !string.IsNullOrWhiteSpace(x.Value)).Select(x => x.Value).ToList());
        IsLoggedInUserMemberProfile = memberProfileMappingModel.IsLoggedInUserMemberProfile;
        Sector = memberProfileDetail.Sector;
        Programmes = memberProfileDetail.Programmes;
        Level = memberProfileDetail.Level;
        Sectors = memberProfileDetail.Sectors;
        ActiveApprenticesCount = memberProfileDetail.ActiveApprenticesCount;
        EmployerName = MapProfilesAndPreferencesService.GetProfileValue(memberProfileMappingModel.EmployerNameProfileId, memberProfileDetail.Profiles);
        FirstName = memberProfileDetail.FirstName;
        LastName = memberProfileDetail.LastName;
        AreasOfInterestTitle = (memberProfileDetail.UserType == MemberUserType.Apprentice) ? MemberProfileTitle.ApprenticeAreasOfInterestTitle : MemberProfileTitle.EmployerAreasOfInterestTitle;
        AreasOfInterestFirstSectionTitle = (memberProfileDetail.UserType == MemberUserType.Apprentice) ? MemberProfileTitle.ApprenticeAreasOfInterestFirstSectionTitle : MemberProfileTitle.EmployerAreasOfInterestFirstSectionTitle;
        AreasOfInterestSecondSectionTitle = (memberProfileDetail.UserType == MemberUserType.Apprentice) ? MemberProfileTitle.ApprenticeAreasOfInterestSecondSectionTitle : MemberProfileTitle.EmployerAreasOfInterestSecondSectionTitle;
        InformationSectionTitle = (memberProfileDetail.UserType == MemberUserType.Apprentice) ? $"{FirstName}’s apprenticeship information" : MemberProfileTitle.EmployerInformationSectionTitle;
        ConnectSectionTitle = (memberProfileDetail.UserType == MemberUserType.Apprentice) ? $"You can connect with {FirstName} by email or LinkedIn." : $"You can contact {FirstName} using the form below or connect directly on LinkedIn.";
        UserType = memberProfileDetail.UserType;
        IsEmployerInformationAvailable = !string.IsNullOrEmpty(EmployerName) || !string.IsNullOrEmpty(Address);
        IsApprenticeshipInformationAvailable = ((UserType == MemberUserType.Apprentice) ? (!string.IsNullOrEmpty(Sector) || !string.IsNullOrEmpty(Programmes) || !string.IsNullOrEmpty(Level)) : (Sectors.Count > 0 || ActiveApprenticesCount > 0));
    }
}