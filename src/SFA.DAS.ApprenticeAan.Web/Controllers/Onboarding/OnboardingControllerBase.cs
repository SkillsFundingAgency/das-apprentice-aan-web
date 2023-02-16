﻿using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Models;

namespace SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;

public abstract class OnboardingControllerBase : Controller
{
    protected readonly ISessionService SessionService;
    protected OnboardingControllerBase(ISessionService sessionService)
    {
        SessionService = sessionService;
    }

    protected (OnboardingSessionModel?, IActionResult?) GetSessionModelWithEscapeRoute(ILogger logger)
    {
        var sessionModel = SessionService.Get<OnboardingSessionModel>();
        if (sessionModel == null)
        {
            logger.LogInformation("Session model is invalid, redirecting to escape route.");
            return (null, RedirectToAction("Index", "Home"));
        }
        return (sessionModel, null);
    }
}