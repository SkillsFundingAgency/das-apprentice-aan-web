using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.ApprenticeAan.Application.Services;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Application.UnitTests.Services;

[TestFixture]
public class ProfileServiceTests
{

    [MoqAutoData]
    public async Task Service_ProfileData_ReturnsProfiles(
        [Frozen] Mock<IOuterApiClient> _outerApiClient)
    {
        const string userType = "apprentice";
        var service = new ProfileService(_outerApiClient.Object);
        var profiles = await service.GetProfilesByUserType(userType);

        var result = await _outerApiClient.Object.GetProfilesByUserType(userType);

        Assert.Multiple(() =>
        {
            Assert.That(profiles, Is.Not.Null);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Profiles, Is.Not.Null);
        });
        result.Profiles.Should().BeEquivalentTo(profiles);
    }
}