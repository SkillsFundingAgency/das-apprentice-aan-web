namespace SFA.DAS.Aan.SharedUi.Models.PublicProfile;

public class ApprenticeshipSectionViewModel
{
    public bool IsEmployerInformationAvailable { get; set; }
    public bool IsApprenticeshipInformationAvailable { get; set; }
    public string ApprenticeshipSectionTitle { get; set; } = null!;
    public string? EmployerName { get; set; }
    public string? EmployerAddress { get; set; }
    public string Sector { get; set; } = null!;
    public string Programme { get; set; } = null!;
    public string Level { get; set; } = null!;
    public List<string> Sectors { get; set; } = new();
    public int ActiveApprenticesCount { get; set; }
}
