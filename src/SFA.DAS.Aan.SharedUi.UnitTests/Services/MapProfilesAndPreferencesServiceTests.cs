using FluentAssertions;
using FluentAssertions.Execution;
using SFA.DAS.Aan.SharedUi.Constants;
using SFA.DAS.Aan.SharedUi.Models;
using SFA.DAS.Aan.SharedUi.Models.AmbassadorProfile;
using SFA.DAS.Aan.SharedUi.Services;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Aan.SharedUi.UnitTests.Services;
public class MapProfilesAndPreferencesServiceTests
{
    private static IEnumerable<Profile> profiles = null!;

    [SetUp]
    public void Setup()
    {
        //Arrange
        profiles = new List<Profile>()
        {
            new Profile(){ Id=1,Description="Profile 1",Category="Events",Ordering=1},
            new Profile(){ Id=2,Description="Profile 2",Category="Events",Ordering=2},
            new Profile(){ Id=3,Description="Profile 3",Category="Promotion",Ordering=3},
            new Profile(){ Id=4,Description="Profile 4",Category="Promotion",Ordering=4}
        };
    }

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

    [TestCase("True")]
    [TestCase("False")]
    [TestCase("MemberProfile")]
    public void WhenGettingProfileDescription_AndProfileIdValid_ReturnsExpectedValue(string memberProfileValue)
    {
        //Arrange
        MemberProfile memberProfile = new MemberProfile()
        {
            ProfileId = 1,
            PreferenceId = 2,
            Value = memberProfileValue
        };

        //Act
        var sut = MapProfilesAndPreferencesService.GetProfileDescription(memberProfile, profiles);

        //Assert
        using (new AssertionScope())
        {
            if (memberProfileValue.ToLower() == "true")
            {
                sut.Should().NotBeNull();
                sut!.GetType().Should().Be<string>();
                sut.Should().Be(profiles.First().Description);
            }
            else
            {
                sut.Should().BeNull();
            }
        }
    }

    [Test]
    public static void WhenGettingProfileDescription_AndProfileIdIsInValid_ReturnsNull()
    {
        //Arrange
        MemberProfile memberProfile = new MemberProfile()
        {
            ProfileId = 145452154,
            PreferenceId = 2,
            Value = "true"
        };

        //Act
        var sut = MapProfilesAndPreferencesService.GetProfileDescription(memberProfile, profiles);

        //Assert
        using (new AssertionScope())
        {
            sut.Should().BeNull();
        }
    }

    [Test, MoqAutoData]
    public void WhenGettingPreferenceValue_AndPreferenceIsValid_ThenItIsReturned(IEnumerable<MemberPreference> memberPreferences)
    {
        var preferenceId = memberPreferences.First().PreferenceId;

        var result = MapProfilesAndPreferencesService.GetPreferenceValue(preferenceId, memberPreferences);

        using (new AssertionScope())
        {
            result!.GetType().Should().Be<bool>();
            result!.Should().Be(memberPreferences.First().Value);
        }
    }

    [Test, MoqAutoData]
    public void WhenGettingPreferenceValue_AndPreferenceIsValid_ThenFalseIsReturned(IEnumerable<MemberPreference> memberPreferences)
    {
        var preferenceId = 102030405;

        var result = MapProfilesAndPreferencesService.GetPreferenceValue(preferenceId, memberPreferences);

        result.Should().BeFalse();
    }
}
