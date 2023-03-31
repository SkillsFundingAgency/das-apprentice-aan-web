namespace SFA.DAS.ApprenticeAan.Web.Models;

public class SelectProfileModel
{
    public int Id { get; set; }
    public string Description { get; set; } = null!;
    public string Category { get; set; } = null!;
    public int Ordering { get; set; }
    public bool IsSelected { get; set; }

    public static implicit operator SelectProfileModel(ProfileModel profile) => new()
    {
        Id = profile.Id,
        Description = profile.Description,
        Category = profile.Category,
        Ordering = profile.Ordering
    };
}