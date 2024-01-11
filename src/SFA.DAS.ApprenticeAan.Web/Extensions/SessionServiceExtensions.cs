using SFA.DAS.Aan.SharedUi.Constants;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using static SFA.DAS.ApprenticeAan.Web.Constants;

namespace SFA.DAS.ApprenticeAan.Web.Extensions;

public static class SessionServiceExtensions
{
    public static Guid GetMemberId(this ISessionService sessionService)
    {
        var id = Guid.Empty;

        var memberId = sessionService.Get(SessionKeys.Member.MemberId);

        if (Guid.TryParse(memberId, out var newGuid))
        {
            id = newGuid;
        }

        return id;
    }

    public static bool IsMemberLive(this ISessionService sessionService)
    {
        var memberId = GetMemberId(sessionService);
        if (memberId == Guid.Empty) return false;

        var status = sessionService.Get(SessionKeys.Member.Status);
        return !string.IsNullOrEmpty(status) && status == MemberStatus.Live.ToString();
    }
}
