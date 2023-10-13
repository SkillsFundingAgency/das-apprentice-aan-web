namespace SFA.DAS.ApprenticeAan.Web.Models.AmbassadorProfile;

public class ApprenticeshipDetailsModel
{
    public ApprenticeshipDetailsModel(string? sector, string? programme, string? level)
    {
        Sector = sector;
        Programme = programme;
        Level = level;
    }
    public string? Sector { get; set; }
    public string? Programme { get; set; }
    public string? Level { get; set; }
}