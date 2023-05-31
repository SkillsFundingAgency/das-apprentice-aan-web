using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.ApprenticeAan.Web.Controllers;

[AllowAnonymous]
[Route("accessdenied")]
[ExcludeFromCodeCoverage]
public class AccessDeniedController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
