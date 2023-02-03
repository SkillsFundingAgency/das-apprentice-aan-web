using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.ApprenticeAan.Web.Controllers;

public class HomeController : Controller
{
    public IActionResult Index() => View();
}