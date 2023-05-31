using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Web.Extensions;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;

namespace SFA.DAS.ApprenticeAan.Web.Controllers;

[Authorize]
public class HomeController : Controller
{
    public IActionResult Index()
    {
        if (User.GetAanMemberId() == Guid.Empty)
        {
            return new RedirectToRouteResult(RouteNames.Onboarding.BeforeYouStart, null);
        }
        else
        {
            return new RedirectToRouteResult(RouteNames.NetworkHub, null);
        }
    }
}
