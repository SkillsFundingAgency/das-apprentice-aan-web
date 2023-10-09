namespace SFA.DAS.Aan.SharedUi.OuterApi.Responses;

public class Apprenticeship
{
    public string Sector { get; set; } = null!;
    public string Programme { get; set; } = null!;
    public string Level { get; set; } = null!;
    public int ActiveApprenticesCount { get; set; }
    public List<string> Sectors { get; set; } = new List<string>();
}
