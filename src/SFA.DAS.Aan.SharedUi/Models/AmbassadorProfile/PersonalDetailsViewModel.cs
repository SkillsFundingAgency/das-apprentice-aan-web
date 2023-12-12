using SFA.DAS.Aan.SharedUi.Constants;
using SFA.DAS.Aan.SharedUi.Services;

namespace SFA.DAS.Aan.SharedUi.Models.AmbassadorProfile;
public class PersonalDetailsViewModel
{
    public PersonalDetailsViewModel()
    {
    }

    public PersonalDetailsViewModel(PersonalDetailsModel personalDetails, IEnumerable<MemberProfile> memberProfiles, IEnumerable<MemberPreference> memberPreferences)
    {
        FullName = personalDetails.FullName;
        var (displayValue, displayClass) = MapProfilesAndPreferencesService.SetDisplayValue(true);
        FullNameDisplayValue = displayValue;
        FullNameDisplayClass = displayClass;
        RegionName = personalDetails.RegionName;
        var (regionNameDisplayValue, regionNameDisplayClass) = MapProfilesAndPreferencesService.SetDisplayValue(true);
        RegionNameDisplayValue = regionNameDisplayValue;
        RegionNameDisplayClass = regionNameDisplayClass;
        var (jobTitle, showJobTitle) = MapProfilesAndPreferencesService.GetProfileValueWithPreference(ProfileConstants.ProfileIds.JobTitle, memberProfiles, memberPreferences);
        JobTitle = jobTitle;
        var (jobTitleDisplayValue, jobTitleDisplayClass) = MapProfilesAndPreferencesService.SetDisplayValue(showJobTitle);
        JobTitleDisplayValue = jobTitleDisplayValue;
        JobTitleDisplayClass = jobTitleDisplayClass;
        var (biography, showBiography) = MapProfilesAndPreferencesService.GetProfileValueWithPreference(ProfileConstants.ProfileIds.Biography, memberProfiles, memberPreferences);
        Biography = biography;
        var (biographyDisplayValue, biographyDisplayClass) = MapProfilesAndPreferencesService.SetDisplayValue(showBiography);
        BiographyDisplayValue = biographyDisplayValue;
        BiographyDisplayClass = biographyDisplayClass;
        UserType = personalDetails.UserType;
        PersonalDetailsChangeUrl = personalDetails.PersonalDetailChangeUrl;
    }

    public string FullName { get; set; } = null!;
    public string FullNameDisplayValue { get; set; } = null!;
    public string FullNameDisplayClass { get; set; } = null!;
    public string RegionName { get; set; } = null!;
    public string RegionNameDisplayValue { get; set; } = null!;
    public string RegionNameDisplayClass { get; set; } = null!;
    public string? JobTitle { get; set; }
    public string JobTitleDisplayValue { get; set; } = null!;
    public string JobTitleDisplayClass { get; set; } = null!;
    public string? Biography { get; set; }
    public string BiographyDisplayValue { get; set; } = null!;
    public string BiographyDisplayClass { get; set; } = null!;
    public MemberUserType UserType { get; set; }
    public string PersonalDetailsChangeUrl { get; set; } = null!;
}