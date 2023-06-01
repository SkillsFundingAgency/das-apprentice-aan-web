using System.Security.Claims;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Moq;
using SFA.DAS.ApprenticeAan.Web.Authentication;
using SFA.DAS.ApprenticeAan.Web.Extensions;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Authentication;

public class StagedApprenticeAuthorizationHandlerTests
{
    [Test, MoqAutoData]
    public async Task Handle_UserIsNotStaged_Succeeds(
        StagedApprenticeRequirement requirement,
        StagedApprenticeAuthorizationHandler sut)
    {
        var httpContextMock = new Mock<HttpContext>();
        var response = new Mock<HttpResponse>();
        httpContextMock.Setup(c => c.Response).Returns(response.Object);
        AuthorizationHandlerContext context = new(new[] { requirement }, new(), httpContextMock.Object);

        await sut.HandleAsync(context);

        context.HasSucceeded.Should().BeTrue();
        response.Verify(x => x.Redirect(@"/accessdenied"));

    }

    [Test, MoqAutoData]
    public async Task Handle_UserIsStaged_Succeeds(
        StagedApprenticeRequirement requirement,
        StagedApprenticeAuthorizationHandler sut)
    {
        var httpContextMock = new Mock<HttpContext>();
        var response = new Mock<HttpResponse>();
        httpContextMock.Setup(c => c.Response).Returns(response.Object);
        ClaimsPrincipal claimsPrincipal = new();
        claimsPrincipal.AddStagedApprenticeClaim();
        claimsPrincipal.AddIdentity(new(new[] { new Claim(ClaimTypes.NameIdentifier, true.ToString()) }));
        AuthorizationHandlerContext context = new(new[] { requirement }, claimsPrincipal, httpContextMock.Object);

        await sut.HandleAsync(context);

        context.HasSucceeded.Should().BeTrue();
        response.Verify(x => x.Redirect(It.IsAny<string>()), Times.Never);
    }
}
