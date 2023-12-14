using FluentValidation.TestHelper;
using SFA.DAS.Aan.SharedUi.Models.EditApprenticeshipInformation;
using SFA.DAS.ApprenticeAan.Web.Validators.EditApprenticeshipInformation;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Validators.EditorApprenticeshipInformation.EmployerDetailsSubmitModelValidatorTests
{
    [TestFixture]
    public class EmployerTownOrCityTests
    {
        [TestCase("London", null, true)]
        [TestCase("", SubmitApprenticeshipInformationModelValidator.TownOrCityEmptyMessage, false)]
        [TestCase(null, SubmitApprenticeshipInformationModelValidator.TownOrCityEmptyMessage, false)]
        [TestCase(" ", SubmitApprenticeshipInformationModelValidator.TownOrCityEmptyMessage, false)]
        [TestCase("+", SubmitApprenticeshipInformationModelValidator.TownOrCityHasExcludedCharacter, false)]
        public void Validates_EmployerTownOrCity_NotNull(string? employerTownOrCity, string? errorMessage, bool isValid)
        {
            var sut = new SubmitApprenticeshipInformationModelValidator();

            var result = sut.TestValidate(new SubmitApprenticeshipInformationModel { EmployerTownOrCity = employerTownOrCity });

            if (isValid)
                result.ShouldNotHaveValidationErrorFor(c => c.EmployerTownOrCity);
            else
                result.ShouldHaveValidationErrorFor(x => x.EmployerTownOrCity)
                    .WithErrorMessage(errorMessage);
        }

        [TestCase(5, true)]
        [TestCase(201, false)]
        public void Validates_EmployerTownOrCity_Length(int length, bool isValid)
        {
            var sut = new SubmitApprenticeshipInformationModelValidator();

            var result = sut.TestValidate(new SubmitApprenticeshipInformationModel { EmployerTownOrCity = new string('a', length) });

            if (isValid)
                result.ShouldNotHaveValidationErrorFor(c => c.EmployerTownOrCity);
            else
                result.ShouldHaveValidationErrorFor(x => x.EmployerTownOrCity)
                    .WithErrorMessage(SubmitApprenticeshipInformationModelValidator.TownOrCityMaxLengthMessage);
        }
    }
}