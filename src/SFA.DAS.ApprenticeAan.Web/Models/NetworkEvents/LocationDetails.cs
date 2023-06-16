namespace SFA.DAS.ApprenticeAan.Web.Models.NetworkEvents;

public record struct LocationDetails(string? Location, string? Postcode, double? Latitude, double? Longitude, double? Distance);