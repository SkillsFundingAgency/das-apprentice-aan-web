using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

namespace SFA.DAS.ApprenticeAan.Web.Extensions;

[ExcludeFromCodeCoverage]
public static class ClaimsPrincipalExtensions
{
    public const string AanMemberId = nameof(AanMemberId);

    public static void AddAanMemberIdClaim(this ClaimsPrincipal principal, Guid memberId)
    {
        principal.AddIdentity(new ClaimsIdentity(new[] { new Claim(AanMemberId, memberId.ToString()) }));

    }

    public static Guid GetAanMemberId(this ClaimsPrincipal principal)
    {
        var memberId = principal.FindFirstValue(AanMemberId);
        var hasParsed = Guid.TryParse(memberId, out Guid value);
        return hasParsed ? value : Guid.Empty;
    }
}
