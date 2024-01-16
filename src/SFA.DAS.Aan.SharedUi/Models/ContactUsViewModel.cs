namespace SFA.DAS.Aan.SharedUi.Models;

public class ContactUsViewModel : INetworkHubLink
{
    public string EastMidlandsEmailAddress { get; set; } = null!;
    public string EastOfEnglandEmailAddress { get; set; } = null!;
    public string LondonEmailAddress { get; set; } = null!;
    public string NorthEastEmailAddress { get; set; } = null!;
    public string NorthWestEmailAddress { get; set; } = null!;
    public string SouthEastEmailAddress { get; set; } = null!;
    public string SouthWestEmailAddress { get; set; } = null!;
    public string WestMidlandsEmailAddress { get; set; } = null!;
    public string YorkshireAndTheHumberEmailAddress { get; set; } = null!;
    public string? NetworkHubLink { get; set; }
}
