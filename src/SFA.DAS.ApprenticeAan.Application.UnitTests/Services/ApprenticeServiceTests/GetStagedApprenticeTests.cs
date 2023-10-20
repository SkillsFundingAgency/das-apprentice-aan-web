using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using RestEase;
using SFA.DAS.ApprenticeAan.Application.Services;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Requests;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Application.UnitTests.Services.ApprenticeServiceTests;

public class GetStagedApprenticeTests
{
    [Test, MoqAutoData]
    public async Task GetStagedApprentice_InvokesApiClient(
        [Frozen] Mock<IOuterApiClient> clientMock,
        ApprenticeService sut,
        GetStagedApprenticeRequest request,
        DateTime dateOfBirth,
        Response<StagedApprentice?> stagedApprenticeResponse,
        CancellationToken cancellationToken)
    {
        request = request with { DateOfBirth = dateOfBirth.ToString("yyyy-MM-dd") };

        stagedApprenticeResponse.ResponseMessage.StatusCode = HttpStatusCode.OK;
        clientMock.Setup(c => c.GetStagedApprentice(request, cancellationToken)).ReturnsAsync(stagedApprenticeResponse);

        await sut.GetStagedApprentice(request.LastName, dateOfBirth.Date, request.Email, cancellationToken);

        clientMock.Verify(c => c.GetStagedApprentice(request, cancellationToken));
    }

    [Test, MoqAutoData]
    public async Task GetStagedApprentice_FoundApprentice_ReturnsApprentice(
        [Frozen] Mock<IOuterApiClient> clientMock,
        ApprenticeService sut,
        GetStagedApprenticeRequest request,
        DateTime dateOfBirth,
        Response<StagedApprentice?> stagedApprenticeResponse,
        CancellationToken cancellationToken)
    {
        request = request with { DateOfBirth = dateOfBirth.ToString("yyyy-MM-dd") };
        stagedApprenticeResponse.ResponseMessage.StatusCode = HttpStatusCode.OK;
        clientMock.Setup(c => c.GetStagedApprentice(request, cancellationToken)).ReturnsAsync(stagedApprenticeResponse);

        var result = await sut.GetStagedApprentice(request.LastName, dateOfBirth.Date, request.Email, cancellationToken);

        result.Should().NotBeNull();
    }

    [Test, MoqAutoData]
    public async Task GetStagedApprentice_NotFound_ReturnsNull(
        [Frozen] Mock<IOuterApiClient> clientMock,
        ApprenticeService sut,
        GetStagedApprenticeRequest request,
        DateTime dateOfBirth,
        Response<StagedApprentice?> stagedApprenticeResponse,
        CancellationToken cancellationToken)
    {
        request = request with { DateOfBirth = dateOfBirth.ToString("yyyy-MM-dd") };
        stagedApprenticeResponse.ResponseMessage.StatusCode = HttpStatusCode.NotFound;
        clientMock.Setup(c => c.GetStagedApprentice(request, cancellationToken)).ReturnsAsync(stagedApprenticeResponse);

        var result = await sut.GetStagedApprentice(request.LastName, dateOfBirth.Date, request.Email, cancellationToken);

        result.Should().BeNull();
    }

    [Test, MoqAutoData]
    public async Task GetStagedApprentice_OuterApiUnsuccessfulResponse_ThrowsInvalidOperationException(
        ApprenticeService sut,
        string lastName, DateTime dateOfBirth, string email,
        CancellationToken cancellationToken)
    {
        Func<Task> action = () => sut.GetStagedApprentice(lastName, dateOfBirth, email, cancellationToken);

        await action.Should().ThrowAsync<InvalidOperationException>();
    }
}
