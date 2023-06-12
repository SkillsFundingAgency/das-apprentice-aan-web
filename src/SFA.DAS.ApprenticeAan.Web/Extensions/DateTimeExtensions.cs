namespace SFA.DAS.ApprenticeAan.Web.Extensions;

public static class DateTimeExtensions
{
    public static string ToApiString(this DateOnly date) => date.ToString("yyyy-MM-dd");
}
