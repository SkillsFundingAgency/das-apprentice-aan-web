using SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses;
using SFA.DAS.ApprenticePortal.Authentication;
using System.Security.Claims;

namespace SFA.DAS.ApprenticeAan.Web.Helpers
{
    public static class ClaimsHelper
    {
        public static bool IsClaimsMismatch(Apprentice apprentice, ClaimsPrincipal claimPrincipal)
        {
            return CheckValueMismatch(apprentice.FirstName, QueryClaimsByType(IdentityClaims.GivenName, claimPrincipal.Claims)) ||

                 CheckValueMismatch(apprentice.LastName, QueryClaimsByType(IdentityClaims.FamilyName, claimPrincipal.Claims));
        }

        public static string[] QueryClaimsByType(string type, IEnumerable<Claim> claims)
        {
            return claims.Where(a => a.Type == type).Select(a => a.Value).ToArray();
        }

        private static bool CheckValueMismatch(string apprenticeValue, string[] identityValues)
        {
            return Array.Exists(identityValues, a => !string.Equals(a, apprenticeValue, StringComparison.Ordinal));
        }

        public static string? GetMismatchValue(string apprenticeValue, ClaimsPrincipal claimPrincipal, string claimType)
        {
            return QueryClaimsByType(claimType, claimPrincipal.Claims).LastOrDefault(a => !string.Equals(a, apprenticeValue, StringComparison.Ordinal));
        }
    }
}
