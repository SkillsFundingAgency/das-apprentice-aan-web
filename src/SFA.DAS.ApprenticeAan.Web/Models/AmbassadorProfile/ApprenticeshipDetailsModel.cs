namespace SFA.DAS.ApprenticeAan.Web.Models.AmbassadorProfile;

public class ApprenticeshipDetailsModel(string? sector, string? programme, string? level)
{
    public string? Sector { get; set; } = sector;
    public string? Programme { get; set; } = programme;
    public string? Level { get; set; } = level;
}