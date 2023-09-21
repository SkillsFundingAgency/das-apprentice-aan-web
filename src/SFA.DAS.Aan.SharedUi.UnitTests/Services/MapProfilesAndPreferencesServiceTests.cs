using FluentAssertions;
using FluentAssertions.Execution;
using SFA.DAS.Aan.SharedUi.Constants;
using SFA.DAS.Aan.SharedUi.Models.AmbassadorProfile;
using SFA.DAS.Aan.SharedUi.Services;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Aan.SharedUi.UnitTests.Services;
public class MapProfilesAndPreferencesServiceTests
{
    [Test, MoqAutoData]
    public void WhenGettingProfileValue_AndProfileIdIsValid_ThenItIsReturned(IEnumerable<MemberProfile> memberProfiles)
    {
        var profileId = memberProfiles.First().ProfileId;

        var result = MapProfilesAndPreferencesService.GetProfileValue(profileId, memberProfiles);

        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result!.GetType().Should().Be<string>();
            result!.Should().Be(memberProfiles.First().Value);
        }
    }

    [Test, MoqAutoData]
    public void WhenGettingProfileValue_AndProfileIdIsInvalid_ThenNullIsReturned(IEnumerable<MemberProfile> memberProfiles)
    {
        var profileId = 102030405;

        var result = MapProfilesAndPreferencesService.GetProfileValue(profileId, memberProfiles);

        result.Should().BeNull();
    }

    [Test, MoqAutoData]
    public void WhenGettingProfileValueWithPreference_AndProfileValueIsFound_ThenProfileValueAndPreferenceValueIsReturned(IEnumerable<MemberProfile> memberProfiles, IEnumerable<MemberPreference> memberPreferences)
    {
        var profileId = memberProfiles.First().ProfileId;
        memberProfiles.First().PreferenceId = memberPreferences.First().PreferenceId;

        var result = MapProfilesAndPreferencesService.GetProfileValueWithPreference(profileId, memberProfiles, memberPreferences);

        using (new AssertionScope())
        {
            result.profileValue.Should().NotBeNull();
            result.profileValue!.GetType().Should().Be<string>();
            result.profileValue!.Should().Be(memberProfiles.First().Value);
            result.isDisplayed.GetType().Should().Be<bool>();
            result.isDisplayed.Should().Be(memberPreferences.First().Value);
        }
    }

    [Test, MoqAutoData]
    public void WhenGettingProfileValueWithPreference_AndProfileValueIsNotFound_ThenNullAndFalseIsReturned(IEnumerable<MemberProfile> memberProfiles, IEnumerable<MemberPreference> memberPreferences)
    {
        var profileId = 102030405;
        memberProfiles.First().PreferenceId = 12345;

        var result = MapProfilesAndPreferencesService.GetProfileValueWithPreference(profileId, memberProfiles, memberPreferences);

        using (new AssertionScope())
        {
            result.profileValue.Should().BeNull();
            result.isDisplayed.GetType().Should().Be<bool>();
            result.isDisplayed.Should().BeFalse();
        }
    }

    [Test, MoqAutoData]
    public void WhenSettingDisplayValue_AndPreferenceIsTrue_ThenDisplayValueAndDisplayClassIsReturned()
    {
        var preference = true;

        var (displayValue, displayClass) = MapProfilesAndPreferencesService.SetDisplayValue(preference);

        using (new AssertionScope())
        {
            displayValue.Should().NotBeNull();
            displayValue.Should().Be(PreferenceConstants.DisplayValue.DisplayTagName);
            displayClass.Should().NotBeNull();
            displayClass.Should().Be(PreferenceConstants.DisplayValue.DisplayTagClass);
        }
    }

    [Test, MoqAutoData]
    public void WhenSettingDisplayValue_AndPreferenceIsFalse_ThenHiddenValueAndHiddenClassIsReturned()
    {
        var preference = false;

        var (displayValue, displayClass) = MapProfilesAndPreferencesService.SetDisplayValue(preference);

        using (new AssertionScope())
        {
            displayValue.Should().NotBeNull();
            displayValue.Should().Be(PreferenceConstants.DisplayValue.HiddenTagName);
            displayClass.Should().NotBeNull();
            displayClass.Should().Be(PreferenceConstants.DisplayValue.HiddenTagClass);
        }
    }
}
