using SFA.DAS.GovUK.Auth.Models;

namespace SFA.DAS.ApprenticeAan.Web.Models;

public class StubAuthenticationViewModel :StubAuthUserDetails
{
    public string ReturnUrl { get; set; }
}