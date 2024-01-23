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

    public static MemberStatus? GetMemberStatus(this ISessionService sessionService)
    {
        if (GetMemberId(sessionService) == Guid.Empty) return null;
        var status = sessionService.Get(SessionKeys.Member.Status);


        foreach (var val in Enum.GetValues(typeof(MemberStatus)))
        {
            if (status == val.ToString())
                return (MemberStatus)val;
        }

        return null;
    }
}
