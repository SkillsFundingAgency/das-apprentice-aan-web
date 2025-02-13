namespace SFA.DAS.ApprenticeAan.Web.Models;

public class NotificationLocation
{
    public string LocationName { get; set; }
    public double[] GeoPoint { get; set; }
    public int Radius { get; set; }
}
