namespace SFA.DAS.Aan.SharedUi.Models.PublicProfile;

public class MemberProfileViewModel : ConnectWithMemberViewModel, INetworkHubLink, ILinkedInViewModel
{
    public MemberPersonalInformationViewModel PersonalInformation { get; set; } = new();

    public string Email { get; set; } = null!;
    public string? LinkedInUrl { get; set; }

    public string ApprenticeshipSectionTitle { get; set; } = null!;
    public bool IsEmployerInformationAvailable { get; set; }
    public bool IsApprenticeshipInformationAvailable { get; set; }
    public string? EmployerName { get; set; }
    public string? EmployerAddress { get; set; }
    public string Sector { get; set; } = null!;
    public string Programme { get; set; } = null!;
    public string Level { get; set; } = null!;
    public List<string> Sectors { get; set; } = new();
    public int ActiveApprenticesCount { get; set; }

    public AreasOfInterestViewModel AreasOfInterest { get; set; } = null!;
    public string? NetworkHubLink { get; set; }
}
