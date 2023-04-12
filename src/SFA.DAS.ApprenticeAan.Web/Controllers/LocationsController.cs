using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;

namespace SFA.DAS.ApprenticeAan.Web.Controllers;

[ExcludeFromCodeCoverage]
//[Authorize]
public class LocationsController : Controller
{
    private readonly IOuterApiClient _outerApiClient;

    public LocationsController(IOuterApiClient outerApiClient)
    {
        _outerApiClient = outerApiClient;
    }

    [HttpGet]
    [Route("/locations")]
    public async Task<IActionResult> GetAddresses([FromQuery] string query)
    {
        var result = await _outerApiClient.GetAddresses(query);

        return Ok(result.Addresses);
    }
}
