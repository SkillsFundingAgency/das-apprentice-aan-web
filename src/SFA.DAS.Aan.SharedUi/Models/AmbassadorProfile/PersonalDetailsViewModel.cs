using SFA.DAS.Aan.SharedUi.Constants;
using SFA.DAS.Aan.SharedUi.Services;

namespace SFA.DAS.Aan.SharedUi.Models.AmbassadorProfile;
public class PersonalDetailsViewModel
{
    public PersonalDetailsViewModel(PersonalDetailsModel personalDetails, IEnumerable<MemberProfile> memberProfiles, IEnumerable<MemberPreference> memberPreferences, string personalDetailsChangeUrl)
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
        PersonalDetailsChangeUrl = personalDetailsChangeUrl;
    }

    public string FullName { get; set; }
    public string FullNameDisplayValue { get; set; }
    public string FullNameDisplayClass { get; set; }
    public string RegionName { get; set; }
    public string RegionNameDisplayValue { get; set; }
    public string RegionNameDisplayClass { get; set; }
    public string? JobTitle { get; set; }
    public string JobTitleDisplayValue { get; set; }
    public string JobTitleDisplayClass { get; set; }
    public string? Biography { get; set; }
    public string BiographyDisplayValue { get; set; }
    public string BiographyDisplayClass { get; set; }
    public MemberUserType UserType { get; set; }
    public string PersonalDetailsChangeUrl { get; set; }
}