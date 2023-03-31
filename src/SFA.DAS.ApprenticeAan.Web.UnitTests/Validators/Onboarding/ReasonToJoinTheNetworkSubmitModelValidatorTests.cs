using FluentValidation.TestHelper;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;
using SFA.DAS.ApprenticeAan.Web.Validators.Onboarding;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Validators.Onboarding
{
    [TestFixture]
    public class ReasonToJoinTheNetworkSubmitModelValidatorTests
    {
        [TestCase(true, true)]
        [TestCase(false, true)]
        [TestCase(null, false)]
        public void EngagedWithAPreviousAmbassadorInTheNetwork_IsValid_Selection(bool? value, bool isValid)
        {
            var model = new ReasonToJoinTheNetworkSubmitModel { EngagedWithAPreviousAmbassadorInTheNetwork = value };
            var sut = new ReasonToJoinTheNetworkSubmitModelValidator();

            var result = sut.TestValidate(model);

            if (isValid)
                result.ShouldNotHaveValidationErrorFor(c => c.EngagedWithAPreviousAmbassadorInTheNetwork);
            else
                result.ShouldHaveValidationErrorFor(x => x.EngagedWithAPreviousAmbassadorInTheNetwork).WithErrorMessage(ReasonToJoinTheNetworkSubmitModelValidator.NoSelectionErrorMessage);
        }
    }
}