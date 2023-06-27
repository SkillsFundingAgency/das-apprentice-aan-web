namespace SFA.DAS.ApprenticeAan.Web.Models.NetworkEvents;

public class ChecklistDetails
{
    public string? Title { get; set; }
    public string? QueryStringParameterName { get; set; }

    public List<ChecklistLookup> Lookups { get; set; } = new List<ChecklistLookup>();
}