using SFA.DAS.Aan.SharedUi.Constants;
using SFA.DAS.Aan.SharedUi.Infrastructure;
using SFA.DAS.Aan.SharedUi.Models;
using SFA.DAS.Aan.SharedUi.Models.AmbassadorProfile;
using SFA.DAS.Testing.AutoFixture;
using static SFA.DAS.Aan.SharedUi.Constants.PreferenceConstants;
using static SFA.DAS.Aan.SharedUi.Constants.ProfileConstants;

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
        memberProfileMappingModel = new MemberProfileMappingModel()
        {
            EmployerNameProfileId = 30,
            AddressProfileIds = new List<int> { 31, 32, 33 }
        };
        profiles = new List<Profile>()
        {
            new Profile { Id = 2, Description = "Presenting at events in person", Category = "Events", Ordering = 2 },
            new Profile { Id = 10, Description = "Carrying out and writing up case studies", Category = "Promotions", Ordering = 3 },
            new Profile { Id = 11, Description = "Designing and creating marketing materials to champion the network", Category = "Promotions", Ordering = 4 },
            new Profile { Id = 1, Description = "Networking at events in person", Category = "Events", Ordering = 1 },
        };
    }

    [Test, MoqAutoData]
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

    [Test, MoqAutoData]
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

    [Test, MoqAutoData]
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

    [Test, MoqAutoData]
    public void MemberProfileViewModel_SetAddressValue_ReturnsExpectedValue(MemberProfileDetail memberProfile)
    {
        //Arrange
        List<MemberProfile> memberProfiles = new List<MemberProfile>()
        {
            new MemberProfile() {ProfileId = 1,PreferenceId = 0,Value = "Address1"},
            new MemberProfile() {ProfileId = 2,PreferenceId = 0,Value = string.Empty},
            new MemberProfile() {ProfileId = 3,PreferenceId = 0,Value = "Address2"},
            new MemberProfile() {ProfileId = 4,PreferenceId = 0,Value = "    "}
        };
        MemberProfileMappingModel memberProfileMappingModel = new MemberProfileMappingModel()
        {
            AddressProfileIds = new List<int> { 1, 2, 4, 5 }
        };
        memberProfile.Profiles = memberProfiles;

        //Act
        MemberProfileViewModel sut = new MemberProfileViewModel(memberProfile, profiles, memberProfileMappingModel);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(sut.Address, Is.Not.Empty);
            Assert.That(sut.Address, Is.EqualTo(memberProfiles.First().Value));
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

    [TestCase(MemberUserType.Employer, true)]
    [TestCase(MemberUserType.Employer, false)]
    [TestCase(MemberUserType.Apprentice, true)]
    [TestCase(MemberUserType.Apprentice, false)]
    public void MemberProfileViewModel_IsApprenticeshipInformationAvailable_ReturnsExpectedBooleanValue(MemberUserType memberUserType, bool isApprenticeshipSet)
    {
        //Arrange
        MemberProfileDetail memberProfile = new MemberProfileDetail();
        memberProfile.UserType = memberUserType;
        memberProfile.Profiles = new List<MemberProfile>();
        if (memberUserType == MemberUserType.Employer && isApprenticeshipSet)
        {
            memberProfile.ActiveApprenticesCount = 1;
            memberProfile.Sectors = new List<string> { "test1", "test2" };
        }
        else
        {
            memberProfile.ActiveApprenticesCount = 0;
            memberProfile.Sectors = new List<string>();
        }

        if (memberUserType == MemberUserType.Apprentice && isApprenticeshipSet)
        {
            memberProfile.Sector = "Sector";
            memberProfile.Programmes = "Programmes";
            memberProfile.Level = "Level";
        }
        else
        {
            memberProfile.Sector = string.Empty;
            memberProfile.Programmes = string.Empty;
            memberProfile.Level = string.Empty;
        }

        //Act
        MemberProfileViewModel sut = new MemberProfileViewModel(memberProfile, profiles, memberProfileMappingModel);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(sut.IsApprenticeshipInformationAvailable, Is.EqualTo(memberUserType == MemberUserType.Employer && isApprenticeshipSet || memberUserType == MemberUserType.Apprentice && isApprenticeshipSet));
        });
    }

    [TestCase(true)]
    [TestCase(false)]
    public void MemberProfileViewModel_IsEmployerInformationAvailable_ReturnsExpectedBooleanValue(bool isEmployerInformationAvailable)
    {
        //Arrange
        List<Profile> profiles = new List<Profile>()
        {
            new Profile { Id = 30, Description = "Employer name", Category = "Employer", Ordering = 1 },
            new Profile { Id = 31, Description = "Address 1", Category = "Employer", Ordering = 2 },
            new Profile { Id = 32, Description = "Address 2", Category = "Employer", Ordering = 3 },
        };
        List<MemberProfile> memberProfiles = new List<MemberProfile>()
        {
            new MemberProfile() {ProfileId = 30,PreferenceId = 0,Value = "Employer name"},
            new MemberProfile() {ProfileId = 31,PreferenceId = 0,Value = "Address1"},
            new MemberProfile() {ProfileId = 32,PreferenceId = 0,Value = "Address2"}
        };
        MemberProfileDetail memberProfile = new MemberProfileDetail();
        memberProfile.Profiles = new List<MemberProfile>();
        if (isEmployerInformationAvailable)
        {
            memberProfile.Profiles = memberProfiles;
        }
        else
        {
            memberProfile.Profiles = new List<MemberProfile>();
        }

        //Act
        MemberProfileViewModel sut = new MemberProfileViewModel(memberProfile, profiles, memberProfileMappingModel);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(sut.IsEmployerInformationAvailable, Is.EqualTo(isEmployerInformationAvailable));
        });
    }

    [Test]
    public void MemberProfileViewModel_ReturnsExpectedNetworkJoinValue()
    {
        //Arrange
        List<Profile> profiles = new List<Profile>();
        MemberProfileDetail memberProfile = new MemberProfileDetail();
        memberProfile.Profiles = new List<MemberProfile>();

        //Act
        MemberProfileViewModel sut = new MemberProfileViewModel(memberProfile, profiles, memberProfileMappingModel);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(sut.ReasonToGetInTouch, Is.EqualTo(0));
            Assert.That(sut.HasAgreedToCodeOfConduct, Is.EqualTo(false));
            Assert.That(sut.HasAgreedToSharePersonalDetails, Is.EqualTo(false));
        });
    }


    [Test, MoqAutoData]
    public void MemberProfileViewModel_SetLinkedinUrl_ReturnsExpectedValue(MemberProfileDetail memberProfile, string linkedinUrl)
    {
        // Arrange
        memberProfile.Profiles = new List<MemberProfile>()
        {
            new MemberProfile(){ProfileId=ProfileIds.LinkedIn,Value=linkedinUrl,PreferenceId=PreferenceIds.LinkedIn}
        };
        memberProfileMappingModel.LinkedinProfileId = ProfileIds.LinkedIn;

        // Act
        MemberProfileViewModel sut = new MemberProfileViewModel(memberProfile, profiles, memberProfileMappingModel);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(sut.LinkedinUrl, Is.Not.Null);
            Assert.That(sut.LinkedinUrl, Is.EqualTo(UrlConstant.LinkedinUrl + linkedinUrl));
        });
    }

    [Test, MoqAutoData]
    public void MemberProfileViewModel_SetLinkedinUrlEmpty_ReturnsEmptyString(MemberProfileDetail memberProfile)
    {
        // Arrange
        memberProfile.Profiles = new List<MemberProfile>()
        {
            new MemberProfile(){ProfileId=ProfileIds.LinkedIn,Value=string.Empty,PreferenceId=PreferenceIds.LinkedIn}
        };
        memberProfileMappingModel.LinkedinProfileId = ProfileIds.LinkedIn;

        // Act
        MemberProfileViewModel sut = new MemberProfileViewModel(memberProfile, profiles, memberProfileMappingModel);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(sut.LinkedinUrl, Is.Not.Null);
            Assert.That(sut.LinkedinUrl, Is.EqualTo(string.Empty));
        });
    }
}