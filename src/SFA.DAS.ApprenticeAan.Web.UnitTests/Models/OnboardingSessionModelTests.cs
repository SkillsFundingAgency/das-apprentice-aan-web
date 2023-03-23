using AutoFixture.NUnit3;
using FluentAssertions;
using SFA.DAS.ApprenticeAan.Domain.Constants;
using SFA.DAS.ApprenticeAan.Web.Models;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Models;

[TestFixture]
public class OnboardingSessionModelTests
{
    private const string Category = "Employer";
    private const string EmployerName = "Department for Education";
    private const string AddressLine1 = "20 Great Smith St";
    private const string Town = "London";
    private const string Postcode = "SW1P 3BT";

    private static readonly List<ProfileModel> profileData = new()
    {
        new() { Id = ProfileDataId.EmployerName, Category = Category, Value = EmployerName},
        new() { Id = ProfileDataId.AddressLine1, Category = Category, Value = AddressLine1},
        new() { Id = ProfileDataId.AddressLine2, Category = Category},
        new() { Id = ProfileDataId.Town, Category = Category, Value = Town},
        new() { Id = ProfileDataId.County, Category = Category, },
        new() { Id = ProfileDataId.Postcode, Category = Category, Value = Postcode}
    };

    private static readonly object[] GetProfileData =
    {
        new object[] { new Guid("bf92480f-1ef6-4552-8a5e-35d7951213f9"), true, true, new List<ProfileModel>(), false },

        new object[] { new Guid("bf92480f-1ef6-4552-8a5e-35d7951213f9"), true, true, profileData, true },
    };

    [TestCase(null, true, true, false)]
    [TestCase("00000000-0000-0000-0000-000000000000", true, true, false)]
    [TestCase("bf92480f-1ef6-4552-8a5e-35d7951213f9", false, true, false)]
    [TestCase("bf92480f-1ef6-4552-8a5e-35d7951213f9", true, false, false)]
    [TestCase("bf92480f-1ef6-4552-8a5e-35d7951213f9", true, null, false)]
    public void IsValid_ChecksModelValidity(Guid? apprenticeId, bool hasAcceptedTAndCs, bool? hasEmployersApproval, bool expectedResult)
    {
        var model = new OnboardingSessionModel()
        {
            HasAcceptedTermsAndConditions = hasAcceptedTAndCs,
            HasEmployersApproval = hasEmployersApproval,
        };
        model.ApprenticeDetails.ApprenticeId = apprenticeId;

        var actualResult = model.IsValid;

        Assert.That(actualResult, Is.EqualTo(expectedResult));
    }

    [TestCaseSource(nameof(GetProfileData))]
    public void IsValid_ProfileData_ChecksModelValidity(Guid? apprenticeId, bool hasAcceptedTAndCs, bool? hasEmployersApproval,
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
    public void GetProfileModelValue_ProfileNotFound_Throws(OnboardingSessionModel sut, ProfileModel profileModel)
    {
        profileModel.Value = null;

        Action action = () => sut.GetProfileValue(profileModel.Id);

        action.Should().Throw<InvalidOperationException>();
    }

    [Test]
    [AutoData]
    public void GetProfileModelValue_ProfileFound_UpdatesValue(OnboardingSessionModel sut, string value)
    {
        var profileModel = sut.ProfileData[0];

        sut.SetProfileValue(profileModel.Id, value);

        profileModel.Value.Should().Be(value);
    }

}