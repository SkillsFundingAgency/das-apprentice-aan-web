namespace SFA.DAS.Aan.SharedUi.Models.EditAreaOfInterest;

public class ProfileViewModel
{
    public int Id { get; set; }
    public string? Value { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Category { get; set; } = null!;
    public int Ordering { get; set; }

    public static implicit operator ProfileViewModel(Profile profile) => new()
    {
        Id = profile.Id,
        Description = profile.Description,
        Category = profile.Category,
        Ordering = profile.Ordering
    };
}