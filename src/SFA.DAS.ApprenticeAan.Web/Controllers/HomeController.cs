using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Aan.SharedUi.Constants;
using SFA.DAS.Aan.SharedUi.Infrastructure;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Configuration;
using SFA.DAS.ApprenticeAan.Web.Extensions;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticePortal.Authentication;
using SFA.DAS.GovUK.Auth.Services;

namespace SFA.DAS.ApprenticeAan.Web.Controllers;

[Authorize]
[Route("", Name = SharedRouteNames.Home)]
[Route("[controller]")]
public class HomeController : Controller
{
    private readonly ISessionService _sessionService;
    private readonly IOidcService _oidcService;
    private readonly ApplicationConfiguration _configuration;
    private readonly IApprenticeAccountProvider _apprenticeAccountProvider;

    public HomeController(ISessionService sessionService, ApplicationConfiguration configuration, IOidcService oidcService, IApprenticeAccountProvider apprenticeAccountProvider)
    {
        _sessionService = sessionService;
        _oidcService = oidcService;
        _configuration = configuration;
        _apprenticeAccountProvider = apprenticeAccountProvider;
    }

    public async Task<IActionResult> Index()
    {
        if (_configuration is { UseGovSignIn: true, StubAuth: false })
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            var govUkUser = await _oidcService.GetAccountDetails(token);
            var loggedInUserEmail = User.EmailAddressClaim()!.Value;
            if (!govUkUser.Email.Equals(loggedInUserEmail, StringComparison.CurrentCultureIgnoreCase))
            {
                await _apprenticeAccountProvider.PutApprenticeAccount(govUkUser.Email, govUkUser.Sub);
            }
        }
        
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

