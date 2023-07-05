namespace SFA.DAS.ApprenticeAan.Web.Extensions;

public static class DateTimeExtensions
{
    public static string ToApiString(this DateOnly date) => date.ToString("yyyy-MM-dd");
    public static string ToApiString(this DateTime date) => date.ToString("yyyy-MM-dd");
    public static string ToScreenString(this DateTime date) => date.ToString("dd/MM/yyyy");

    private readonly static TimeZoneInfo LocalTimeZone = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");
    public static DateTime UtcToLocalTime(this DateTime date) => TimeZoneInfo.ConvertTimeFromUtc(date, LocalTimeZone);
}
