using FluentValidation.TestHelper;
using SFA.DAS.Aan.SharedUi.Models.EditApprenticeshipInformation;
using SFA.DAS.ApprenticeAan.Web.Validators.EditApprenticeshipInformation;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Validators.EditorApprenticeshipInformation.SubmitApprenticeshipInformationModelValidatorTests
{
    [TestFixture]
    public class EmployerCountyTests
    {
        [TestCase(5, true)]
        [TestCase(201, false)]
        public void Validates_EmployerCounty_Length(int length, bool isValid)
        {
            var sut = new SubmitApprenticeshipInformationModelValidator();

            var result = sut.TestValidate(new SubmitApprenticeshipInformationModel { EmployerCounty = new string('a', length) });

            if (isValid)
                result.ShouldNotHaveValidationErrorFor(c => c.EmployerCounty);
            else
                result.ShouldHaveValidationErrorFor(x => x.EmployerCounty)
                    .WithErrorMessage(SubmitApprenticeshipInformationModelValidator.CountyMaxLengthMessage);
        }

        [TestCase(null, true)]
        [TestCase("\\", false)]
        public void Validates_EmployerCounty_Input(string? employerCounty, bool isValid)
        {
            var sut = new SubmitApprenticeshipInformationModelValidator();

            var result = sut.TestValidate(new SubmitApprenticeshipInformationModel { EmployerCounty = employerCounty });

            if (isValid)
                result.ShouldNotHaveValidationErrorFor(c => c.EmployerCounty);
            else
                result.ShouldHaveValidationErrorFor(x => x.EmployerCounty)
                .WithErrorMessage(SubmitApprenticeshipInformationModelValidator.CountyHasExcludedCharacter);
        }
    }
}