using AutoFixture.NUnit3;
using FluentAssertions;
using SFA.DAS.ApprenticeAan.Web.Models;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Models;

[TestFixture]
public class OnboardingSessionModelTests
{

    [Test]
    public void IsValid_NoProfileData_ReturnsFalse()
    {
        OnboardingSessionModel sut = new() { HasAcceptedTerms = true };

        sut.IsValid.Should().BeFalse();
    }

    [Test]
    [AutoData]
    public void IsValid_HasNotAcceptedTerms_ReturnsFalse(List<ProfileModel> profileData)
    {
        OnboardingSessionModel sut = new() { HasAcceptedTerms = false, ProfileData = profileData };

        sut.IsValid.Should().BeFalse();
    }

    [Test]
    [AutoData]
    public void GetProfileValue_FoundProfileIdWithValue_ReturnsValue(OnboardingSessionModel sut, ProfileModel profileModel)
    {
        sut.ProfileData.Add(profileModel);

        sut.GetProfileValue(profileModel.Id).Should().Be(profileModel.Value);
    }

    [Test]
    [AutoData]
    public void GetProfilelValue_FoundProfileIdWithNoValue_ReturnsNull(OnboardingSessionModel sut, ProfileModel profileModel)
    {
        profileModel.Value = null;
        sut.ProfileData.Add(profileModel);

        sut.GetProfileValue(profileModel.Id).Should().BeNull();
    }

    [Test]
    [AutoData]
    public void GetProfilelValue_ProfileNotFound_ThrowsInvalidOperationException(OnboardingSessionModel sut, ProfileModel profileModel)
    {
        profileModel.Value = null;

        Action action = () => sut.GetProfileValue(profileModel.Id);

        action.Should().Throw<InvalidOperationException>();
    }

    [Test]
    [AutoData]
    public void SetProfileValue_ProfileFound_UpdatesValue(OnboardingSessionModel sut, string value)
    {
        var profileModel = sut.ProfileData[0];

        sut.SetProfileValue(profileModel.Id, value);

        profileModel.Value.Should().Be(value);
    }

    [Test]
    public void SetProfileValue_ProfileNotFound_ThrowsInvalidOperationException()
    {
        OnboardingSessionModel sut = new();

        Action action = () => sut.SetProfileValue(111, "randome value");

        action.Should().Throw<InvalidOperationException>();
    }
}