using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Aan.SharedUi.Infrastructure;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Extensions;

namespace SFA.DAS.ApprenticeAan.Web.Controllers;

[Authorize]
public class RejoinTheNetworkController(IOuterApiClient apiClient, ISessionService sessionService) : Controller
{
    private readonly IOuterApiClient _apiClient = apiClient;
    private readonly ISessionService _sessionService = sessionService;

    [HttpGet]
    [Route("rejoin-the-network", Name = SharedRouteNames.RejoinTheNetwork)]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    [Route("rejoin-the-network", Name = SharedRouteNames.RejoinTheNetwork)]
    public async Task<IActionResult> Post(CancellationToken cancellationToken)
    {
        await _apiClient.PostMemberReinstate(_sessionService.GetMemberId(), cancellationToken);
        _sessionService.Clear();
        return RedirectToRoute(SharedRouteNames.Home);
    }
}