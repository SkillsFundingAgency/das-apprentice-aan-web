using SFA.DAS.Aan.SharedUi.Models.NetworkEvents;
using SFA.DAS.ApprenticeAan.Web.Models.NetworkDirectory;

namespace SFA.DAS.Aan.SharedUi.Models.NetworkDirectory;

public class NetworkDirectoryViewModel
{
    public PaginationViewModel PaginationViewModel { get; set; } = null!;

    public int TotalCount { get; set; }

    public string TotalCountDescription
    {
        get
        {
            string totalCountDescription = $"{TotalCount} results";
            if (TotalCount == 1)
            {
                totalCountDescription = "1 result";
            }
            return totalCountDescription;
        }
    }

    public List<MembersViewModel> Members { get; set; } = new List<MembersViewModel>();

    public DirectoryFilterChoices FilterChoices { get; set; } = new DirectoryFilterChoices();

    public SelectedFiltersModel SelectedFiltersModel { get; set; } = new SelectedFiltersModel();
}

