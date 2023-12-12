using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Aan.SharedUi.Infrastructure;

namespace SFA.DAS.ApprenticeAan.Web.Controllers;

[Authorize]
[Route("contact-us", Name = SharedRouteNames.ContactUs)]
public class ContactUsController : Controller
{

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
}
