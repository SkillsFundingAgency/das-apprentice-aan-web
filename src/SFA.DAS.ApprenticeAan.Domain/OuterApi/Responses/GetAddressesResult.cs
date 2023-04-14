namespace SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses;

public class GetAddressesResult
{
    public List<AddressItem> Addresses { get; set; } = new();
}

public class AddressItem
{
    public string Uprn { get; set; } = null!;
    public string? OrganisationName { get; set; }
    public string? Town { get; set; }
    public string? County { get; set; }
    public string? Postcode { get; set; }
    public string? AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public double? Longitude { get; set; }
    public double? Latitude { get; set; }
}
