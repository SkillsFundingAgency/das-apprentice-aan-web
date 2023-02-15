using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;

namespace SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;

[Route("onboarding/regions")]
public class RegionsController : OnboardingControllerBase
{
    private readonly IRegionService _regionService;
    public RegionsController(ISessionService sessionService, IRegionService regionService) : base(sessionService)
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
