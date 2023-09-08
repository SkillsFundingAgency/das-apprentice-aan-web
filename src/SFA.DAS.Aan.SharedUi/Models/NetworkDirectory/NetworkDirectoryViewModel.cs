using SFA.DAS.Aan.SharedUi.Constants;
using SFA.DAS.Aan.SharedUi.Models.NetworkEvents;
using SFA.DAS.Aan.SharedUi.OuterApi.Responses;
using SFA.DAS.ApprenticeAan.Web.Models.NetworkDirectory;

namespace SFA.DAS.Aan.SharedUi.Models.NetworkDirectory;

public class NetworkDirectoryViewModel
{
    public PaginationViewModel PaginationViewModel { get; set; } = null!;

    public int TotalCount { get; set; }

    public List<MembersViewModel> Members { get; set; } = new List<MembersViewModel>();

    public DirectoryFilterChoices FilterChoices { get; set; } = new DirectoryFilterChoices();

    public List<SelectedFilter> SelectedFilters { get; set; } = new List<SelectedFilter>();

    public bool ShowFilterOptions => SelectedFilters.Any();

    public string ClearSelectedFiltersLink { get; set; } = null!;

}

public class MembersViewModel
{
    public Guid MemberId { get; set; }
    public string? FullName { get; set; } = null!;
    public int? RegionId { get; set; } = null!;
    public string? RegionName { get; set; } = null!;
    public Role UserType { get; set; }
    public bool? IsRegionalChair { get; set; } = null!;
    public DateTime JoinedDate { get; set; }

    public static implicit operator MembersViewModel(NetworkDirectorySummary source)
        => new()
        {
            MemberId = source.MemberId,
            FullName = source.FullName,
            RegionId = source.RegionId,
            RegionName = (source.RegionId != null) ? source.RegionName : "Multi-regional",
            UserType = (source.IsRegionalChair ?? false) ? Role.IsRegionalChair : source.UserType,
            IsRegionalChair = source.IsRegionalChair,
            JoinedDate = source.JoinedDate
        };
}