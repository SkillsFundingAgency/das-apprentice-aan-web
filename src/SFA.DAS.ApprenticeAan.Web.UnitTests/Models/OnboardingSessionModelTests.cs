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
        Assert.That(actualResult, Is.EqualTo(model.ApprenticeDetails.ApprenticeId != null
            && model.ApprenticeDetails.ApprenticeId != Guid.Empty
            && model.HasAcceptedTermsAndConditions
            && model.HasEmployersApproval.GetValueOrDefault()));

        Assert.That(actualResult, Is.EqualTo(expectedResult));
    }

    [Test]
    public void IsValid_Validate()
    {

        var model = new OnboardingSessionModel()
        {
            HasAcceptedTermsAndConditions = true,
            HasEmployersApproval = true
        };
        model.ApprenticeDetails.ApprenticeId = Guid.NewGuid();

        Assert.Multiple(() =>
        {
            Assert.That(model.ApprenticeDetails, Is.Not.Null);
            Assert.That(model.ApprenticeDetails, Is.TypeOf<ApprenticeDetailsModel>());
            Assert.That(model.ApprenticeDetails, Is.InstanceOf<ApprenticeDetailsModel>());
            Assert.That(model.ApprenticeDetails.ApprenticeId, Is.Not.Null);
            Assert.That(model.ApprenticeDetails.ApprenticeId, Is.Not.EqualTo(Guid.Empty));
            Assert.That(model.HasAcceptedTermsAndConditions, Is.True);
            Assert.That(model.HasEmployersApproval.GetValueOrDefault(), Is.True);
        });
    }
}
