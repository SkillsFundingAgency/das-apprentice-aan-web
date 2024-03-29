﻿using SFA.DAS.Aan.SharedUi.Constants;
using SFA.DAS.Aan.SharedUi.Services;

namespace SFA.DAS.Aan.SharedUi.Models.AmbassadorProfile;
public class ContactDetailsViewModel
{
    public ContactDetailsViewModel()
    {
    }

    public ContactDetailsViewModel(string emailAddress, IEnumerable<MemberProfile> memberProfiles, IEnumerable<MemberPreference> memberPreferences, string contactDetailChangeUrl)
    {
        EmailAddress = emailAddress;
        var (profileValue, isDisplayed) = MapProfilesAndPreferencesService.GetProfileValueWithPreference(ProfileConstants.ProfileIds.LinkedIn, memberProfiles, memberPreferences);
        LinkedIn = profileValue;
        var (displayValue, displayClass) = MapProfilesAndPreferencesService.SetDisplayValue(isDisplayed);
        LinkedInDisplayValue = displayValue;
        LinkedInDisplayClass = displayClass;
        ContactDetailChangeUrl = contactDetailChangeUrl;
    }

    public string EmailAddress { get; set; } = null!;
    public string? LinkedIn { get; set; }
    public string LinkedInDisplayValue { get; set; } = null!;
    public string LinkedInDisplayClass { get; set; } = null!;
    public string ContactDetailChangeUrl { get; set; } = null!;
}