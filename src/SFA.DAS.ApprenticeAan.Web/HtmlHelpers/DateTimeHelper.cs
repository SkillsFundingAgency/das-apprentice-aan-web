namespace SFA.DAS.ApprenticeAan.Web.HtmlHelpers;

public class DateTimeHelper
{
    public static string? ToScreenFormat(DateTime? dateTime)
    {
        return dateTime?.ToString("d/MM/yyyy");
    }

    public static string? ToUrlFormat(DateTime? dateTime)
    {
        return dateTime?.ToString("yyyy-MM-dd");
    }
}
