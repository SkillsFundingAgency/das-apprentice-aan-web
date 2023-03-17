using SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Models;

[TestFixture]
public class ProfileModelTests
{
    [Test]
    [MoqAutoData]
    public void Apprentice_ProfileModelFromProfile(Profile profiles)
    {
        var profileData = (ProfileModel)profiles;

        Assert.Multiple(() =>
        {
            Assert.That(profileData, Is.Not.Null);

            Assert.That(profiles!.Id, Is.EqualTo(profileData.Id));
            Assert.That(profiles.Description, Is.EqualTo(profileData.Description));
            Assert.That(profiles.Category, Is.EqualTo(profileData.Category));
            Assert.That(profiles.Ordering, Is.EqualTo(profileData.Ordering));
        });
    }
}