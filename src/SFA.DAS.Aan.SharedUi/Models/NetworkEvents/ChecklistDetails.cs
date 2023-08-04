namespace SFA.DAS.Aan.SharedUi.Models.NetworkEvents;

public class ChecklistDetails
{
    public string? Title { get; set; }
    public string? QueryStringParameterName { get; set; }

    public IEnumerable<ChecklistLookup> Lookups { get; set; } = Enumerable.Empty<ChecklistLookup>();
}