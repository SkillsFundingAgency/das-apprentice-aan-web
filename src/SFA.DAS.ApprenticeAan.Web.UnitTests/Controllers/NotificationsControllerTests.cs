using System.Net;
using System.Text.Json;
using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RestEase;
using SFA.DAS.Aan.SharedUi.Infrastructure;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses;
using SFA.DAS.ApprenticeAan.Web.Controllers;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticePortal.Authentication.TestHelpers;
using static SFA.DAS.ApprenticeAan.Application.Constants;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers;

public class NotificationsControllerTests
{
    private Mock<IOuterApiClient> outerApiClientMock = null!;

    [Test]
    public async Task Index_OnFailureToGetNotification_ReturnsHomeRoute()
    {
        var sut = GetSut(null);

        var result = await sut.Index(Guid.NewGuid(), CancellationToken.None);

        result.As<RedirectToRouteResult>().RouteName.Should().Be(RouteNames.Home);
    }

    [Test]
    [InlineAutoData(NotificationTemplateNames.AANApprenticeOnboarding, RouteNames.NetworkHub)]
    [InlineAutoData(NotificationTemplateNames.AANApprenticeEventSignup, SharedRouteNames.NetworkEventDetails)]
    [InlineAutoData(NotificationTemplateNames.AANAdminEventUpdate, SharedRouteNames.NetworkEventDetails)]
    [InlineAutoData(NotificationTemplateNames.AANAdminEventCancel, SharedRouteNames.NetworkEventDetails)]
    [InlineAutoData(NotificationTemplateNames.AANApprenticeEventCancel, SharedRouteNames.NetworkEvents)]
    [InlineAutoData(NotificationTemplateNames.AANIndustryAdvice, SharedRouteNames.MemberProfile)]
    [InlineAutoData(NotificationTemplateNames.AANAskForHelp, SharedRouteNames.MemberProfile)]
    [InlineAutoData(NotificationTemplateNames.AANRequestCaseStudy, SharedRouteNames.MemberProfile)]
    [InlineAutoData(NotificationTemplateNames.AANGetInTouch, SharedRouteNames.MemberProfile)]
    [InlineAutoData("unknown template", RouteNames.Home)]
    public async Task Index_OnAANApprenticeOnboardingTemplate_ReturnsNetworkHubRoute(string templateName, string routeName, GetNotificationResult result)
    {
        result.TemplateName = templateName;
        var sut = GetSut(result);

        var actual = await sut.Index(Guid.NewGuid(), CancellationToken.None);

        actual.As<RedirectToRouteResult>().RouteName.Should().Be(routeName);
    }

    private NotificationsController GetSut(GetNotificationResult? getNotificationResult)
    {
        var user = AuthenticatedUsersForTesting.FakeLocalUserFullyVerifiedClaim(Guid.NewGuid());

        outerApiClientMock = new();

        var response = GetOuterApiResponse(getNotificationResult);
        outerApiClientMock
            .Setup(o => o.GetNotification(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var sut = new NotificationsController(outerApiClientMock.Object);
        sut.ControllerContext = new() { HttpContext = new DefaultHttpContext() { User = user } };
        return sut;
    }

    private static Response<GetNotificationResult?> GetOuterApiResponse(GetNotificationResult? getNotificationResult) =>
        getNotificationResult == null
            ? new Response<GetNotificationResult?>(null, new(HttpStatusCode.BadRequest), () => null)
            : new Response<GetNotificationResult?>(JsonSerializer.Serialize(getNotificationResult), new(HttpStatusCode.OK), () => getNotificationResult);
}
