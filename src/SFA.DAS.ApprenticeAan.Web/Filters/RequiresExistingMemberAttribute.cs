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

    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        return;

        if (context.ActionDescriptor is not ControllerActionDescriptor controllerActionDescriptor) return;

        if (BypassCheck(controllerActionDescriptor)) return;

        var isValidRequest = await IsValidRequest(context, controllerActionDescriptor);

        if (!isValidRequest)
        {
            context.Result = RedirectToHome;
        }
    }

    private bool BypassCheck(ControllerActionDescriptor controllerActionDescriptor)
    {
        var controllersToByPass = new[] { nameof(HomeController), nameof(LocationsController), nameof(AccessDeniedController), nameof(LeavingTheNetworkConfirmationController) };

        return controllersToByPass.Contains(controllerActionDescriptor.ControllerTypeInfo.Name);
    }

    // private static bool IsValidRequest(ActionExecutingContext context, ControllerActionDescriptor controllerActionDescriptor)
    // {
    //     var isMember = context.HttpContext._sessionService.GetMemberId() != Guid.Empty;
    //
    //     var isRequestingOnboardingPage = IsRequestForOnboardingAction(controllerActionDescriptor);
    //
    //     return (isMember && !isRequestingOnboardingPage) || (!isMember && isRequestingOnboardingPage);
    // }

    private async Task<bool> IsValidRequest(ActionExecutingContext context, ControllerActionDescriptor controllerActionDescriptor)
    {
        return true;

        var isLive = false;
        var memberId = _sessionService.Get(Constants.SessionKeys.MemberId);
        var currentMemberStatus = _sessionService.Get(Constants.SessionKeys.MemberStatus);

        if (!string.IsNullOrEmpty(currentMemberStatus))
        {
            if (currentMemberStatus == Constants.MemberStatus.Live)
            {
                isLive = true;
            }
        }

        if (memberId == null)
        {
            var response = await _outerApiClient.GetApprentice(context.HttpContext.User.GetApprenticeId());

            if (response.ResponseMessage.IsSuccessStatusCode)
            {
                var apprentice = response.GetContent();
                memberId = apprentice!.MemberId.ToString().ToLower();
                _sessionService.Set(Constants.SessionKeys.MemberId, memberId);
                var memberStatus = apprentice.Status;
                _sessionService.Set(Constants.SessionKeys.MemberStatus, memberStatus);

                if (memberStatus == Constants.MemberStatus.Live)
                {
                    isLive = true;
                }
            }
        }

        if (BypassCheck(controllerActionDescriptor)) return true;

        var isMember = memberId != null;

        var isRequestingOnboardingPage = IsRequestForOnboardingAction(controllerActionDescriptor);

        return (isMember && isLive && !isRequestingOnboardingPage) || (!isMember && isRequestingOnboardingPage);
    }

}
