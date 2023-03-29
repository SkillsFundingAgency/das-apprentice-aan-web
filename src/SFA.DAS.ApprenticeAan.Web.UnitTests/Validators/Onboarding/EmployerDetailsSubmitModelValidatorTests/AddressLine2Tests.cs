using FluentValidation.TestHelper;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;
using SFA.DAS.ApprenticeAan.Web.Validators.Onboarding;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Validators.Onboarding.EmployerDetailsSubmitModelValidatorTests
{
    [TestFixture]
    public class AddressLine2Tests
    {
        [TestCase(5, true)]
        [TestCase(201, false)]
        public void Validates_AddressLine2_Length(int length, bool isValid)
        {
            var sut = new EmployerDetailsSubmitModelValidator();

            var result = sut.TestValidate(new EmployerDetailsSubmitModel { AddressLine2 = new string('a', length) });

            if (isValid)
                result.ShouldNotHaveValidationErrorFor(c => c.AddressLine2);
            else
                result.ShouldHaveValidationErrorFor(x => x.AddressLine2)
                    .WithErrorMessage(EmployerDetailsSubmitModelValidator.AddressLine2MaxLengthMessage);
        }

        [TestCase(null, true)]
        [TestCase("<", false)]
        public void Validates_AddressLine2_Input(string? addressLine2, bool isValid)
        {
            var sut = new EmployerDetailsSubmitModelValidator();

            var result = sut.TestValidate(new EmployerDetailsSubmitModel { AddressLine2 = addressLine2 });

            if (isValid)
                result.ShouldNotHaveValidationErrorFor(c => c.AddressLine2);
            else
                result.ShouldHaveValidationErrorFor(x => x.AddressLine2)
                .WithErrorMessage(EmployerDetailsSubmitModelValidator.AddressLine2HasExcludedCharacter);
        }
    }
}