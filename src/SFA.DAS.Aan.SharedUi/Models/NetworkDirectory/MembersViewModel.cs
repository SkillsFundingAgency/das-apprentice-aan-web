using SFA.DAS.Aan.SharedUi.Constants;
using SFA.DAS.Aan.SharedUi.OuterApi.Responses;

namespace SFA.DAS.Aan.SharedUi.Models.NetworkDirectory;
public class MembersViewModel
{
    public Guid MemberId { get; set; }
    public string? FullName { get; set; }
    public int? RegionId { get; set; }
    public string? RegionName { get; set; }
    public Role UserRole { get; set; }
    public bool? IsRegionalChair { get; set; }
    public DateTime JoinedDate { get; set; }

    public static implicit operator MembersViewModel(MemberSummary source)
        => new()
        {
            MemberId = source.MemberId,
            FullName = source.FullName,
            RegionId = source.RegionId,
            RegionName = (source.RegionId != null) ? source.RegionName : "Multi-regional",
            UserRole = (source.IsRegionalChair ?? false) ? Role.RegionalChair : source.UserType,
            IsRegionalChair = source.IsRegionalChair,
            JoinedDate = source.JoinedDate
        };
}