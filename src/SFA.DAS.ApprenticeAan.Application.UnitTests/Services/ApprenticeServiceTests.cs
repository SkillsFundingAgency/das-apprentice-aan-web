using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using RestEase;
using SFA.DAS.ApprenticeAan.Application.Services;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Application.UnitTests.Services;

public class ApprenticeServiceTests
{
    [Test, MoqAutoData]
    public async Task GetApprentice_InvokesApiClient(
        [Frozen] Mock<IOuterApiClient> clientMock,
        ApprenticeService sut,
        Guid apprenticeId,
        Response<Apprentice?> apprenticeResponse)
    {
        apprenticeResponse.ResponseMessage.StatusCode = HttpStatusCode.OK;
        clientMock.Setup(c => c.GetApprentice(apprenticeId)).ReturnsAsync(apprenticeResponse);

        await sut.GetApprentice(apprenticeId);

        clientMock.Verify(c => c.GetApprentice(apprenticeId));
    }

    [Test, MoqAutoData]
    public async Task GetApprentice_FoundApprentice_ReturnsApprentice(
        [Frozen] Mock<IOuterApiClient> clientMock,
        ApprenticeService sut,
        Guid apprenticeId,
        Response<Apprentice?> apprenticeResponse)
    {
        apprenticeResponse.ResponseMessage.StatusCode = HttpStatusCode.OK;
        clientMock.Setup(c => c.GetApprentice(apprenticeId)).ReturnsAsync(apprenticeResponse);

        var result = await sut.GetApprentice(apprenticeId);

        result.Should().NotBeNull();
    }

    [Test, MoqAutoData]
    public async Task GetApprentice_NotFound_ReturnsNull(
        [Frozen] Mock<IOuterApiClient> clientMock,
        ApprenticeService sut,
        Guid apprenticeId,
        Response<Apprentice?> apprenticeResponse)
    {
        apprenticeResponse.ResponseMessage.StatusCode = HttpStatusCode.NotFound;
        clientMock.Setup(c => c.GetApprentice(apprenticeId)).ReturnsAsync(apprenticeResponse);

        var result = await sut.GetApprentice(apprenticeId);

        result.Should().BeNull();
    }

    [Test, MoqAutoData]
    public async Task GetApprentice_OuterApiUnsuccessfulResponse_ThrowsInvalidOperationException(
        ApprenticeService sut)
    {
        Func<Task> action = () => sut.GetApprentice(Guid.NewGuid());

        await action.Should().ThrowAsync<InvalidOperationException>();
    }
}
