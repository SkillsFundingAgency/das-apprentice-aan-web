namespace SFA.DAS.Aan.SharedUi.Models.NetworkEvents;

public class SelectedFiltersModel
{
    public List<SelectedFilter> SelectedFilters { get; set; } = new List<SelectedFilter>();

    public bool ShowFilterOptions => SelectedFilters.Any();

    public string ClearSelectedFiltersLink { get; set; } = null!;
}