using FluentValidation.TestHelper;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;
using SFA.DAS.ApprenticeAan.Web.Validators.Onboarding;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Validators.Onboarding.EmployerDetailsSubmitModelValidatorTests
{
    [TestFixture]
    public class CountyTests
    {
        [TestCase(5, true)]
        [TestCase(201, false)]
        public void Validates_County_Length(int length, bool isValid)
        {
            var sut = new EmployerDetailsSubmitModelValidator();

            var result = sut.TestValidate(new EmployerDetailsSubmitModel { County = new string('a', length) });

            if (isValid)
                result.ShouldNotHaveValidationErrorFor(c => c.County);
            else
                result.ShouldHaveValidationErrorFor(x => x.County)
                    .WithErrorMessage(EmployerDetailsSubmitModelValidator.AddressLine3MaxLengthMessage);
        }

        [TestCase(null, true)]
        [TestCase("\\", false)]
        public void Validates_County_Input(string? county, bool isValid)
        {
            var sut = new EmployerDetailsSubmitModelValidator();

            var result = sut.TestValidate(new EmployerDetailsSubmitModel { County = county });

            if (isValid)
                result.ShouldNotHaveValidationErrorFor(c => c.County);
            else
                result.ShouldHaveValidationErrorFor(x => x.County)
                .WithErrorMessage(EmployerDetailsSubmitModelValidator.AddressLine3HasExcludedCharacter);
        }
    }
}