using Moq;
using SFA.DAS.Aan.SharedUi.Constants;
using SFA.DAS.Aan.SharedUi.Models;
using SFA.DAS.Aan.SharedUi.OuterApi.Responses;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Models;

[TestFixture]
public class MemberProfileViewModelTest
{
    MemberProfileMappingModel memberProfileMappingModel = null!;

    [SetUp]
    public void Initialize()
    {
        memberProfileMappingModel = new MemberProfileMappingModel(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), new List<int>(), new List<int>(), new List<int>(), false);
    }

    [Test]
    [MoqAutoData]
    public void MemberProfileViewModel_RegionalChairFalse(MemberProfile memberProfile)
    {
        //Arrange
        memberProfile.IsRegionalChair = false;

        //Act
        MemberProfileViewModel sut = new MemberProfileViewModel(memberProfile, memberProfileMappingModel);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(sut.UserRole, Is.EqualTo(memberProfile.UserType));
        });
    }

    [Test]
    [MoqAutoData]
    public void MemberProfileViewModel_RegionalChairTrue(MemberProfile memberProfile)
    {
        //Arrange
        memberProfile.IsRegionalChair = true;

        //Act
        MemberProfileViewModel sut = new MemberProfileViewModel(memberProfile, memberProfileMappingModel);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(sut.UserRole, Is.EqualTo(Role.RegionalChair));
        });
    }

    [Test]
    [MoqAutoData]
    public void MemberProfileViewModel_RegionIdNull(MemberProfile memberProfile)
    {
        //Arrange
        memberProfile.RegionId = null;

        //Act
        MemberProfileViewModel sut = new MemberProfileViewModel(memberProfile, memberProfileMappingModel);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(sut.RegionId, Is.EqualTo(memberProfile.RegionId));
            Assert.That(sut.RegionName, Is.EqualTo("Multi-regional"));
        });
    }

    [Test]
    [MoqAutoData]
    public void MemberProfileViewModel_RegionIdNotNull(MemberProfile memberProfile)
    {
        //Arrange
        memberProfile.RegionId = 1;

        //Act
        MemberProfileViewModel sut = new MemberProfileViewModel(memberProfile, memberProfileMappingModel);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(sut.RegionId, Is.EqualTo(memberProfile.RegionId));
            Assert.That(sut.RegionName, Is.EqualTo(memberProfile.RegionName));
        });
    }

    [TestCase(Role.Apprentice)]
    public void MemberProfileViewModel_UserRoleApprentice_ReturnsExpectedValueOfTitles(Role role)
    {
        //Arrange
        MemberProfile memberProfile = new MemberProfile();
        memberProfile.UserType = role;
        memberProfile.FirstName = "test";
        string expectedAreasOfInterestTitle = "Here are the areas I’m most interested in as an ambassador.";
        string expectedAreasOfInterestFirstSectionTitle = "Events:";
        string expectedAreasOfInterestSecondSectionTitle = "Promoting the network:";
        string expectedInformationSectionTitle = $"{memberProfile.FirstName}’s apprenticeship information";
        string expectedConnectSectionTitle = $"You can connect with {memberProfile.FirstName} by email or LinkedIn.";

        //Act
        MemberProfileViewModel sut = new MemberProfileViewModel(memberProfile, memberProfileMappingModel);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(sut.UserRole, Is.EqualTo(role));
            Assert.That(sut.AreasOfInterestTitle, Is.EqualTo(expectedAreasOfInterestTitle));
            Assert.That(sut.AreasOfInterestFirstSectionTitle, Is.EqualTo(expectedAreasOfInterestFirstSectionTitle));
            Assert.That(sut.AreasOfInterestSecondSectionTitle, Is.EqualTo(expectedAreasOfInterestSecondSectionTitle));
            Assert.That(sut.InformationSectionTitle, Is.EqualTo(expectedInformationSectionTitle));
            Assert.That(sut.ConnectSectionTitle, Is.EqualTo(expectedConnectSectionTitle));
        });
    }

    [TestCase(Role.Employer)]
    public void MemberProfileViewModel_UserRoleEmployer_ReturnsExpectedValueOfTitles(Role role)
    {
        //Arrange
        MemberProfile memberProfile = new MemberProfile();
        memberProfile.UserType = role;
        memberProfile.FirstName = "test";
        string expectedAreasOfInterestTitle = "Here are my reasons for becoming an ambassador, what support I need and how I can help other members.";
        string expectedAreasOfInterestFirstSectionTitle = "Why I wanted to join the network:";
        string expectedAreasOfInterestSecondSectionTitle = "What support I need from the network:";
        string expectedInformationSectionTitle = "Employer information";
        string expectedConnectSectionTitle = $"You can contact {memberProfile.FirstName} using the form below or connect directly on LinkedIn.";

        //Act
        MemberProfileViewModel sut = new MemberProfileViewModel(memberProfile, memberProfileMappingModel);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(sut.UserRole, Is.EqualTo(role));
            Assert.That(sut.AreasOfInterestTitle, Is.EqualTo(expectedAreasOfInterestTitle));
            Assert.That(sut.AreasOfInterestFirstSectionTitle, Is.EqualTo(expectedAreasOfInterestFirstSectionTitle));
            Assert.That(sut.AreasOfInterestSecondSectionTitle, Is.EqualTo(expectedAreasOfInterestSecondSectionTitle));
            Assert.That(sut.InformationSectionTitle, Is.EqualTo(expectedInformationSectionTitle));
            Assert.That(sut.ConnectSectionTitle, Is.EqualTo(expectedConnectSectionTitle));
        });
    }

    [TestCase(null, "")]
    [TestCase("Multi-Regional", "Multi-Regional")]
    public void GetValueOrDefault_ReturnExpectedValue(string passedValue, string expectedValue)
    {
        //Arrange
        Profile? profile = null;
        if (!string.IsNullOrEmpty(passedValue))
        {
            profile = new Profile { Value = passedValue };
        }

        //Act
        string sut = MemberProfileViewModel.GetValueOrDefault(profile);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(sut, Is.EqualTo(expectedValue));
        });
    }
}