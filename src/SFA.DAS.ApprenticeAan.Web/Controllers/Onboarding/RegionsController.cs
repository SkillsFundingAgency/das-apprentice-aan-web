using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;

namespace SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;

//TODO remove ExcludeFromCodeCoverage on proper implementation
//This is initial implementation just to get api client working example
//remove this comments once proper implementation is done
[ExcludeFromCodeCoverage]

[Route("onboarding/regions")]
public class RegionsController : Controller
{
    private readonly IRegionService _regionService;

    public RegionsController(IRegionService regionService)
    {
        _regionService = regionService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var regions = await _regionService.GetRegions();
        return Ok(regions);
    }
}
