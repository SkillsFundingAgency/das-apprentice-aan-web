using AutoFixture.NUnit3;
using FluentAssertions;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Requests;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses;

namespace SFA.DAS.ApprenticeAan.Domain.UnitTests.OuterApi.Requests;

public class CreateMyApprenticeshipRequestTests
{

    [Test, AutoData]
    public void Operator_GivenStagedApprentice_ReturnsRequest(StagedApprentice source)
    {
        CreateMyApprenticeshipRequest sut = source;

        sut.Should().BeEquivalentTo(source, options => options.ExcludingMissingMembers());
    }
}