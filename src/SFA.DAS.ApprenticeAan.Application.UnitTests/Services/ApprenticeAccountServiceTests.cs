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
public class ApprenticeAccountServiceTests
{
    [Test]
    [MoqAutoData]
    public async Task GetApprenticeAccountDetails_OuterApiSuccess_ReturnsDetails(
        [Frozen] Mock<IOuterApiClient> outerApiClientMock,
        ApprenticeAccountService sut,
        Guid apprenticeId,
        Response<ApprenticeAccount?> apprenticeAccountResponse)
    {
        apprenticeAccountResponse.ResponseMessage.StatusCode = HttpStatusCode.OK;
        outerApiClientMock.Setup(o => o.GetApprenticeAccount(apprenticeId, It.IsAny<CancellationToken>())).ReturnsAsync(apprenticeAccountResponse);

        var actual = await sut.GetApprenticeAccountDetails(apprenticeId);

        actual.Should().BeEquivalentTo(apprenticeAccountResponse.GetContent());
    }

    [Test]
    [MoqAutoData]
    public async Task GetApprenticeAccountDetails_OuterApiNotFoundResponse_ReturnsNull(
        [Frozen] Mock<IOuterApiClient> outerApiClientMock,
        ApprenticeAccountService sut,
        Guid apprenticeId,
        Response<ApprenticeAccount?> apprenticeAccountResponse)
    {
        apprenticeAccountResponse.ResponseMessage.StatusCode = HttpStatusCode.NotFound;
        outerApiClientMock.Setup(o => o.GetApprenticeAccount(apprenticeId, It.IsAny<CancellationToken>())).ReturnsAsync(apprenticeAccountResponse);

        var actual = await sut.GetApprenticeAccountDetails(apprenticeId);

        actual.Should().BeNull();
    }

    [Test]
    [MoqAutoData]
    public async Task GetApprenticeAccountDetails_OuterApiUnsuccessfulResponse_ThrowsInvalidOperationException(
        [Frozen] Mock<IOuterApiClient> outerApiClientMock,
        ApprenticeAccountService sut,
        Guid apprenticeId,
        Response<ApprenticeAccount?> apprenticeAccountResponse)
    {
        apprenticeAccountResponse.ResponseMessage.StatusCode = HttpStatusCode.InternalServerError;
        outerApiClientMock.Setup(o => o.GetApprenticeAccount(apprenticeId, It.IsAny<CancellationToken>())).ReturnsAsync(apprenticeAccountResponse);

        Func<Task> action = () => sut.GetApprenticeAccountDetails(apprenticeId);

        await action.Should().ThrowAsync<InvalidOperationException>();
    }
}
