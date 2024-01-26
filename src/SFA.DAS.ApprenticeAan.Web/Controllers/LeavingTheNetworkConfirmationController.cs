using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Aan.SharedUi.Infrastructure;
using SFA.DAS.Aan.SharedUi.Models.LeaveTheNetwork;

namespace SFA.DAS.ApprenticeAan.Web.Controllers;

[Authorize]
[Route("leaving-network-confirmation", Name = SharedRouteNames.LeaveTheNetworkComplete)]
public class LeavingTheNetworkConfirmationController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        var model = new LeaveTheNetworkConfirmedViewModel
        {
            HomeUrl = Url.RouteUrl(SharedRouteNames.Home)!
        };

        return View(model);
    }
}
