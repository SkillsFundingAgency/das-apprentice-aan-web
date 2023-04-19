using FluentValidation.TestHelper;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;
using SFA.DAS.ApprenticeAan.Web.Validators.Onboarding;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Validators.Onboarding
{
    [TestFixture]
    public class LineManagerSubmitModelValidatorTests
    {
        [Test]
        public void HasEmployersApproval_NoSelection_Null()
        {
            var model = new LineManagerSubmitModel { HasEmployersApproval = null };
            var sut = new LineManagerSubmitModelValidator();

            var result = sut.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.HasEmployersApproval).WithErrorMessage(LineManagerSubmitModelValidator.NoSelectionErrorMessage);
        }
    }
}