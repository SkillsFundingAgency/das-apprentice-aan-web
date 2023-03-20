using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Web.Filters;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models;

namespace SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;

[ExcludeFromCodeCoverage]
[Route("onboarding/name-of-employer", Name = RouteNames.Onboarding.NameOfEmployer)]
[RequiredSessionModel(typeof(OnboardingSessionModel))]
public class NameOfEmployerController : Controller
{
    public const string ViewPath = "~/Views/Onboarding/NameOfEmployer.cshtml";

    [HttpGet]
    public IActionResult Get()
    {
        return View(ViewPath);
    }
}