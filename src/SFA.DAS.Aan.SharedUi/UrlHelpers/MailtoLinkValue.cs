namespace SFA.DAS.Aan.SharedUi.UrlHelpers;

public static class MailtoLinkValue
{
    public static string FromAddressAndSubject(string emailAddress, string subject)
    {
        return $"mailto:{emailAddress}?subject={Uri.EscapeDataString(subject)}";
    }
}
