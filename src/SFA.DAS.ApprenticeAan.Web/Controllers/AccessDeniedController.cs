using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Aan.SharedUi.Infrastructure;

namespace SFA.DAS.ApprenticeAan.Web.Controllers;

[AllowAnonymous]
[ExcludeFromCodeCoverage]
public class AccessDeniedController : Controller
{

    public const string RemovedShutterPath = "~/Views/AccessDenied/RemovedMember.cshtml";

    [Route("accessdenied")]
    public IActionResult Index()
    {
        return View();
    }

    [Route("access-removed", Name = SharedRouteNames.RemovedShutter)]
    public IActionResult RemovedShutter()
    {
        return View(RemovedShutterPath);
    }
}
