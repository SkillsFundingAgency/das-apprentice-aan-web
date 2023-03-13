using AutoFixture.NUnit3;
using Moq;
using SFA.DAS.ApprenticeAan.Application.Services;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Application.UnitTests.Services;

[TestFixture]
public class ProfileServiceTests
{
    [MoqAutoData]
    public async Task WhenGetProfile_OuterApi_ReturnsProfiles(
        [Frozen] Mock<IOuterApiClient> _outerApiClient)
    {
        var result = await _outerApiClient.Object.GetProfiles();

        Assert.That(result.Profiles, Is.Not.Null);
    }

    [MoqAutoData]
    public async Task WhenGetProfile_ProfileService_ReturnsProfiles(
        [Frozen] Mock<IProfileService> _profileService)
    {
        var profiles = await _profileService.Object.GetProfiles();
        _profileService.Verify(p => p.GetProfiles());

        Assert.That(profiles, Is.Not.Null);
    }
}