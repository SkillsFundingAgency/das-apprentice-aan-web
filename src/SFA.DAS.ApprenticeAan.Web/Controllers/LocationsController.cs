using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;

namespace SFA.DAS.ApprenticeAan.Web.Controllers;

[Authorize]
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

    [HttpGet]
    [Route("/locations/search")]
    public async Task<IActionResult> GetLocationsBySearch([FromQuery] string query, CancellationToken cancellationToken)
    {
        var result = await _outerApiClient.GetLocationsBySearch(query, cancellationToken);
        return Ok(result.Locations);
    }
}
