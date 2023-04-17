using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Models;

[TestFixture]
public class SelectProfileModelTests
{
    [Test]
    [MoqAutoData]
    public void Apprentice_SelectProfileModelFromProfile(ProfileModel profiles)
    {
        var selectProfileData = (SelectProfileModel)profiles;

        Assert.Multiple(() =>
        {
            Assert.That(selectProfileData, Is.Not.Null);

            Assert.That(profiles!.Id, Is.EqualTo(selectProfileData.Id));
            Assert.That(profiles.Description, Is.EqualTo(selectProfileData.Description));
            Assert.That(profiles.Category, Is.EqualTo(selectProfileData.Category));
            Assert.That(profiles.Ordering, Is.EqualTo(selectProfileData.Ordering));
        });
    }
}