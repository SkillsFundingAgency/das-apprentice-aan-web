using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Aan.SharedUi.Infrastructure;
using SFA.DAS.Aan.SharedUi.Models;
using SFA.DAS.ApprenticeAan.Web.Configuration;

namespace SFA.DAS.ApprenticeAan.Web.Controllers;

[Authorize]
[Route("contact-us", Name = SharedRouteNames.ContactUs)]
public class ContactUsController(ApplicationConfiguration applicationConfiguration) : Controller
{
    private readonly ApplicationConfiguration _applicationConfiguration = applicationConfiguration;
    public const string ContactUsViewPath = "~/Views/ContactUs/Index.cshtml";

    [HttpGet]
    public IActionResult Index()
    {
        var contactUsEmails = _applicationConfiguration.ContactUsEmails;

        var viewModel = new ContactUsViewModel
        {
            EastMidlandsEmailAddress = contactUsEmails.EastMidlands,
            EastOfEnglandEmailAddress = contactUsEmails.EastOfEngland,
            LondonEmailAddress = contactUsEmails.London,
            NorthEastEmailAddress = contactUsEmails.NorthEast,
            NorthWestEmailAddress = contactUsEmails.NorthWest,
            SouthEastEmailAddress = contactUsEmails.SouthEast,
            SouthWestEmailAddress = contactUsEmails.SouthWest,
            WestMidlandsEmailAddress = contactUsEmails.WestMidlands,
            YorkshireAndTheHumberEmailAddress = contactUsEmails.YorkshireAndTheHumber
        };

        return View(ContactUsViewPath, viewModel);
    }
}
