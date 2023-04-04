using FluentValidation.TestHelper;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;
using SFA.DAS.ApprenticeAan.Web.Validators.Onboarding;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Validators.Onboarding.EmployerDetailsSubmitModelValidatorTests
{
    [TestFixture]
    public class TownOrCityTests
    {
        [TestCase("London", null, true)]
        [TestCase("", EmployerDetailsSubmitModelValidator.TownOrCityEmptyMessage, false)]
        [TestCase(null, EmployerDetailsSubmitModelValidator.TownOrCityEmptyMessage, false)]
        [TestCase(" ", EmployerDetailsSubmitModelValidator.TownOrCityEmptyMessage, false)]
        [TestCase("+", EmployerDetailsSubmitModelValidator.TownOrCityHasExcludedCharacter, false)]
        public void Validates_TownOrCity_NotNull(string? town, string? errorMessage, bool isValid)
        {
            var sut = new EmployerDetailsSubmitModelValidator();

            var result = sut.TestValidate(new EmployerDetailsSubmitModel { Town = town });

            if (isValid)
                result.ShouldNotHaveValidationErrorFor(c => c.Town);
            else
                result.ShouldHaveValidationErrorFor(x => x.Town)
                    .WithErrorMessage(errorMessage);
        }

        [TestCase(5, true)]
        [TestCase(201, false)]
        public void Validates_TownOrCity_Length(int length, bool isValid)
        {
            var sut = new EmployerDetailsSubmitModelValidator();

            var result = sut.TestValidate(new EmployerDetailsSubmitModel { Town = new string('a', length) });

            if (isValid)
                result.ShouldNotHaveValidationErrorFor(c => c.Town);
            else
                result.ShouldHaveValidationErrorFor(x => x.Town)
                    .WithErrorMessage(EmployerDetailsSubmitModelValidator.TownOrCityMaxLengthMessage);
        }
    }
}