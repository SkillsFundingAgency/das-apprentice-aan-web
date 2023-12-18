namespace SFA.DAS.Aan.SharedUi.Models.PublicProfile;

public class AreasOfInterestSectionViewModel
{
    public string Title { get; set; } = null!;
    public List<AreasOfInterestSection> Sections { get; set; } = new();
    public bool HasItems => Sections.SelectMany(s => s.Interests).Any();
}

public record AreasOfInterestSection(string Title, IEnumerable<string> Interests)
{
    public bool HasItems => Interests.Any();
}
