using SFA.DAS.Aan.SharedUi.Infrastructure;
using SFA.DAS.Aan.SharedUi.Models;
using SFA.DAS.Aan.SharedUi.Models.AmbassadorProfile;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Models;

[TestFixture]
public class MemberProfileViewModelTest
{
    MemberProfileMappingModel memberProfileMappingModel = null!;
    List<Profile> profiles = null!;

    [SetUp]
    public void Setup()
    {
        //Arrange
        memberProfileMappingModel = new MemberProfileMappingModel();
        profiles = new List<Profile>()
        {
            new Profile { Id = 2, Description = "Presenting at events in person", Category = "Events", Ordering = 2 },
            new Profile { Id = 10, Description = "Carrying out and writing up case studies", Category = "Promotions", Ordering = 3 },
            new Profile { Id = 11, Description = "Designing and creating marketing materials to champion the network", Category = "Promotions", Ordering = 4 },
            new Profile { Id = 1, Description = "Networking at events in person", Category = "Events", Ordering = 1 },
        };
    }

    [Test]
    [MoqAutoData]
    public void MemberProfileViewModel_RegionalChairFalse(MemberProfileDetail memberProfile)
    {
        //Arrange
        memberProfile.IsRegionalChair = false;

        //Act
        MemberProfileViewModel sut = new MemberProfileViewModel(memberProfile, profiles, memberProfileMappingModel);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(sut.UserRole, Is.EqualTo(memberProfile.UserType));
        });
    }

    [Test]
    [MoqAutoData]
    public void MemberProfileViewModel_RegionalChairTrue(MemberProfileDetail memberProfile)
    {
        //Arrange
        memberProfile.IsRegionalChair = true;

        //Act
        MemberProfileViewModel sut = new MemberProfileViewModel(memberProfile, profiles, memberProfileMappingModel);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(sut.UserRole, Is.EqualTo(MemberUserType.RegionalChair));
        });
    }

    [Test]
    [MoqAutoData]
    public void MemberProfileViewModel_RegionIdNotNull(MemberProfileDetail memberProfile)
    {
        //Arrange
        memberProfile.RegionId = 1;

        //Act
        MemberProfileViewModel sut = new MemberProfileViewModel(memberProfile, profiles, memberProfileMappingModel);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(sut.RegionId, Is.EqualTo(memberProfile.RegionId));
            Assert.That(sut.RegionName, Is.EqualTo(memberProfile.RegionName));
        });
    }

    [TestCase(MemberUserType.Apprentice)]
    public void MemberProfileViewModel_UserRoleApprentice_ReturnsExpectedValueOfTitles(MemberUserType memberUserType)
    {
        //Arrange
        MemberProfileDetail memberProfile = new MemberProfileDetail();
        memberProfile.UserType = memberUserType;
        memberProfile.FirstName = "test";
        memberProfile.Profiles = new List<MemberProfile>();
        string expectedAreasOfInterestTitle = MemberProfileTitle.ApprenticeAreasOfInterestTitle;
        string expectedAreasOfInterestFirstSectionTitle = MemberProfileTitle.ApprenticeAreasOfInterestFirstSectionTitle;
        string expectedAreasOfInterestSecondSectionTitle = MemberProfileTitle.ApprenticeAreasOfInterestSecondSectionTitle;
        string expectedInformationSectionTitle = $"{memberProfile.FirstName}’s apprenticeship information";
        string expectedConnectSectionTitle = $"You can connect with {memberProfile.FirstName} by email or LinkedIn.";

        //Act
        MemberProfileViewModel sut = new MemberProfileViewModel(memberProfile, profiles, memberProfileMappingModel);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(sut.UserRole, Is.EqualTo(memberUserType));
            Assert.That(sut.AreasOfInterestTitle, Is.EqualTo(expectedAreasOfInterestTitle));
            Assert.That(sut.AreasOfInterestFirstSectionTitle, Is.EqualTo(expectedAreasOfInterestFirstSectionTitle));
            Assert.That(sut.AreasOfInterestSecondSectionTitle, Is.EqualTo(expectedAreasOfInterestSecondSectionTitle));
            Assert.That(sut.InformationSectionTitle, Is.EqualTo(expectedInformationSectionTitle));
            Assert.That(sut.ConnectSectionTitle, Is.EqualTo(expectedConnectSectionTitle));
        });
    }

    [TestCase(MemberUserType.Employer)]
    public void MemberProfileViewModel_UserRoleEmployer_ReturnsExpectedValueOfTitles(MemberUserType memberUserType)
    {
        //Arrange
        MemberProfileDetail memberProfile = new MemberProfileDetail();
        memberProfile.UserType = memberUserType;
        memberProfile.FirstName = "test";
        memberProfile.Profiles = new List<MemberProfile>();
        string expectedAreasOfInterestTitle = MemberProfileTitle.EmployerAreasOfInterestTitle;
        string expectedAreasOfInterestFirstSectionTitle = MemberProfileTitle.EmployerAreasOfInterestFirstSectionTitle;
        string expectedAreasOfInterestSecondSectionTitle = MemberProfileTitle.EmployerAreasOfInterestSecondSectionTitle;
        string expectedInformationSectionTitle = MemberProfileTitle.EmployerInformationSectionTitle;
        string expectedConnectSectionTitle = $"You can contact {memberProfile.FirstName} using the form below or connect directly on LinkedIn.";

        //Act
        MemberProfileViewModel sut = new MemberProfileViewModel(memberProfile, profiles, memberProfileMappingModel);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(sut.UserRole, Is.EqualTo(memberUserType));
            Assert.That(sut.AreasOfInterestTitle, Is.EqualTo(expectedAreasOfInterestTitle));
            Assert.That(sut.AreasOfInterestFirstSectionTitle, Is.EqualTo(expectedAreasOfInterestFirstSectionTitle));
            Assert.That(sut.AreasOfInterestSecondSectionTitle, Is.EqualTo(expectedAreasOfInterestSecondSectionTitle));
            Assert.That(sut.InformationSectionTitle, Is.EqualTo(expectedInformationSectionTitle));
            Assert.That(sut.ConnectSectionTitle, Is.EqualTo(expectedConnectSectionTitle));
        });
    }
}