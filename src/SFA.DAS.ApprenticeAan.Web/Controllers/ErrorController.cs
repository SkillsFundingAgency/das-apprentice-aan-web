using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticePortal.Authentication;

namespace SFA.DAS.ApprenticeAan.Web.Controllers;

[Authorize]
public class ErrorController : Controller
{
    private readonly ILogger<ErrorController> _logger;

    public ErrorController(ILogger<ErrorController> logger)
    {
        _logger = logger;
    }

    [AllowAnonymous]
    [Route("Error/{statuscode}")]
    public IActionResult HttpStatusCodeHandler(int statusCode)
    {
        switch (statusCode)
        {
            case 403:
            case 404:
                return View("PageNotFound");
            default:
                return View("ErrorInService");
        }
    }

    [AllowAnonymous]
    [Route("Error")]
    public IActionResult ErrorInService()
    {
        var feature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

        if (User.Identity!.IsAuthenticated)
        {
            _logger.LogError(feature!.Error, "Unexpected error occured during request to path: {path} by user: {user}", feature.Path, User.FindFirstValue(IdentityClaims.GivenName));
        }
        else
        {
            _logger.LogError(feature!.Error, "Unexpected error occured during request to {path}", feature.Path);
        }
        return View();
    }
}