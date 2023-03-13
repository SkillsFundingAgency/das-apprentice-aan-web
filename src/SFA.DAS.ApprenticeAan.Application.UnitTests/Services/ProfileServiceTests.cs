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
    public async Task Service_OuterApi_ReturnsProfiles(
        [Frozen] Mock<IOuterApiClient> _outerApiClient)
    {
        var result = await _outerApiClient.Object.GetProfiles();
        _outerApiClient.Verify(p => p.GetProfiles());

        Assert.That(result.Profiles, Is.Not.Null);
        Assert.That(result.Profiles.Count(), Is.GreaterThan(0));
        Assert.That(result.Profiles.FirstOrDefault().GetType().Name.Equals(nameof(Profile)));
    }
}