using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses;
using SFA.DAS.ApprenticeAan.Web.Extensions;
using SFA.DAS.ApprenticePortal.Authentication;

namespace SFA.DAS.ApprenticeAan.Web.Authentication;

[ExcludeFromCodeCoverage]
public class AuthenticationEventsLocal : OpenIdConnectEvents
{
    private readonly IApprenticeAccountService _apprenticeAccountService;
    private readonly IApprenticeService _apprenticesService;

    public AuthenticationEventsLocal(IApprenticeAccountService apprenticeAccountService, IApprenticeService apprenticesService)
    {
        _apprenticeAccountService = apprenticeAccountService;
        _apprenticesService = apprenticesService;
    }

    public override async Task TokenValidated(TokenValidatedContext context)
    {
        await base.TokenValidated(context);
        await AddClaims(context.Principal!);
    }

    public async Task AddClaims(ClaimsPrincipal principal)
    {
        // ApprenticeId is generated in login service so this should be present if user is authenticated
        var apprenticeId = principal.GetApprenticeId();
        var apprentice = await _apprenticeAccountService.GetApprenticeAccountDetails(apprenticeId);
        // user has not been through account registration
        // authorization filter will navigate user to accounts page
        if (apprentice == null) return;

        AddApprenticeAccountClaims(principal, apprentice);
    }

    private void AddApprenticeAccountClaims(ClaimsPrincipal principal, ApprenticeAccount apprentice)
    {
        principal.AddAccountCreatedClaim();

        AddNameClaims(principal, apprentice);
    }

    private static void AddNameClaims(ClaimsPrincipal principal, IApprenticeAccount apprentice)
    {
        principal.AddIdentity(new ClaimsIdentity(new[]
        {
            new Claim(IdentityClaims.GivenName, apprentice.FirstName),
            new Claim(IdentityClaims.FamilyName, apprentice.LastName),
        }));
    }
}
