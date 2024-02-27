namespace SFA.DAS.Aan.SharedUi.OuterApi.Responses;
public class ApprenticeshipDetails
{
    public string Sector { get; set; } = null!;
    public string Programme { get; set; } = null!;
    public string Level { get; set; } = null!;
    public List<string> Sectors { get; set; } = [];
    public int ActiveApprenticesCount { get; set; }
}