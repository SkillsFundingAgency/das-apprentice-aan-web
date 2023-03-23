using AutoFixture.NUnit3;
using FluentAssertions;
using SFA.DAS.ApprenticeAan.Web.Models;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Models;

[TestFixture]
public class OnboardingSessionModelTests
{
    private static readonly object[] GetProfileData =
    {
        new object[] { new Guid("bf92480f-1ef6-4552-8a5e-35d7951213f9"), true, true, new List<ProfileModel>(), false },

        new object[] { new Guid("bf92480f-1ef6-4552-8a5e-35d7951213f9"), true, true, new List<ProfileModel> { new ProfileModel() }, true },
    };

    [TestCaseSource(nameof(GetProfileData))]
    public void IsValid_ProfileData_ChecksModelValidity(
        Guid? apprenticeId, bool hasAcceptedTAndCs, bool? hasEmployersApproval,
        List<ProfileModel> profileData, bool expectedResult)
    {
        var model = new OnboardingSessionModel()
        {
            HasAcceptedTermsAndConditions = hasAcceptedTAndCs,
            HasEmployersApproval = hasEmployersApproval,
        };
        model.ApprenticeDetails.ApprenticeId = apprenticeId;
        model.ProfileData = profileData;

        var actualResult = model.IsValid;

        Assert.That(actualResult, Is.EqualTo(expectedResult));
    }

    [TestCase(null, true, true, false)]
    [TestCase("00000000-0000-0000-0000-000000000000", true, true, false)]
    [TestCase("bf92480f-1ef6-4552-8a5e-35d7951213f9", false, true, false)]
    [TestCase("bf92480f-1ef6-4552-8a5e-35d7951213f9", true, false, false)]
    [TestCase("bf92480f-1ef6-4552-8a5e-35d7951213f9", true, null, false)]
    [TestCase("bf92480f-1ef6-4552-8a5e-35d7951213f9", true, true, true)]
    public void IsValid_ChecksModelValidity(Guid? apprenticeId, bool hasAcceptedTAndCs, bool? hasEmployersApproval, bool expectedResult)
    {
        var model = new OnboardingSessionModel()
        {
            HasAcceptedTermsAndConditions = hasAcceptedTAndCs,
            HasEmployersApproval = hasEmployersApproval,
        };
        model.ProfileData.Add(new ProfileModel());
        model.ApprenticeDetails.ApprenticeId = apprenticeId;

        var actualResult = model.IsValid;

        Assert.That(actualResult, Is.EqualTo(expectedResult));
    }

    [Test]
    [AutoData]
    public void GetProfileModelValue_FoundProfileIdWithValue_ReturnsValue(OnboardingSessionModel sut, ProfileModel profileModel)
    {
        sut.ProfileData.Add(profileModel);

        sut.GetProfileValue(profileModel.Id).Should().Be(profileModel.Value);
    }

    [Test]
    [AutoData]
    public void GetProfileModelValue_FoundProfileIdWithNoValue_ReturnsNull(OnboardingSessionModel sut, ProfileModel profileModel)
    {
        profileModel.Value = null;
        sut.ProfileData.Add(profileModel);

        sut.GetProfileValue(profileModel.Id).Should().BeNull();
    }

    [Test]
    [AutoData]
    public void GetProfileModelValue_ProfileNotFound_ThrowsInvalidOperationException(OnboardingSessionModel sut, ProfileModel profileModel)
    {
        profileModel.Value = null;

        Action action = () => sut.GetProfileValue(profileModel.Id);

        action.Should().Throw<InvalidOperationException>();
    }

    [Test]
    [AutoData]
    public void SetProfileModelValue_ProfileFound_UpdatesValue(OnboardingSessionModel sut, string value)
    {
        var profileModel = sut.ProfileData[0];

        sut.SetProfileValue(profileModel.Id, value);

        profileModel.Value.Should().Be(value);
    }

    [Test]
    public void SetProfileModelValue_ProfileNotFound_ThrowsInvalidOperationException()
    {
        OnboardingSessionModel sut = new();

        Action action = () => sut.SetProfileValue(111, "randome value");

        action.Should().Throw<InvalidOperationException>();
    }
}