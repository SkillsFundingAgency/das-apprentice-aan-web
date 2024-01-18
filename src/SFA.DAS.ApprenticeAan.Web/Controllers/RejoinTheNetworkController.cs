using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Aan.SharedUi.Constants;
using SFA.DAS.Aan.SharedUi.Infrastructure;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;

namespace SFA.DAS.ApprenticeAan.Web.Controllers;

[Authorize]
public class RejoinTheNetworkController : Controller
{
    private readonly IOuterApiClient _apiClient;
    private readonly ISessionService _sessionService;

    public RejoinTheNetworkController(IOuterApiClient apiClient, ISessionService sessionService)
    {
        _apiClient = apiClient;
        _sessionService = sessionService;
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


        // if all goes well....
        _sessionService.Set(Constants.SessionKeys.MemberStatus, MemberStatus.Live);

        return RedirectToRoute(SharedRouteNames.Home);
    }
}
