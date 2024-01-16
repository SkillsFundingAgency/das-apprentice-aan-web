using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Aan.SharedUi.Infrastructure;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Extensions;

namespace SFA.DAS.ApprenticeAan.Web.Controllers;
public class RejoinTheNetworkController : Controller
{
    private readonly IOuterApiClient _apiClient;

    public RejoinTheNetworkController(IOuterApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public const string LeaveTheNetworkViewPath = "~/Views/RejoinTheNetwork/Index.cshtml";

    [HttpGet]
    [Route("rejoin-the-network", Name = SharedRouteNames.RejoinTheNetwork)]
    public IActionResult Index()
    {
        return View(LeaveTheNetworkViewPath);
    }

    [HttpPost]
    [Route("rejoin-the-network", Name = SharedRouteNames.RejoinTheNetwork)]
    public async Task<IActionResult> Post()
    {
        // update record

        User.AddAanMemberIdClaim(User.GetAanMemberId());
        await HttpContext.SignInAsync(User);

        return RedirectToRoute(SharedRouteNames.Home);
    }
}
