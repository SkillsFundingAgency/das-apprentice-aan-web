using FluentValidation.TestHelper;
using SFA.DAS.Aan.SharedUi.Models.EditApprenticeshipInformation;
using SFA.DAS.ApprenticeAan.Web.Validators.EditApprenticeshipInformation;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Validators.EditorApprenticeshipInformation.SubmitApprenticeshipInformationModelValidatorTests
{
    [TestFixture]
    public class EmployerPostcodeTests
    {
        [TestCase("", SubmitApprenticeshipInformationModelValidator.PostcodeEmptyMessage, false)]
        [TestCase(null, SubmitApprenticeshipInformationModelValidator.PostcodeEmptyMessage, false)]
        [TestCase(" ", SubmitApprenticeshipInformationModelValidator.PostcodeEmptyMessage, false)]
        [TestCase("M1", SubmitApprenticeshipInformationModelValidator.PostcodeInvalidMessage, false)]
        [TestCase("M60 1NW", null, true)]
        [TestCase("CR2 6HP", null, true)]
        [TestCase("DN55 1PT", null, true)]
        [TestCase("W1P 1HQ", null, true)]
        [TestCase("EC1A 1BB", null, true)]
        [TestCase("eC1A1bB", null, true)]
        public void Validate_Postcode_Input(string? employerPostcode, string? errorMessage, bool isValid)
        {
            var sut = new SubmitApprenticeshipInformationModelValidator();

            var result = sut.TestValidate(new SubmitApprenticeshipInformationModel { EmployerPostcode = employerPostcode });

            if (isValid)
                result.ShouldNotHaveValidationErrorFor(c => c.EmployerPostcode);
            else
                result.ShouldHaveValidationErrorFor(c => c.EmployerPostcode).WithErrorMessage(errorMessage);
        }
    }
}