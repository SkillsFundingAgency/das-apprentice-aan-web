using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Aan.SharedUi.Infrastructure;
using SFA.DAS.Aan.SharedUi.Models.LeaveTheNetwork;

namespace SFA.DAS.ApprenticeAan.Web.Controllers;

[Authorize]
[Route("leaving-the-network-complete", Name = SharedRouteNames.LeaveTheNetworkComplete)]
public class LeavingTheNetworkConfirmationController : Controller
{
    public const string LeaveTheNetworkConfirmedViewPath = "~/Views/LeaveTheNetwork/LeavingConfirmed.cshtml";

    [HttpGet]
    public IActionResult LeavingNetworkComplete()
    {
        var model = new LeaveTheNetworkConfirmedViewModel
        {
            HomeUrl = Url.RouteUrl(SharedRouteNames.Home)!
        };

        return View(LeaveTheNetworkConfirmedViewPath, model);
    }
}
