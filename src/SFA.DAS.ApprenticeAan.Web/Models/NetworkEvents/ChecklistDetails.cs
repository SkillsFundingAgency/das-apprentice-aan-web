namespace SFA.DAS.ApprenticeAan.Web.Models.NetworkEvents;

public class ChecklistDetails
{
    public string? Title { get; set; }
    public string? QueryStringParameterName { get; set; }

    public IEnumerable<ChecklistLookup> Lookups { get; set; } = Enumerable.Empty<ChecklistLookup>();
}