using SFA.DAS.Aan.SharedUi.Models.NetworkEvents;

namespace SFA.DAS.ApprenticeAan.Web.Models.NetworkDirectory;

public class DirectoryFilterChoices
{
    public string? Keyword { get; set; }
    public ChecklistDetails RoleChecklistDetails { get; set; } = new ChecklistDetails();
    public ChecklistDetails RegionChecklistDetails { get; set; } = new ChecklistDetails();
}
