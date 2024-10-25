using Microsoft.AspNetCore.Mvc.Rendering;

namespace SFA.DAS.Aan.SharedUi.Models.NetworkEvents;

public class EventFilterChoices
{
    public string? Keyword { get; set; }
    public string Location { get; set; }
    public int Radius { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public ChecklistDetails EventFormatChecklistDetails { get; set; } = new ChecklistDetails();
    public ChecklistDetails EventTypeChecklistDetails { get; set; } = new ChecklistDetails();
    public ChecklistDetails RegionChecklistDetails { get; set; } = new ChecklistDetails();

    public List<SelectListItem> RadiusOptions =>
    [
        new SelectListItem("5 miles", "5"),
        new SelectListItem("10 miles", "10"),
        new SelectListItem("20 miles", "20"),
        new SelectListItem("30 miles", "30"),
        new SelectListItem("50 miles", "50"),
        new SelectListItem("100 miles", "100"),
        new SelectListItem("Across England", "0")
    ];
}
