using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models;

namespace SFA.DAS.ApprenticeAan.Web.Controllers;

[Authorize]
[Route("network-hub", Name = RouteNames.NetworkHub)]
public class NetworkHubController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View(new NetworkHubViewModel());
    }
}
