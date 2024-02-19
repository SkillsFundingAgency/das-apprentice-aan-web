using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Aan.SharedUi.Constants;
using SFA.DAS.Aan.SharedUi.Infrastructure;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Extensions;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;

namespace SFA.DAS.ApprenticeAan.Web.Controllers;

[Authorize]
[Route("", Name = SharedRouteNames.Home)]
[Route("[controller]")]
public class HomeController(ISessionService sessionService) : Controller
{
    private readonly ISessionService _sessionService = sessionService;

    public IActionResult Index()
    {
        if (_sessionService.GetMemberId() == Guid.Empty)
        {
            return new RedirectToRouteResult(RouteNames.Onboarding.BeforeYouStart, null);
        }

        var status = _sessionService.GetMemberStatus();

        return status switch
        {
            MemberStatus.Withdrawn or MemberStatus.Deleted => new RedirectToRouteResult(SharedRouteNames.RejoinTheNetwork, null),
            MemberStatus.Removed => new RedirectToRouteResult(SharedRouteNames.RemovedShutter, null),
            _ => new RedirectToRouteResult(RouteNames.NetworkHub, null),
        };
    }
}

