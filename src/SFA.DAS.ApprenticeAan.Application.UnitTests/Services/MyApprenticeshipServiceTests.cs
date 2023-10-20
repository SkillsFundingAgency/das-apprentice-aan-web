using System.Net;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using RestEase;
using SFA.DAS.ApprenticeAan.Application.Services;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Requests;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Application.UnitTests.Services;

public class MyApprenticeshipServiceTests
{
    [Test, MoqAutoData]
    public async Task TryCreateMyApprenticeship_MyApprenticeshipExists_DoesNotCreateNew(
        Guid apprenticeId,
        MyApprenticeship myApprenticeship,
        string lastName,
        string email,
        DateTime dateOfBirth,
        CancellationToken cancellationToken)
    {
        Mock<IOuterApiClient> clientMock = new();

        Response<MyApprenticeship> myApprenticeshipResponse = new(string.Empty, new(HttpStatusCode.OK), () => myApprenticeship);
        clientMock.Setup(c => c.GetMyApprenticeship(apprenticeId, cancellationToken)).ReturnsAsync(myApprenticeshipResponse);

        MyApprenticeshipService sut = new(clientMock.Object, Mock.Of<ILogger<MyApprenticeshipService>>());

        var result = await sut.TryCreateMyApprenticeship(apprenticeId, lastName, email, dateOfBirth, cancellationToken);

        result.Should().Be(myApprenticeship);
        clientMock.Verify(o => o.GetMyApprenticeship(apprenticeId, cancellationToken), Times.Once);
        clientMock.Verify(o => o.GetStagedApprentice(It.IsAny<GetStagedApprenticeRequest>(), It.IsAny<CancellationToken>()), Times.Never);
        clientMock.Verify(o => o.CreateMyApprenticeship(It.IsAny<CreateMyApprenticeshipRequest>(), cancellationToken), Times.Never);
    }

    [Test, MoqAutoData]
    public async Task TryCreateMyApprenticeship_MyApprenticeshipDoesNotExists_CreatesNew(
        Guid apprenticeId,
        MyApprenticeship myApprenticeship,
        GetStagedApprenticeRequest request,
        DateTime dateOfBirth,
        CancellationToken cancellationToken)
    {
        request = request with { DateOfBirth = dateOfBirth.ToString("yyyy-MM-dd") };
        Mock<IOuterApiClient> clientMock = new();

        Response<MyApprenticeship> myApprenticeshipNotFoundResponse = new(string.Empty, new(HttpStatusCode.NotFound), () => null!);
        Response<MyApprenticeship> myApprenticeshipOkResponse = new(string.Empty, new(HttpStatusCode.OK), () => myApprenticeship);
        clientMock.SetupSequence(c => c.GetMyApprenticeship(apprenticeId, cancellationToken))
            .ReturnsAsync(myApprenticeshipNotFoundResponse)
            .ReturnsAsync(myApprenticeshipOkResponse);

        Response<StagedApprentice?> stagedApprenticeOkResponse = new(string.Empty, new(HttpStatusCode.OK), () => new StagedApprentice());
        clientMock.Setup(c => c.GetStagedApprentice(request, cancellationToken)).ReturnsAsync(stagedApprenticeOkResponse);

        MyApprenticeshipService sut = new(clientMock.Object, Mock.Of<ILogger<MyApprenticeshipService>>());

        var result = await sut.TryCreateMyApprenticeship(apprenticeId, request.LastName, request.Email, dateOfBirth.Date, cancellationToken);

        result.Should().Be(myApprenticeship);
        clientMock.Verify(o => o.GetMyApprenticeship(apprenticeId, cancellationToken), Times.Exactly(2));
        clientMock.Verify(o => o.GetStagedApprentice(It.IsAny<GetStagedApprenticeRequest>(), It.IsAny<CancellationToken>()));
        clientMock.Verify(o => o.CreateMyApprenticeship(It.IsAny<CreateMyApprenticeshipRequest>(), cancellationToken));
    }
}
