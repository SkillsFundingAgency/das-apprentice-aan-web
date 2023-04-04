using FluentValidation.TestHelper;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;
using SFA.DAS.ApprenticeAan.Web.Validators.Onboarding;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Validators.Onboarding.EmployerDetailsSubmitModelValidatorTests
{
    [TestFixture]
    public class AddressLine1Tests
    {
        [TestCase("Farringdon Rd", null, true)]
        [TestCase("", EmployerDetailsSubmitModelValidator.AddressLine1EmptyMessage, false)]
        [TestCase(null, EmployerDetailsSubmitModelValidator.AddressLine1EmptyMessage, false)]
        [TestCase(" ", EmployerDetailsSubmitModelValidator.AddressLine1EmptyMessage, false)]
        [TestCase("Farringdon=Rd", EmployerDetailsSubmitModelValidator.AddressLine1HasExcludedCharacter, false)]
        public void Validates_AddressLine1_Input(string? addressLine1, string? errorMessage, bool isValid)
        {
            var sut = new EmployerDetailsSubmitModelValidator();

            var result = sut.TestValidate(new EmployerDetailsSubmitModel { AddressLine1 = addressLine1 });

            if (isValid)
                result.ShouldNotHaveValidationErrorFor(c => c.AddressLine1);
            else
                result.ShouldHaveValidationErrorFor(x => x.AddressLine1)
                .WithErrorMessage(errorMessage);
        }

        [TestCase(5, true)]
        [TestCase(201, false)]
        public void Validates_AddressLine1_Length(int length, bool isValid)
        {
            var sut = new EmployerDetailsSubmitModelValidator();

            var result = sut.TestValidate(new EmployerDetailsSubmitModel { AddressLine1 = new string('a', length) });

            if (isValid)
                result.ShouldNotHaveValidationErrorFor(c => c.AddressLine1);
            else
                result.ShouldHaveValidationErrorFor(x => x.AddressLine1)
                    .WithErrorMessage(EmployerDetailsSubmitModelValidator.AddressLine1MaxLengthMessage);
        }
    }
}