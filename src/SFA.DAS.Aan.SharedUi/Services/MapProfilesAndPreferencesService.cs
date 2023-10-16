using SFA.DAS.Aan.SharedUi.Constants;
using SFA.DAS.Aan.SharedUi.Models;
using SFA.DAS.Aan.SharedUi.Models.AmbassadorProfile;

namespace SFA.DAS.Aan.SharedUi.Services;
public static class MapProfilesAndPreferencesService
{
    public static (string? profileValue, bool isDisplayed) GetProfileValueWithPreference(int profileId, IEnumerable<MemberProfile> memberProfiles, IEnumerable<MemberPreference> memberPreferences)
    {
        var value = GetProfileValue(profileId, memberProfiles);
        var isDisplayed = memberPreferences.FirstOrDefault(p => p.PreferenceId == memberProfiles.FirstOrDefault(p => p.ProfileId == profileId)?.PreferenceId)?.Value ?? false;
        return (value, isDisplayed);
    }

    public static string? GetProfileValue(int profileId, IEnumerable<MemberProfile> memberProfiles) => memberProfiles.FirstOrDefault(p => p.ProfileId == profileId)?.Value;

    public static string? GetProfileDescription(MemberProfile memberProfile, IEnumerable<Profile> profiles) => profiles.FirstOrDefault(p => p.Id == memberProfile.ProfileId && memberProfile.Value.ToLower() == "true")?.Description;

    public static (string displayValue, string displayClass) SetDisplayValue(bool preference)
    {
        if (preference)
        {
            return (PreferenceConstants.DisplayValue.DisplayTagName, PreferenceConstants.DisplayValue.DisplayTagClass);
        }
        else
        {
            return (PreferenceConstants.DisplayValue.HiddenTagName, PreferenceConstants.DisplayValue.HiddenTagClass);
        }
    }
}