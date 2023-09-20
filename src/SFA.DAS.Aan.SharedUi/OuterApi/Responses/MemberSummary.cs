using SFA.DAS.Aan.SharedUi.Constants;

namespace SFA.DAS.Aan.SharedUi.OuterApi.Responses;

public class MemberSummary
{
    public Guid MemberId { get; set; }
    public string? FullName { get; set; }
    public int? RegionId { get; set; }
    public string? RegionName { get; set; }
    public Role UserType { get; set; }
    public bool? IsRegionalChair { get; set; }
    public DateTime JoinedDate { get; set; }
}

