using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using SFA.DAS.Aan.SharedUi.Constants;
using SFA.DAS.Aan.SharedUi.Models.AmbassadorProfile;

namespace SFA.DAS.Aan.SharedUi.UnitTests.Models.AmbassadorProfileViewModelsTests;
public class PersonalDetailsViewModelTests
{
    private PersonalDetailsViewModel sut;
    private string fullName;
    private string regionName;
    private string personalDetailsChangeUrl = Guid.NewGuid().ToString();
    private string areaOfInterestChangeUrl = Guid.NewGuid().ToString();
    private IEnumerable<MemberProfile> memberProfiles;
    private IEnumerable<MemberPreference> memberPreferences;
    private MemberUserType userType;

    [SetUp]
    public void Setup()
    {
        var fixture = new Fixture();
        fullName = fixture.Create<string>();
        regionName = fixture.Create<string>();
        memberProfiles = fixture.CreateMany<MemberProfile>(4);
        memberPreferences = fixture.CreateMany<MemberPreference>();
        memberProfiles.ToArray()[0].ProfileId = ProfileConstants.ProfileIds.JobTitle;
        memberPreferences.ToArray()[0].PreferenceId = PreferenceConstants.PreferenceIds.JobTitle;
        memberProfiles.ToArray()[0].PreferenceId = memberPreferences.ToArray()[0].PreferenceId;
        memberPreferences.ToArray()[0].Value = true;
        memberProfiles.ToArray()[1].ProfileId = ProfileConstants.ProfileIds.Biography;
        memberPreferences.ToArray()[1].PreferenceId = PreferenceConstants.PreferenceIds.Biography;
        memberProfiles.ToArray()[1].PreferenceId = memberPreferences.ToArray()[1].PreferenceId;
        memberPreferences.ToArray()[1].Value = false;
        userType = MemberUserType.Apprentice;
        var personalDetails = new PersonalDetailsModel(fullName, regionName, userType, personalDetailsChangeUrl, areaOfInterestChangeUrl);
        sut = new PersonalDetailsViewModel(personalDetails, memberProfiles, memberPreferences);
    }

    [Test]
    public void PersonalDetailsViewModelIsSet()
    {
        using (new AssertionScope())
        {
            sut.Should().NotBeNull();
            sut.FullName.Should().Be(fullName);
            sut.JobTitleDisplayValue.GetType().Should().Be(typeof(string));
            sut.UserType.Should().Be(userType);
        }
    }

    [Test]
    public void BiographyDisplayPropertiesAreSet()
    {
        using (new AssertionScope())
        {
            if ((bool)memberPreferences.First(x => x.PreferenceId == PreferenceConstants.PreferenceIds.Biography)?.Value!)
            {
                sut.BiographyDisplayClass.Should().Be(PreferenceConstants.DisplayValue.DisplayTagClass);
                sut.BiographyDisplayValue.Should().Be(PreferenceConstants.DisplayValue.DisplayTagName);
            }
            else
            {
                sut.BiographyDisplayClass.Should().Be(PreferenceConstants.DisplayValue.HiddenTagClass);
                sut.BiographyDisplayValue.Should().Be(PreferenceConstants.DisplayValue.HiddenTagName);
            }
        }
    }

    [Test]
    public void JobTitleDisplayPropertiesAreSet()
    {
        using (new AssertionScope())
        {
            if ((bool)memberPreferences.First(x => x.PreferenceId == PreferenceConstants.PreferenceIds.JobTitle)?.Value!)
            {
                sut.JobTitleDisplayClass.Should().Be(PreferenceConstants.DisplayValue.DisplayTagClass);
                sut.JobTitleDisplayValue.Should().Be(PreferenceConstants.DisplayValue.DisplayTagName);
            }
            else
            {
                sut.JobTitleDisplayClass.Should().Be(PreferenceConstants.DisplayValue.HiddenTagClass);
                sut.JobTitleDisplayValue.Should().Be(PreferenceConstants.DisplayValue.HiddenTagName);
            }
        }
    }

    [Test]
    public void RegionNameDisplayPropertiesAreSet()
    {
        using (new AssertionScope())
        {
            sut.RegionNameDisplayClass.Should().Be(PreferenceConstants.DisplayValue.DisplayTagClass);
            sut.RegionNameDisplayValue.Should().Be(PreferenceConstants.DisplayValue.DisplayTagName);
        }
    }

    [Test]
    public void FullNameDisplayPropertiesAreSet()
    {
        using (new AssertionScope())
        {
            sut.FullNameDisplayClass.Should().Be(PreferenceConstants.DisplayValue.DisplayTagClass);
            sut.FullNameDisplayValue.Should().Be(PreferenceConstants.DisplayValue.DisplayTagName);
        }
    }

    [Test]
    public void PersonalDetailsChangeUrlPropertyAreSet()
    {
        using (new AssertionScope())
        {
            sut.PersonalDetailsChangeUrl.Should().BeEquivalentTo(personalDetailsChangeUrl);
        }
    }
}
