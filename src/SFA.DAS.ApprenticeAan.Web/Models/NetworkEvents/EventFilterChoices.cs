namespace SFA.DAS.ApprenticeAan.Web.Models.NetworkEvents;

public class EventFilterChoices
{
    public string? Keyword { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public ChecklistDetails EventFormatChecklistDetails { get; set; } = new ChecklistDetails();
    public ChecklistDetails EventTypeChecklistDetails { get; set; } = new ChecklistDetails();
    public ChecklistDetails RegionChecklistDetails { get; set; } = new ChecklistDetails();
}
