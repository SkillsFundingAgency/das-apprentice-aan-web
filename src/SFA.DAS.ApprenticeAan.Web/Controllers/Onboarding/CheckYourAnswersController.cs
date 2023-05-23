using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Domain.Constants;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Requests;
using SFA.DAS.ApprenticeAan.Web.Extensions;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;
using SFA.DAS.ApprenticePortal.Authentication;
using static SFA.DAS.ApprenticeAan.Domain.OuterApi.Requests.CreateApprenticeMemberRequest;

namespace SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;

[Authorize]
[Route("onboarding/check-your-answers", Name = RouteNames.Onboarding.CheckYourAnswers)]
public class CheckYourAnswersController : Controller
{
    public const string ViewPath = "~/Views/Onboarding/CheckYourAnswers.cshtml";
    public const string ApplicationSubmittedViewPath = "~/Views/Onboarding/ApplicationSubmitted.cshtml";
    private readonly ISessionService _sessionService;
    private readonly IOuterApiClient _outerApiClient;

    public CheckYourAnswersController(ISessionService sessionService, IOuterApiClient outerApiClient)
    {
        _sessionService = sessionService;
        _outerApiClient = outerApiClient;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var sessionModel = _sessionService.Get<OnboardingSessionModel>();
        if (!sessionModel.HasSeenPreview)
        {
            sessionModel.HasSeenPreview = true;
            sessionModel.ApprenticeId = Guid.Parse(User.ApprenticeIdClaim()!.Value);
            sessionModel.MyApprenticeship = await _outerApiClient.GetMyApprenticeship(sessionModel.ApprenticeId);
            _sessionService.Set(sessionModel);
        }

        CheckYourAnswersViewModel model = new(Url, sessionModel, User);
        return View(ViewPath, model);
    }

    [HttpPost]
    public async Task<IActionResult> Post()
    {
        var onboardingSessionModel = _sessionService.Get<OnboardingSessionModel>();
        var result = await _outerApiClient.PostApprenticeMember(GenerateCreateApprenticeMemberRequest(onboardingSessionModel));

        User.AddAanMemberIdClaim(result.MemberId);

        await HttpContext.SignInAsync(User);

        return View(ApplicationSubmittedViewPath);
    }

    private CreateApprenticeMemberRequest GenerateCreateApprenticeMemberRequest(OnboardingSessionModel source)
    {
        CreateApprenticeMemberRequest request = new()
        {
            ApprenticeId = source.ApprenticeId,
            JoinedDate = DateTime.UtcNow,
            OrganisationName = source.GetProfileValue(ProfileDataId.EmployerName)!,
            RegionId = source.RegionId.GetValueOrDefault()
        };
        request.ProfileValues.AddRange(source.ProfileData.Where(p => !string.IsNullOrWhiteSpace(p.Value)).Select(p => new ProfileValue(p.Id, p.Value!)));
        request.Email = User.EmailAddressClaim()!.Value;
        request.FirstName = User.Claims.FirstOrDefault(c => c.Type == IdentityClaims.GivenName)!.Value;
        request.LastName = User.Claims.FirstOrDefault(c => c.Type == IdentityClaims.FamilyName)!.Value;
        return request;
    }
}
