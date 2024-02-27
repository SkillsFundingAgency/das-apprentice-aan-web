using FluentValidation.TestHelper;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;
using SFA.DAS.ApprenticeAan.Web.Validators.Onboarding;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Validators.Onboarding.EmployerDetailsSubmitModelValidatorTests
{
    [TestFixture]
    public class PostcodeTests
    {
        [TestCase("", EmployerDetailsSubmitModelValidator.PostcodeEmptyMessage, false)]
        [TestCase(null, EmployerDetailsSubmitModelValidator.PostcodeEmptyMessage, false)]
        [TestCase(" ", EmployerDetailsSubmitModelValidator.PostcodeEmptyMessage, false)]
        [TestCase("M1", EmployerDetailsSubmitModelValidator.PostcodeInvalidMessage, false)]
        [TestCase("M60 1NW", null, true)]
        [TestCase("CR2 6HP", null, true)]
        [TestCase("DN55 1PT", null, true)]
        [TestCase("W1P 1HQ", null, true)]
        [TestCase("EC1A 1BB", null, true)]
        [TestCase("eC1A1bB", null, true)]
        public void Validate_Postcode_Input(string? postcode, string? errorMessage, bool isValid)
        {
            var sut = new EmployerDetailsSubmitModelValidator();

            var result = sut.TestValidate(new EmployerDetailsSubmitModel { Postcode = postcode });

            if (isValid)
                result.ShouldNotHaveValidationErrorFor(c => c.Postcode);
            else
                result.ShouldHaveValidationErrorFor(c => c.Postcode).WithErrorMessage(errorMessage);
        }
    }
}