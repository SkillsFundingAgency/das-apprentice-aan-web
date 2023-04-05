﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Filters;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;

namespace SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;

[Authorize]
[Route("onboarding/check-your-answers", Name = RouteNames.Onboarding.CheckYourAnswers)]
[RequiredSessionModel(typeof(OnboardingSessionModel))]
public class CheckYourAnswersController : Controller
{
    public const string ViewPath = "~/Views/Onboarding/CheckYourAnswers.cshtml";
    private readonly ISessionService _sessionService;

    public CheckYourAnswersController(ISessionService sessionService)
    {
        _sessionService = sessionService;
    }

    public IActionResult Index()
    {
        var sessionModel = _sessionService.Get<OnboardingSessionModel>();
        sessionModel.HasSeenPreview = true;
        _sessionService.Set(sessionModel);

        CheckYourAnswersViewModel model = new(Url, sessionModel);
        return View(ViewPath, model);
    }
}