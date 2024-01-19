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
public class HomeController : Controller
{
    private readonly ISessionService _sessionService;

    public HomeController(ISessionService sessionService)
    {
        _sessionService = sessionService;
    }

    public IActionResult Index()
    {
        if (_sessionService.GetMemberId() == Guid.Empty)
        {
            return new RedirectToRouteResult(RouteNames.Onboarding.BeforeYouStart, null);
        }

        var status = _sessionService.GetMemberStatus();

        if (status == MemberStatus.Withdrawn || status == MemberStatus.Deleted)
        {
            return new RedirectToRouteResult(SharedRouteNames.RejoinTheNetwork, null);
        }

        return new RedirectToRouteResult(RouteNames.NetworkHub, null);

    }
}

