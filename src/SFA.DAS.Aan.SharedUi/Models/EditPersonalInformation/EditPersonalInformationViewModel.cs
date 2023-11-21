using SFA.DAS.Aan.SharedUi.Models.AmbassadorProfile;

namespace SFA.DAS.Aan.SharedUi.Models;
public class EditPersonalInformationViewModel
{
    public int RegionId { get; set; }
    public string? JobTitle { get; set; }
    public bool ShowJobTitle { get; set; }
    public string? Biography { get; set; }
    public bool ShowBiography { get; set; }
    public MemberUserType UserType { get; set; }
    public string? OrganisationName { get; set; }
    public string YourAmbassadorProfileUrl { get; set; } = null!;
    public List<RegionViewModel> Regions { get; set; } = new();
}