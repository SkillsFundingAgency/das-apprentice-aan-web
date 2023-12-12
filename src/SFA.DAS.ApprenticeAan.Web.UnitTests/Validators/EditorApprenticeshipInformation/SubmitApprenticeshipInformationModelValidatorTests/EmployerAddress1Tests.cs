using FluentValidation.TestHelper;
using SFA.DAS.Aan.SharedUi.Models.EditApprenticeshipInformation;
using SFA.DAS.ApprenticeAan.Web.Validators.EditApprenticeshipInformation;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Validators.EditorApprenticeshipInformation.SubmitApprenticeshipInformationModelValidatorTests
{
    [TestFixture]
    public class EmployerAddress1Tests
    {
        [TestCase("Farringdon Rd", null, true)]
        [TestCase("", SubmitApprenticeshipInformationModelValidator.AddressLine1EmptyMessage, false)]
        [TestCase(null, SubmitApprenticeshipInformationModelValidator.AddressLine1EmptyMessage, false)]
        [TestCase(" ", SubmitApprenticeshipInformationModelValidator.AddressLine1EmptyMessage, false)]
        [TestCase("Farringdon=Rd", SubmitApprenticeshipInformationModelValidator.AddressLine1HasExcludedCharacter, false)]
        public void Validates_EmployerAddress1_Input(string? employerAddress1, string? errorMessage, bool isValid)
        {

            var sut = new SubmitApprenticeshipInformationModelValidator();

            var result = sut.TestValidate(new SubmitApprenticeshipInformationModel { EmployerAddress1 = employerAddress1 });

            if (isValid)
                result.ShouldNotHaveValidationErrorFor(c => c.EmployerAddress1);
            else
                result.ShouldHaveValidationErrorFor(x => x.EmployerAddress1)
                .WithErrorMessage(errorMessage);
        }

        [TestCase(5, true)]
        [TestCase(201, false)]
        public void Validates_EmployerAddress1_Length(int length, bool isValid)
        {
            var sut = new SubmitApprenticeshipInformationModelValidator();

            var result = sut.TestValidate(new SubmitApprenticeshipInformationModel { EmployerAddress1 = new string('a', length) });

            if (isValid)
                result.ShouldNotHaveValidationErrorFor(c => c.EmployerAddress1);
            else
                result.ShouldHaveValidationErrorFor(x => x.EmployerAddress1)
                    .WithErrorMessage(SubmitApprenticeshipInformationModelValidator.AddressLine1MaxLengthMessage);
        }
    }
}