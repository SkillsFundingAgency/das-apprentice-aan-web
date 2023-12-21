using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Aan.SharedUi.Constants;

namespace SFA.DAS.Aan.SharedUi.Models.NetworkDirectory;

public class NetworkDirectoryRequestModel
{
    [FromQuery]
    public string? Keyword { get; set; }

    [FromQuery]
    public List<int> RegionId { get; set; } = new List<int>();

    [FromQuery]
    public List<Role> UserRole { get; set; } = new List<Role>();

    [FromQuery]
    public List<MemberMaturityStatus> Status { get; set; } = new List<MemberMaturityStatus>();

    [FromQuery]
    public bool? IsRegionalChair { get; set; } = null;

    [FromQuery]
    public int? Page { get; set; }

    [FromQuery]
    public int? PageSize { get; set; }

    public Dictionary<string, string[]> ToQueryStringParameters()
    {
        var parameters = new Dictionary<string, string[]>();
        if (!string.IsNullOrWhiteSpace(Keyword)) parameters.Add("keyword", new[] { Keyword.Trim() });
        parameters.Add("regionId", RegionId.Select(region => region.ToString()).ToArray());
        if (UserRole.Any())
        {
            parameters.Add("isRegionalChair", new[] { UserRole.Exists(userRole => userRole == Role.RegionalChair).ToString() }!);
            parameters.Add("userType", UserRole.Where(userRole => userRole != Role.RegionalChair).Select(userRole => userRole.ToString()).ToArray());
        }
        if (Status.Count == 1)
        {
            var value = Status.First() == MemberMaturityStatus.New;
            parameters.Add("isNew", new[] { value.ToString() });
        }
        if (Page != null) parameters.Add("page", new[] { Page.ToString() }!);
        if (PageSize != null) parameters.Add("pageSize", new[] { PageSize.ToString() }!);

        return parameters;
    }
}
