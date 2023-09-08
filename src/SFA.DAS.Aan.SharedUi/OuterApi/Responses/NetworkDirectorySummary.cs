using SFA.DAS.Aan.SharedUi.Constants;

namespace SFA.DAS.Aan.SharedUi.OuterApi.Responses;

public class NetworkDirectorySummary
{
    public Guid MemberId { get; set; }
    public string? FullName { get; set; } = null!;
    public int? RegionId { get; set; } = null!;
    public string? RegionName { get; set; } = null!;
    public Role UserType { get; set; }
    public bool? IsRegionalChair { get; set; } = null!;
    public DateTime JoinedDate { get; set; }
}

