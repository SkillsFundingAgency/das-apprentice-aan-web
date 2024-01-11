using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using SFA.DAS.ApprenticePortal.Authentication;

namespace SFA.DAS.ApprenticeAan.Web.Extensions;

[ExcludeFromCodeCoverage]
public static class ClaimsPrincipalExtensions
{
    public static class ClaimTypes
    {
        public const string AanMemberId = "member_id";

        //This is defined in the login service, so it should be exact match
        public const string ApprenticeId = "apprentice_id";

    }

    public static void RemoveAanMemberIdClaim(this ClaimsPrincipal user, Guid memberId)
    {
        Claim? claim = null;

        foreach (var identity in user.Identities)
        {
            foreach (var c in identity.Claims)
            {
                if (c.Type == ClaimTypes.AanMemberId && c.Value == memberId.ToString())
                {
                    claim = c;
                }
            }

            if (claim == null) continue;
            identity.RemoveClaim(claim);
            claim = null;
        }
    }

    public static Guid GetApprenticeId(this ClaimsPrincipal principal) => GetClaimValue(principal, IdentityClaims.ApprenticeId);

    private static Guid GetClaimValue(ClaimsPrincipal principal, string claimType)
    {
        var memberId = principal.FindFirstValue(claimType);
        var hasParsed = Guid.TryParse(memberId, out Guid value);
        return hasParsed ? value : Guid.Empty;
    }
}
