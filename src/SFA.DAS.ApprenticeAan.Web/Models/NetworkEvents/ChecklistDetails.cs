namespace SFA.DAS.ApprenticeAan.Web.Models.NetworkEvents;

public class ChecklistDetails
{
    public string? InputName { get; set; }
    public List<ChecklistLookup> Lookups { get; set; } = new List<ChecklistLookup>();
}