using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;
using SFA.DAS.ApprenticePortal.SharedUi.Menu;

namespace SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;

[Authorize]
[Route("onboarding/confirmDetails", Name = RouteNames.Onboarding.ConfirmDetails)]
[HideNavigationBar(true, true)]
public class ConfirmDetailsController : Controller
{
    public const string ViewPath = "~/Views/Onboarding/ConfirmDetails.cshtml";

    private readonly ISessionService _sessionService;

    public ConfirmDetailsController(ISessionService sessionService)
    {
        _sessionService = sessionService;
    }

    [HttpGet]
    public IActionResult Index([FromRoute] string employerAccountId)
    {
        var sessionModel = _sessionService.Get<OnboardingSessionModel>();
        var viewModel = GetViewModel(sessionModel);
        return View(ViewPath, viewModel);
    }

    [HttpPost]
    public IActionResult Index()
    {
        return RedirectToRoute(RouteNames.Onboarding.EmployerSearch);
    }

    private ConfirmDetailsViewModel GetViewModel(OnboardingSessionModel sessionModel)
    {
        return new ConfirmDetailsViewModel
        {
            BackLink = Url.RouteUrl(RouteNames.Onboarding.RegionalNetwork)!,
            FullName = sessionModel.ApprenticeDetails.Name,
            Email = sessionModel.ApprenticeDetails.Email,
            ApprenticeshipSector = sessionModel.MyApprenticeship.TrainingCourse?.Sector,
            ApprenticeshipProgram = sessionModel.MyApprenticeship.TrainingCourse?.Name,
            ApprenticeshipLevel = sessionModel.MyApprenticeship.TrainingCourse?.Level,
        };
    }
}

