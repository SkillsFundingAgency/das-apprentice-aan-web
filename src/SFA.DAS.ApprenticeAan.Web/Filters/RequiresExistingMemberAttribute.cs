using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using SFA.DAS.Aan.SharedUi.Constants;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Controllers;
using SFA.DAS.ApprenticeAan.Web.Extensions;
using SFA.DAS.ApprenticeAan.Web.Helpers;
using SFA.DAS.ApprenticePortal.Authentication;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.ApprenticeAan.Web.Filters;

[ExcludeFromCodeCoverage]
public class RequiresExistingMemberAttribute : ApplicationFilterAttribute
{
    private readonly ISessionService _sessionService;
    private readonly IOuterApiClient _outerApiClient;
    private readonly IMemberService _memberService;
    public RequiresExistingMemberAttribute(ISessionService sessionService, IOuterApiClient outerApiClient, IMemberService memberService)
    {
        _sessionService = sessionService;
        _outerApiClient = outerApiClient;
        _memberService = memberService;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.ActionDescriptor is not ControllerActionDescriptor controllerActionDescriptor) return;

        if (BypassCheck(controllerActionDescriptor)) return;

        var isValidRequest = IsValidRequest(context, controllerActionDescriptor).Result;

        if (!isValidRequest)
        {
            context.Result = RedirectToHome;
        }
    }

    private bool BypassCheck(ControllerActionDescriptor controllerActionDescriptor)
    {
        var controllersToByPass = new[] { nameof(HomeController),
            nameof(LocationsController),
            nameof(AccessDeniedController),
            nameof(LeavingTheNetworkConfirmationController),
            nameof(RejoinTheNetworkController) };

        return controllersToByPass.Contains(controllerActionDescriptor.ControllerTypeInfo.Name);
    }

    private async Task<bool> IsValidRequest(ActionExecutingContext context, ControllerActionDescriptor controllerActionDescriptor)
    {
        var memberId = _sessionService.Get(Constants.SessionKeys.Member.MemberId);
        var isLive = _sessionService.GetMemberStatus() == MemberStatus.Live;

        if (memberId == null)
        {
            var response = _outerApiClient.GetApprentice(context.HttpContext.User.GetApprenticeId());

            if (response.Result.ResponseMessage.IsSuccessStatusCode)
            {
                var apprentice = response.Result.GetContent();
                memberId = apprentice!.MemberId.ToString().ToLower();
                _sessionService.Set(Constants.SessionKeys.Member.MemberId, memberId);
                var memberStatus = apprentice.Status;
                _sessionService.Set(Constants.SessionKeys.Member.Status, memberStatus);

                bool isClaimsMismatch = ClaimsHelper.IsClaimsMismatch(apprentice, context.HttpContext.User);

                if(isClaimsMismatch)
                {
                    await _memberService.UpdateMemberDetails(
                        apprentice.MemberId,
                        ClaimsHelper.GetMismatchValue(apprentice.FirstName, context.HttpContext.User, IdentityClaims.GivenName) ?? apprentice.FirstName,
                        ClaimsHelper.GetMismatchValue(apprentice.LastName, context.HttpContext.User, IdentityClaims.FamilyName) ?? apprentice.LastName,
                        context.HttpContext.RequestAborted
                    );
                }

                isLive = memberStatus == MemberStatus.Live.ToString();
            }
        }

        if (BypassCheck(controllerActionDescriptor)) return true;

        var isMember = memberId != null;

        var isRequestingOnboardingPage = IsRequestForOnboardingAction(controllerActionDescriptor);

        return (isMember && isLive && !isRequestingOnboardingPage)
               || (!isMember && isRequestingOnboardingPage);
    }
}
