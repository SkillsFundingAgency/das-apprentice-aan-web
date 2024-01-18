using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using static SFA.DAS.ApprenticeAan.Web.Constants;

namespace SFA.DAS.ApprenticeAan.Web.Extensions;

public static class SessionServiceExtensions
{
    public static Guid GetMemberId(this ISessionService sessionService)
    {
        var id = Guid.Empty;

        var memberId = sessionService.Get(Constants.SessionKeys.MemberId);

        if (Guid.TryParse(memberId, out var newGuid))
        {
            id = newGuid;
        }

        return id;
    }

    public static string GetMemberStatus(this ISessionService sessionService)
    {
        var status = sessionService.Get(SessionKeys.MemberStatus);

        return string.IsNullOrEmpty(status) ? string.Empty : status;
    }


    public static bool IsMemberLive(this ISessionService sessionService)
    {
        var status = sessionService.Get(SessionKeys.MemberStatus);
        return !string.IsNullOrEmpty(status) && status == MemberStatus.Live;
    }
}
