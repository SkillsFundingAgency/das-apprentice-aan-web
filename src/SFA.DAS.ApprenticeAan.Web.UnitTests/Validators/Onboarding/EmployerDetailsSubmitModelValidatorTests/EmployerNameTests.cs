using FluentValidation.TestHelper;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;
using SFA.DAS.ApprenticeAan.Web.Validators.Onboarding;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Validators.Onboarding.EmployerDetailsSubmitModelValidatorTests
{
    [TestFixture]
    public class EmployerNameTests
    {
        [TestCase("Royal Mail", null, true)]
        [TestCase("", EmployerDetailsSubmitModelValidator.EmployerNameEmptyMessage, false)]
        [TestCase(null, EmployerDetailsSubmitModelValidator.EmployerNameEmptyMessage, false)]
        [TestCase(" ", EmployerDetailsSubmitModelValidator.EmployerNameEmptyMessage, false)]
        [TestCase("Royal@ Mail", EmployerDetailsSubmitModelValidator.EmployerNameHasExcludedCharacter, false)]
        public void Validates_EmployerName_Input(string? employerName, string? errorMessage, bool isValid)
        {
            var sut = new EmployerDetailsSubmitModelValidator();

            var result = sut.TestValidate(new EmployerDetailsSubmitModel { EmployerName = employerName });

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
            var sut = new EmployerDetailsSubmitModelValidator();

            var result = sut.TestValidate(new EmployerDetailsSubmitModel { EmployerName = new string('a', length) });

            if (isValid)
                result.ShouldNotHaveValidationErrorFor(c => c.EmployerName);
            else
                result.ShouldHaveValidationErrorFor(x => x.EmployerName)
                    .WithErrorMessage(EmployerDetailsSubmitModelValidator.EmployerNameMaxLengthMessage);
        }
   }
}