using AutoFixture.NUnit3;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.Aan.SharedUi.Infrastructure;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Controllers;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers;
public class RejoinTheNetworkControllerTests
{
    [Test]
    public void WhenGettingRejoinTheNetwork_ReturnsViewResult()
    {
        RejoinTheNetworkController sut = new(Mock.Of<IOuterApiClient>(), Mock.Of<ISessionService>());
        var response = sut.Index();
        Assert.That(response, Is.InstanceOf<ViewResult>());
    }

    [Test, RecursiveMoqAutoData]
    public async Task WhenPostingRejoinTheNetwork_RedirectToHome(
        [Frozen] Mock<IOuterApiClient> outerApiClientMock,
        [Frozen] Mock<ISessionService> sessionServiceMock,
        Guid memberId,
        CancellationToken cancellationToken
    )
    {
        sessionServiceMock.Setup(x => x.Get(Constants.SessionKeys.Member.MemberId)).Returns(memberId.ToString);
        RejoinTheNetworkController sut = new(outerApiClientMock.Object, sessionServiceMock.Object);

        var response = await sut.Post(cancellationToken);

        outerApiClientMock.Verify(x => x.PostMemberReinstate(memberId, cancellationToken), Times.Once);
        sessionServiceMock.Verify(x => x.Delete(Constants.SessionKeys.Member.MemberId), Times.Once);

        Assert.Multiple(() =>
        {
            Assert.That(response, Is.TypeOf<RedirectToRouteResult>());
            var redirectToAction = (RedirectToRouteResult)response;
            Assert.That(redirectToAction.RouteName, Does.Contain(SharedRouteNames.Home));
        });
    }
}
