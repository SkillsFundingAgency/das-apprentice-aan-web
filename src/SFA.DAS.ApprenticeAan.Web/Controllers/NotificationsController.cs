using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Aan.SharedUi.Infrastructure;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Extensions;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using static SFA.DAS.ApprenticeAan.Application.Constants;

namespace SFA.DAS.ApprenticeAan.Web.Controllers;

[Authorize]
public class NotificationsController : Controller
{
    private readonly IOuterApiClient _outerApiClient;

    public NotificationsController(IOuterApiClient outerApiClient)
    {
        _outerApiClient = outerApiClient;
    }

    [Route("links/{id}")]
    public async Task<IActionResult> Index(Guid id, CancellationToken cancellationToken)
    {
        var response = await _outerApiClient.GetNotification(User.GetAanMemberId(), id, cancellationToken);

        if (response.ResponseMessage.StatusCode != HttpStatusCode.OK) return RedirectToRoute(RouteNames.Home);

        var notification = response.GetContent()!;

        (string RouteName, object? RouteValues) route = notification.TemplateName switch
        {
            NotificationTemplateNames.AANApprenticeOnboarding
                => (RouteNames.NetworkHub, null),

            NotificationTemplateNames.AANApprenticeEventSignup
            or NotificationTemplateNames.AANAdminEventUpdate
            or NotificationTemplateNames.AANAdminEventCancel
                => (SharedRouteNames.NetworkEventDetails, new { id = notification.ReferenceId }),

            NotificationTemplateNames.AANApprenticeEventCancel
                => (SharedRouteNames.NetworkEvents, null),

            NotificationTemplateNames.AANIndustryAdvice
            or NotificationTemplateNames.AANAskForHelp
            or NotificationTemplateNames.AANRequestCaseStudy
            or NotificationTemplateNames.AANGetInTouch
                => (SharedRouteNames.MemberProfile, new { id = notification.ReferenceId }),

            _ => (RouteNames.Home, null)
        };

        return RedirectToRoute(route.RouteName, route.RouteValues);
    }
}
