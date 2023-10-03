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

    [TestCase(null, "")]
    [TestCase("Multi-Regional", "Multi-Regional")]
    [MoqAutoData]
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