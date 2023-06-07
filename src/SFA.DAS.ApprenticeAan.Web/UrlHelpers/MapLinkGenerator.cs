using System.Web;
using SFA.DAS.ApprenticeAan.Web.Models.NetworkEvents;

namespace SFA.DAS.ApprenticeAan.Web.UrlHelpers;

public static class MapLinkGenerator
{
    public static string GetStaticImagePreviewLink(LocationDetails locationDetails, string privateApiKey, string signature)
    {
        string location = HttpUtility.UrlEncode(locationDetails.Location!);
        return $"https://maps.googleapis.com/maps/api/staticmap?center={location}&size=250x250&maptype=roadmap&zoom=13&&markers=color:red|label:C|{locationDetails.Longitude},{locationDetails.Latitude}&key=AIzaSyDUm26s3x5fDIxnply07hN5egKZ5qTQDY0";
    }

    public static string GetLinkToFullMap(
        double latitude,
        double longitude,
        string privateApiKey,
        string signature)
    {
        return $"https://www.google.com/maps/dir/{latitude},{longitude}";
    }
}
