using FluentValidation.TestHelper;
using SFA.DAS.Aan.SharedUi.Models.EditApprenticeshipInformation;
using SFA.DAS.ApprenticeAan.Web.Validators.EditApprenticeshipInformation;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Validators.EditorApprenticeshipInformation.SubmitApprenticeshipInformationModelValidatorTests
{
    [TestFixture]
    public class EmployerAddress2Tests
    {
        [TestCase(5, true)]
        [TestCase(201, false)]
        public void Validates_EmployerAddress2_Length(int length, bool isValid)
        {
            var sut = new SubmitApprenticeshipInformationModelValidator();

            var result = sut.TestValidate(new SubmitApprenticeshipInformationModel { EmployerAddress2 = new string('a', length) });

            if (isValid)
                result.ShouldNotHaveValidationErrorFor(c => c.EmployerAddress2);
            else
                result.ShouldHaveValidationErrorFor(x => x.EmployerAddress2)
                    .WithErrorMessage(SubmitApprenticeshipInformationModelValidator.AddressLine2MaxLengthMessage);
        }

        [TestCase(null, true)]
        [TestCase("<", false)]
        public void Validates_EmployerAddress2_Input(string? addressLine2, bool isValid)
        {
            var sut = new SubmitApprenticeshipInformationModelValidator();

            var result = sut.TestValidate(new SubmitApprenticeshipInformationModel { EmployerAddress2 = addressLine2 });

            if (isValid)
                result.ShouldNotHaveValidationErrorFor(c => c.EmployerAddress2);
            else
                result.ShouldHaveValidationErrorFor(x => x.EmployerAddress2)
                .WithErrorMessage(SubmitApprenticeshipInformationModelValidator.AddressLine2HasExcludedCharacter);
        }
    }
}