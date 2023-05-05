using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticePortal.Authentication;

namespace SFA.DAS.ApprenticeAan.Web.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly IApprenticeService _apprenticeService;
    private readonly AuthenticatedUser _authenticatedUser;

    public HomeController(IApprenticeService apprenticeService, AuthenticatedUser authenticatedUser)
    {
        _apprenticeService = apprenticeService;
        _authenticatedUser = authenticatedUser;
    }

    public async Task<IActionResult> Index()
    {
        var apprentice = await _apprenticeService.GetApprentice(_authenticatedUser.ApprenticeId);

        if (apprentice == null)
        {
            return new RedirectToRouteResult(RouteNames.Onboarding.BeforeYouStart, null);
        }
        else
        {
            return new RedirectToRouteResult(RouteNames.NetworkHub, null);
        }
    }
}
