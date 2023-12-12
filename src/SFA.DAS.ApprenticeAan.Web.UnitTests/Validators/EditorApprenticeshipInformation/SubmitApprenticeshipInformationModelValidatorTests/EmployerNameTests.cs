using FluentValidation.TestHelper;
using SFA.DAS.Aan.SharedUi.Models.EditApprenticeshipInformation;
using SFA.DAS.ApprenticeAan.Web.Validators.EditApprenticeshipInformation;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Validators.EditorApprenticeshipInformation.SubmitApprenticeshipInformationModelValidatorTests
{
    [TestFixture]
    public class EmployerNameTests
    {
        [TestCase("Royal Mail", null, true)]
        [TestCase("", SubmitApprenticeshipInformationModelValidator.EmployerNameEmptyMessage, false)]
        [TestCase(null, SubmitApprenticeshipInformationModelValidator.EmployerNameEmptyMessage, false)]
        [TestCase(" ", SubmitApprenticeshipInformationModelValidator.EmployerNameEmptyMessage, false)]
        [TestCase("Royal@ Mail", SubmitApprenticeshipInformationModelValidator.EmployerNameHasExcludedCharacter, false)]
        public void Validates_EmployerName_Input(string? employerName, string? errorMessage, bool isValid)
        {
            var sut = new SubmitApprenticeshipInformationModelValidator();

            var result = sut.TestValidate(new SubmitApprenticeshipInformationModel { EmployerName = employerName });

            if (isValid)
                result.ShouldNotHaveValidationErrorFor(c => c.EmployerName);
            else
                result.ShouldHaveValidationErrorFor(x => x.EmployerName)
                .WithErrorMessage(errorMessage);
        }

        [TestCase(5, true)]
        [TestCase(201, false)]
        public void Validates_EmployerName_Length(int length, bool isValid)
        {
            var sut = new SubmitApprenticeshipInformationModelValidator();

            var result = sut.TestValidate(new SubmitApprenticeshipInformationModel { EmployerName = new string('a', length) });

            if (isValid)
                result.ShouldNotHaveValidationErrorFor(c => c.EmployerName);
            else
                result.ShouldHaveValidationErrorFor(x => x.EmployerName)
                    .WithErrorMessage(SubmitApprenticeshipInformationModelValidator.EmployerNameMaxLengthMessage);
        }
    }
}