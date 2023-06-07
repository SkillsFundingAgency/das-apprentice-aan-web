using System.Web;

namespace SFA.DAS.ApprenticeAan.Web.UrlHelpers;

public static class MapLink
{
    public static string FromLocation(string nameAddressAndPostcode, string privateApiKey)
    {
        string location = HttpUtility.UrlEncode(nameAddressAndPostcode);
        return $"https://www.google.com/maps/embed/v1/place?key={privateApiKey}&q={location}";
    }
}
