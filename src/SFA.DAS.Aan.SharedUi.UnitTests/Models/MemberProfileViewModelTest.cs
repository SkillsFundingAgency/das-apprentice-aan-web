using SFA.DAS.Aan.SharedUi.Constants;
using SFA.DAS.Aan.SharedUi.Infrastructure;
using SFA.DAS.Aan.SharedUi.Models;
using SFA.DAS.Aan.SharedUi.OuterApi.Responses;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Models;

[TestFixture]
public class MemberProfileViewModelTest
{
    MemberProfileMappingModel memberProfileMappingModel = null!;
    List<UserTypeProfilesModel> userTypeProfilesModels = null!;

    [SetUp]
    public void Initialize()
    {
        memberProfileMappingModel = new MemberProfileMappingModel();
        userTypeProfilesModels = new List<UserTypeProfilesModel>()
        {
            new UserTypeProfilesModel { Id = 2, Description = "Presenting at events in person", Category = "Events", Ordering = 2 },
            new UserTypeProfilesModel { Id = 10, Description = "Carrying out and writing up case studies", Category = "Promotions", Ordering = 3 },
            new UserTypeProfilesModel { Id = 11, Description = "Designing and creating marketing materials to champion the network", Category = "Promotions", Ordering = 4 },
            new UserTypeProfilesModel { Id = 1, Description = "Networking at events in person", Category = "Events", Ordering = 1 },
        };
    }

    [Test]
    [MoqAutoData]
    public void MemberProfileViewModel_RegionalChairFalse(MemberProfile memberProfile)
    {
        //Arrange
        memberProfile.IsRegionalChair = false;

        //Act
        MemberProfileViewModel sut = new MemberProfileViewModel(memberProfile, memberProfileMappingModel, userTypeProfilesModels);

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
        MemberProfileViewModel sut = new MemberProfileViewModel(memberProfile, memberProfileMappingModel, userTypeProfilesModels);

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
        MemberProfileViewModel sut = new MemberProfileViewModel(memberProfile, memberProfileMappingModel, userTypeProfilesModels);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(sut.RegionId, Is.EqualTo(memberProfile.RegionId));
            Assert.That(sut.RegionName, Is.EqualTo(MemberProfileTitle.MultiRegionalRegion));
        });
    }

    [Test]
    [MoqAutoData]
    public void MemberProfileViewModel_RegionIdNotNull(MemberProfile memberProfile)
    {
        //Arrange
        memberProfile.RegionId = 1;

        //Act
        MemberProfileViewModel sut = new MemberProfileViewModel(memberProfile, memberProfileMappingModel, userTypeProfilesModels);

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
        string expectedAreasOfInterestTitle = MemberProfileTitle.ApprenticeAreasOfInterestTitle;
        string expectedAreasOfInterestFirstSectionTitle = MemberProfileTitle.ApprenticeAreasOfInterestFirstSectionTitle;
        string expectedAreasOfInterestSecondSectionTitle = MemberProfileTitle.ApprenticeAreasOfInterestSecondSectionTitle;
        string expectedInformationSectionTitle = $"{memberProfile.FirstName}’s apprenticeship information";
        string expectedConnectSectionTitle = $"You can connect with {memberProfile.FirstName} by email or LinkedIn.";

        //Act
        MemberProfileViewModel sut = new MemberProfileViewModel(memberProfile, memberProfileMappingModel, userTypeProfilesModels);

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
        string expectedAreasOfInterestTitle = MemberProfileTitle.EmployerAreasOfInterestTitle;
        string expectedAreasOfInterestFirstSectionTitle = MemberProfileTitle.EmployerAreasOfInterestFirstSectionTitle;
        string expectedAreasOfInterestSecondSectionTitle = MemberProfileTitle.EmployerAreasOfInterestSecondSectionTitle;
        string expectedInformationSectionTitle = MemberProfileTitle.EmployerInformationSectionTitle;
        string expectedConnectSectionTitle = $"You can contact {memberProfile.FirstName} using the form below or connect directly on LinkedIn.";

        //Act
        MemberProfileViewModel sut = new MemberProfileViewModel(memberProfile, memberProfileMappingModel, userTypeProfilesModels);

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
        Assert.That(sut, Is.EqualTo(expectedValue));
    }

    [Test]
    public void GetSectionProfilesDescription_ReturnExpectedValueAndCount()
    {
        //Arrange
        List<Profile> profiles = new List<Profile>()
        {
          new Profile { ProfileId = 1, Value = "True", PreferenceId = null},
          new Profile { ProfileId = 2, Value = "False", PreferenceId = null},
          new Profile { ProfileId = 10, Value = "True", PreferenceId = null},
          new Profile { ProfileId = 11, Value = "False", PreferenceId = null }
        };

        //Act
        List<string> sut = MemberProfileViewModel.GetSectionProfilesDescription(profiles, userTypeProfilesModels);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(sut, Has.Count.EqualTo(2));
            Assert.That(sut.FirstOrDefault(), Is.EqualTo("Networking at events in person"));
        });
    }
}