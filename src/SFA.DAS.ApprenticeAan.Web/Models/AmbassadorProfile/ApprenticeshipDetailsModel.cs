namespace SFA.DAS.ApprenticeAan.Web.Models.AmbassadorProfile;

public class ApprenticeshipDetailsModel
{
    public ApprenticeshipDetailsModel(string? sector, string? programmes, string? level)
    {
        Sector = sector;
        Programmes = programmes;
        Level = level;
    }
    public string? Sector { get; set; }
    public string? Programmes { get; set; }
    public string? Level { get; set; }
}