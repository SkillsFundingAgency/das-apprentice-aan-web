using System.Text;
using System.Web;
using SFA.DAS.ApprenticeAan.Web.Models.NetworkEvents;

namespace SFA.DAS.ApprenticeAan.Web.UrlHelpers;

public static class MapLinkGenerator
{
    public static string GetStaticImagePreviewLink(LocationDetails locationDetails, string privateApiKey, string signature)
    {
        string location = HttpUtility.UrlEncode(locationDetails.Location!);

        return new StringBuilder()
            .Append($"https://maps.googleapis.com/maps/api/staticmap?center={location}")
            .Append("&size=300x235")
            .Append("&maptype=roadmap")
            .Append("&zoom=13")
            .Append("&markers=color:red")
            .Append('|')
            .Append($"{locationDetails.Latitude}")
            .Append(',')
            .Append($"{locationDetails.Longitude}")
            .Append($"&key={privateApiKey}")
            .Append($"&signature={signature}")
            .ToString();
    }

    public static string GetLinkToFullMap(double latitude, double longitude)
    {
        return $"https://www.google.com/maps/dir//{latitude},{longitude}";
    }
}
