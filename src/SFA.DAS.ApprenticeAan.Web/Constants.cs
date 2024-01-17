namespace SFA.DAS.ApprenticeAan.Web;

public static class Constants
{
    public static class SessionKeys
    {
        public const string MemberId = nameof(MemberId);
        public const string MemberStatus = nameof(MemberStatus);
    }

    public static class MemberStatus
    {
        public static string Live = "Live";
        public static string Withdrawn = "Withdrawn";
    }
}
