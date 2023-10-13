using SFA.DAS.Aan.SharedUi.Constants;
using SFA.DAS.Aan.SharedUi.Services;

namespace SFA.DAS.Aan.SharedUi.Models.AmbassadorProfile;
public class ContactDetailsViewModel
{
    public ContactDetailsViewModel(string emailAddress, IEnumerable<MemberProfile> memberProfiles, IEnumerable<MemberPreference> memberPreferences)
    {
        EmailAddress = emailAddress;
        var (profileValue, isDisplayed) = MapProfilesAndPreferencesService.GetProfileValueWithPreference(ProfileConstants.ProfileIds.LinkedIn, memberProfiles, memberPreferences);
        LinkedIn = profileValue;
        var (displayValue, displayClass) = MapProfilesAndPreferencesService.SetDisplayValue(isDisplayed);
        LinkedInDisplayValue = displayValue;
        LinkedInDisplayClass = displayClass;
    }

    public string EmailAddress { get; set; }
    public string? LinkedIn { get; set; }
    public string LinkedInDisplayValue { get; set; }
    public string LinkedInDisplayClass { get; set; }
}