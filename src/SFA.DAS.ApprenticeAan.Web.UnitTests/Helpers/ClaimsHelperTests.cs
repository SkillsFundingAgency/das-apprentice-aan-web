using AutoFixture.NUnit3;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses;
using SFA.DAS.ApprenticeAan.Web.Helpers;
using SFA.DAS.ApprenticePortal.Authentication;
using System.Security.Claims;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Helpers
{
    public class ClaimsHelperTests
    {
        [Test]
        [AutoData]
        public void IsClaimsMismatch_ReturnsMisMatchOnGivenName(Apprentice apprentice)
        {
            apprentice.LastName = nameof(IdentityClaims.FamilyName);

            var claimsPrincipal = CreateClaimsPrinciple();

            var isClaimsMismatch = ClaimsHelper.IsClaimsMismatch(apprentice, claimsPrincipal);

            Assert.True(isClaimsMismatch);
        }

        [Test]
        [AutoData]
        public void IsClaimsMismatch_ReturnsMisMatchOnLastName(Apprentice apprentice)
        {
            apprentice.FirstName = nameof(IdentityClaims.GivenName);

            var claimsPrincipal = CreateClaimsPrinciple();

            var isClaimsMismatch = ClaimsHelper.IsClaimsMismatch(apprentice, claimsPrincipal);

            Assert.True(isClaimsMismatch);
        }

        [Test]
        [AutoData]
        public void QueryClaimsByType_ReturnsCorrectType(Apprentice apprentice)
        {
            apprentice.FirstName = nameof(IdentityClaims.GivenName);

            var claimsPrincipal = CreateClaimsPrinciple();

            var claimValues = ClaimsHelper.QueryClaimsByType(IdentityClaims.GivenName, claimsPrincipal.Claims);

            Assert.Contains(nameof(IdentityClaims.GivenName), claimValues);
        }

        [Test]
        [AutoData]
        public void GetMismatchValue_ReturnsMisMatchOnGivenName(Apprentice apprentice)
        {
            var claimsPrincipal = CreateClaimsPrinciple();

            var mismatchValue = ClaimsHelper.GetMismatchValue(apprentice.FirstName, claimsPrincipal, IdentityClaims.GivenName);

            Assert.That(mismatchValue ?? string.Empty, Is.EqualTo(nameof(IdentityClaims.GivenName)));
        }

        private static ClaimsPrincipal CreateClaimsPrinciple(bool excludeGivenName = false, bool excludeFamilyName = false)
        {
            List<Claim> claims = new List<Claim>();

            if(!excludeGivenName)
            {
                claims.Add(new(IdentityClaims.GivenName, nameof(IdentityClaims.GivenName)));
            }

            if (!excludeGivenName)
            {
                claims.Add(new(IdentityClaims.FamilyName, nameof(IdentityClaims.FamilyName)));
            }

            ClaimsPrincipal claimsPrincipal = new();

            claimsPrincipal.AddIdentity(new ClaimsIdentity(claims));

            return claimsPrincipal;
        }
    }
}
