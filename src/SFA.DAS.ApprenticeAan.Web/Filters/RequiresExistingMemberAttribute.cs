using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Controllers;
using SFA.DAS.ApprenticeAan.Web.Extensions;

namespace SFA.DAS.ApprenticeAan.Web.Filters;

[ExcludeFromCodeCoverage]
public class RequiresExistingMemberAttribute : ApplicationFilterAttribute
{
    private readonly ISessionService _sessionService;
    private readonly IOuterApiClient _outerApiClient;

    public RequiresExistingMemberAttribute(ISessionService sessionService, IOuterApiClient outerApiClient)
    {
        _sessionService = sessionService;
        _outerApiClient = outerApiClient;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.ActionDescriptor is not ControllerActionDescriptor controllerActionDescriptor) return;

        if (BypassCheck(controllerActionDescriptor)) return;

        if (!IsValidRequest(context, controllerActionDescriptor))
        {
            context.Result = RedirectToHome;
        }
    }

    private bool BypassCheck(ControllerActionDescriptor controllerActionDescriptor)
    {
        var controllersToByPass = new[] { nameof(HomeController), nameof(LocationsController), nameof(AccessDeniedController), nameof(LeavingTheNetworkConfirmationController) };

        return controllersToByPass.Contains(controllerActionDescriptor.ControllerTypeInfo.Name);
    }

    private bool IsValidRequest(ActionExecutingContext context, ControllerActionDescriptor controllerActionDescriptor)
    {
        var memberId = _sessionService.Get(Constants.SessionKeys.Member.MemberId);
        var isLive = _sessionService.IsMemberLive();

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
                isLive = _sessionService.IsMemberLive();
            }
        }

        if (BypassCheck(controllerActionDescriptor)) return true;

        var isMember = memberId != null;

        var isRequestingOnboardingPage = IsRequestForOnboardingAction(controllerActionDescriptor);

        return (isMember && isLive && !isRequestingOnboardingPage)
               || (!isMember && isRequestingOnboardingPage)
               || (isMember && !isLive && isRequestingOnboardingPage);
        // NOTE: The last condition is a temporary measure to all a non-live member to go to onboarding. This will be updated in story
        // CSP-1221 shutter page for removed member
    }
}
