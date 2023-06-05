namespace SFA.DAS.ApprenticeAan.Web.Models.NetworkEvents;

public record struct LocationDetails
{
    public string? Location { get; set; }
    public string? Postcode { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public double? Distance { get; set; }
}
