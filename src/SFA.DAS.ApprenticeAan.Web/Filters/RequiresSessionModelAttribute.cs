﻿using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;
using SFA.DAS.ApprenticeAan.Web.Models;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.ApprenticeAan.Web.Filters;

[ExcludeFromCodeCoverage]
public class RequiresSessionModelAttribute : ApplicationFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.ActionDescriptor is not ControllerActionDescriptor controllerActionDescriptor) return;

        if (BypassCheck(controllerActionDescriptor)) return;

        if (!HasValidSessionModel(context.HttpContext.RequestServices))
        {
            context.Result = RedirectToHome;
        }
    }

    private bool BypassCheck(ControllerActionDescriptor controllerActionDescriptor)
    {
        var controllersToByPass = new[] { nameof(BeforeYouStartController), nameof(TermsAndConditionsController), nameof(LineManagerController) };

        if (!IsRequestForOnboardingAction(controllerActionDescriptor)) return true;

        if (controllersToByPass.Contains(controllerActionDescriptor.ControllerTypeInfo.Name)) return true;

        return false;
    }

    private static bool HasValidSessionModel(IServiceProvider services)
    {
        ISessionService sessionService = services.GetService<ISessionService>()!;

        OnboardingSessionModel sessionModel = sessionService.Get<OnboardingSessionModel>();

        return sessionModel != null && sessionModel.IsValid;
    }
}
