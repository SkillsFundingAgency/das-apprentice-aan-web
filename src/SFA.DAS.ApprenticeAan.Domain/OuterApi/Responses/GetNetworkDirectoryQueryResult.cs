using SFA.DAS.Aan.SharedUi.OuterApi.Responses;

namespace SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses;
public class GetNetworkDirectoryQueryResult
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public int TotalCount { get; set; }
    public IEnumerable<NetworkDirectorySummary> Members { get; set; } = Enumerable.Empty<NetworkDirectorySummary>();
}
