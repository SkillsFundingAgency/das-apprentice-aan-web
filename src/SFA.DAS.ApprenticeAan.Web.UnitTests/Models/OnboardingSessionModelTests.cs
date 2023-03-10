using SFA.DAS.ApprenticeAan.Web.Models;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Models;

[TestFixture]
public class OnboardingSessionModelTests
{
    [TestCase(null, true, true, false)]
    [TestCase("00000000-0000-0000-0000-000000000000", true, true, false)]
    [TestCase("bf92480f-1ef6-4552-8a5e-35d7951213f9", false, true, false)]
    [TestCase("bf92480f-1ef6-4552-8a5e-35d7951213f9", true, false, false)]
    [TestCase("bf92480f-1ef6-4552-8a5e-35d7951213f9", true, true, true)]
    [TestCase("bf92480f-1ef6-4552-8a5e-35d7951213f9", true, null, false)]
    public void IsValid_ChecksModelValidity(Guid? apprenticeId, bool hasAcceptedTAndCs, bool? hasEmployersApproval, bool expectedResult)
    {
        var model = new OnboardingSessionModel()
        {
            HasAcceptedTermsAndConditions = hasAcceptedTAndCs,
            HasEmployersApproval = hasEmployersApproval,
        };
        model.ApprenticeDetails.ApprenticeId = apprenticeId;

        var actualResult = model.IsValid;

        Assert.That(actualResult, Is.EqualTo(expectedResult));
    }
}