namespace SFA.DAS.Aan.SharedUi.OuterApi.Responses;

public class Apprenticeship
{
    public string Sector { get; set; } = string.Empty;
    public string Programmes { get; set; } = string.Empty;
    public string Level { get; set; } = string.Empty;
    public int ActiveApprenticesCount { get; set; }
    public List<string> Sectors { get; set; } = new List<string>();
}
