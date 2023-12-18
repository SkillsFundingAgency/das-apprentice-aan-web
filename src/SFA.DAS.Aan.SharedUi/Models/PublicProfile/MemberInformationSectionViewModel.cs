using SFA.DAS.Aan.SharedUi.Constants;

namespace SFA.DAS.Aan.SharedUi.Models.PublicProfile;

public class MemberInformationSectionViewModel
{
    public Role UserRole { get; set; }
    public string FullName { get; set; } = null!;
    public string? JobTitle { get; set; }
    public string RegionName { get; set; } = null!;
    public string? Biography { get; set; }
}
