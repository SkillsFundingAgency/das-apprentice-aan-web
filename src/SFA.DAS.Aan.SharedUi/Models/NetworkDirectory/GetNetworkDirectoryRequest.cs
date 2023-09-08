using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Aan.SharedUi.Constants;

namespace SFA.DAS.Aan.SharedUi.Models.NetworkDirectory;

public class GetNetworkDirectoryRequest
{
    [FromQuery]
    public string? Keyword { get; set; }

    [FromQuery]
    public List<int> RegionId { get; set; } = new List<int>();

    [FromQuery]
    public List<Role> UserType { get; set; } = new List<Role>();

    [FromQuery]
    public bool? IsRegionalChair { get; set; } = null;

    [FromQuery]
    public int? Page { get; set; }

    [FromQuery]
    public int? PageSize { get; set; }
}
