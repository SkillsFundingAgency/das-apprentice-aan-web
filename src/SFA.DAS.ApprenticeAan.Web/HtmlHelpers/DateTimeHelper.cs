namespace SFA.DAS.ApprenticeAan.Web.HtmlHelpers;

public static class DateTimeHelper
{
    public static string? ToScreenFormat(DateTime? dateTime)
    {
        return dateTime?.ToString("dd/MM/yyyy");
    }

    public static string? ToUrlFormat(DateTime? dateTime)
    {
        return dateTime?.ToString("yyyy-MM-dd");
    }
}
