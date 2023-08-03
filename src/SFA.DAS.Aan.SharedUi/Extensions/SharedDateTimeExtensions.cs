namespace SFA.DAS.Aan.SharedUi.Extensions;

public static class SharedDateTimeExtensions
{
    private readonly static TimeZoneInfo SharedLocalTimeZone = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");
    public static DateTime SharedUtcToLocalTime(this DateTime date) => TimeZoneInfo.ConvertTimeFromUtc(date, SharedLocalTimeZone);
}
