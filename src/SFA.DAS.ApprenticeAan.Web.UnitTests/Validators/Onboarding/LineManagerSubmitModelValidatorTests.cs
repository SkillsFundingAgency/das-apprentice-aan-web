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

        [TestCase(true, true)]
        [TestCase(false, false)]
        public void HasEmployersApproval_Valid_NoErrors(bool value, bool isValid)
        {
            var model = new LineManagerSubmitModel { HasEmployersApproval = value };
            var sut = new LineManagerSubmitModelValidator();

            var result = sut.TestValidate(model);

            if (isValid)
                result.ShouldNotHaveValidationErrorFor(c => c.HasEmployersApproval);
            else
                result.ShouldHaveValidationErrorFor(c => c.HasEmployersApproval).WithErrorMessage(LineManagerSubmitModelValidator.NoApprovalErrorMessage);
        }
    }
}