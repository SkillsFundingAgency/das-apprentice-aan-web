namespace SFA.DAS.Aan.SharedUi.Models.PublicProfile;

public class MemberPersonalInformationViewModel
{
    public string UserRole { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string? JobTitle { get; set; }
    public string RegionName { get; set; } = null!;
    public string? Biography { get; set; }
}
