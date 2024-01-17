using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Aan.SharedUi.Infrastructure;
using SFA.DAS.Aan.SharedUi.Models.LeaveTheNetwork;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Requests;
using SFA.DAS.ApprenticeAan.Web.Extensions;
using SFA.DAS.ApprenticeAan.Web.Models;
using static SFA.DAS.ApprenticeAan.Web.Constants;

namespace SFA.DAS.ApprenticeAan.Web.Controllers;

[Authorize]
public class LeaveTheNetworkController : Controller
{
    private readonly IOuterApiClient _apiClient;
    private readonly ISessionService _sessionService;

    public const string LeaveTheNetworkViewPath = "~/Views/LeaveTheNetwork/Index.cshtml";
    public const string LeaveTheNetworkAreYouSureViewPath = "~/Views/LeaveTheNetwork/LeaveTheNetworkAreYouSure.cshtml";

    public LeaveTheNetworkController(IOuterApiClient apiClient, ISessionService sessionService)
    {
        _apiClient = apiClient;
        _sessionService = sessionService;
    }

    [HttpGet]
    [Route("leave-the-network", Name = SharedRouteNames.LeaveTheNetwork)]
    public async Task<IActionResult> Index()
    {
        var leavingReasons = await _apiClient.GetLeavingReasons();

        var viewModel = new LeaveTheNetworkViewModel
        {
            LeavingReasonsTitle = leavingReasons.First(x => x.Category.Contains("reasons")).Category,
            LeavingExperienceTitle = leavingReasons.First(x => x.Category.Contains("experience")).Category,
            LeavingBenefitsTitle = leavingReasons.First(x => x.Category.Contains("benefit")).Category,
            LeavingReasons = leavingReasons.First(x => x.Category.Contains("reasons")).LeavingReasons.OrderBy(x => x.Ordering).ToList(),
            LeavingExperience = leavingReasons.First(x => x.Category.Contains("experience")).LeavingReasons.OrderBy(x => x.Ordering).ToList(),
            LeavingBenefits = leavingReasons.First(x => x.Category.Contains("benefit")).LeavingReasons.OrderBy(x => x.Ordering).ToList(),
            ProfileSettingsLink = Url.RouteUrl(SharedRouteNames.ProfileSettings)!
        };

        return View(LeaveTheNetworkViewPath, viewModel);
    }

    [HttpPost]
    [Route("leave-the-network", Name = SharedRouteNames.LeaveTheNetwork)]
    public IActionResult Post(SubmitLeaveTheNetworkViewModel model)
    {
        List<int> reasonsTicked = model.LeavingReasons!.Where(x => x.IsSelected).Select(reason => reason.Id).ToList();
        reasonsTicked.AddRange(model.LeavingBenefits!.Where(x => x.IsSelected).Select(reason => reason.Id));

        if (model.LeavingExperienceRating != 0)
        {
            reasonsTicked.Add(model.LeavingExperienceRating);
            foreach (var reason in model.LeavingExperience!.Where(reason => reason.Id == model.LeavingExperienceRating))
            {
                reason.IsSelected = true;
            }
        }

        var sessionModel = new ReasonsForLeavingSessionModel
        {
            ReasonsForLeaving = reasonsTicked
        };
        _sessionService.Set(sessionModel);

        return RedirectToRoute(SharedRouteNames.LeaveTheNetworkConfirmation);
    }

    [HttpGet]
    [Route("leave-the-network-confirmation", Name = SharedRouteNames.LeaveTheNetworkConfirmation)]
    public IActionResult AreYouSure()
    {
        var viewModel = new LeaveTheNetworkAreYouSureViewModel
        {
            ProfileSettingsLink = Url.RouteUrl(SharedRouteNames.ProfileSettings)!
        };

        return View(LeaveTheNetworkAreYouSureViewPath, viewModel);
    }

    [HttpPost]
    [Route("leave-the-network-confirmation", Name = SharedRouteNames.LeaveTheNetworkConfirmation)]
    public async Task<IActionResult> AreYouSurePost(CancellationToken cancellationToken)
    {
        var sessionModel = _sessionService.Get<ReasonsForLeavingSessionModel>();

        await _apiClient.PostMemberLeaving(_sessionService.GetMemberId(),
            new MemberLeavingRequest { LeavingReasons = sessionModel.ReasonsForLeaving }, cancellationToken);

        _sessionService.Set(SessionKeys.MemberStatus, MemberStatus.Withdrawn);

        _sessionService.Delete<ReasonsForLeavingSessionModel>();

        return RedirectToRoute(SharedRouteNames.LeaveTheNetworkComplete);
    }
}