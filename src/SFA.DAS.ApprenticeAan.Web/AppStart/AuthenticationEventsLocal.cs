using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticePortal.Authentication;

namespace SFA.DAS.ApprenticeAan.Web.AppStart;

[ExcludeFromCodeCoverage]
public class AuthenticationEventsLocal : OpenIdConnectEvents
{
    private readonly IApprenticeAccountService _apprenticeAccountService;

    public AuthenticationEventsLocal(IApprenticeAccountService apprenticeAccountService)
    {
        _apprenticeAccountService = apprenticeAccountService;
    }

    public override async Task TokenValidated(TokenValidatedContext context)
    {
        await base.TokenValidated(context);
        await AddClaims(context.Principal!);
    }

    public async Task AddClaims(ClaimsPrincipal principal)
    {
        var claim = principal.ApprenticeIdClaim();

        if (!Guid.TryParse(claim?.Value, out var apprenticeId)) return;

        var apprentice = await _apprenticeAccountService.GetApprenticeAccountDetails(apprenticeId);

        if (apprentice == null) return;

        AddApprenticeAccountClaims(principal, apprentice);
    }

    private static void AddNameClaims(ClaimsPrincipal principal, IApprenticeAccount apprentice)
    {
        principal.AddIdentity(new ClaimsIdentity(new[]
        {
                new Claim(IdentityClaims.GivenName, apprentice.FirstName),
                new Claim(IdentityClaims.FamilyName, apprentice.LastName),
            }));
    }

    private static void AddApprenticeAccountClaims(ClaimsPrincipal principal, IApprenticeAccount apprentice)
    {
        principal.AddAccountCreatedClaim();

        AddNameClaims(principal, apprentice);

        if (apprentice.TermsOfUseAccepted)
            principal.AddTermsOfUseAcceptedClaim();
    }
}