using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.GovUK.Auth.Services;

namespace SFA.DAS.ApprenticeAan.Web.Controllers;

public class ServicesController(IStubAuthenticationService stubAuthenticationService, IConfiguration configuration) : Controller
{
    [HttpGet]
    [Route("account-details", Name = RouteNames.StubAccountDetailsGet)]
    public IActionResult AccountDetails([FromQuery] string returnUrl)
    {
        if (configuration["ResourceEnvironmentName"].ToUpper() == "PRD")
        {
            return NotFound();
        }

        return View("AccountDetails", new StubAuthenticationViewModel
        {
            ReturnUrl = returnUrl
        });
    }

    [HttpPost]
    [Route("account-details", Name = RouteNames.StubAccountDetailsPost)]
    public async Task<IActionResult> AccountDetails(StubAuthenticationViewModel model)
    {
        if (configuration["ResourceEnvironmentName"].ToUpper() == "PRD")
        {
            return NotFound();
        }

        var claims = await stubAuthenticationService.GetStubSignInClaims(model);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claims,
            new AuthenticationProperties());

        return RedirectToRoute(RouteNames.StubSignedIn, new { returnUrl = model.ReturnUrl });
    }

    [HttpGet]
    [Authorize]
    [Route("Stub-Auth", Name = RouteNames.StubSignedIn)]
    public IActionResult StubSignedIn([FromQuery] string returnUrl)
    {
        if (configuration["ResourceEnvironmentName"].ToUpper() == "PRD")
        {
            return NotFound();
        }
        var viewModel = new StubAuthenticationViewModel
        {
            Email = User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Email))?.Value,
            Id = User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.NameIdentifier))?.Value,
            ReturnUrl = returnUrl
        };
        return View(viewModel);
    }
}