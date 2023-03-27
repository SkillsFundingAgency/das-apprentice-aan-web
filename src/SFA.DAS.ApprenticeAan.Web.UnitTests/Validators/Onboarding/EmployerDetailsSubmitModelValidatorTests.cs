using FluentValidation.TestHelper;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;
using SFA.DAS.ApprenticeAan.Web.Validators.Onboarding;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Validators.Onboarding
{
    [TestFixture]
    public class EmployerDetailsSubmitModelValidatorTests
    {
        [TestCase("Royal Mail", true)]
        [TestCase("", false)]
        [TestCase(null, false)]
        [TestCase(" ", false)]
        public void Validates_EmployerName_NotNull(string? employerName, bool isValid)
        {
            var sut = new EmployerDetailsSubmitModelValidator();

            var result = sut.TestValidate(new EmployerDetailsSubmitModel { EmployerName = employerName });

            if (isValid)
                result.ShouldNotHaveValidationErrorFor(c => c.EmployerName);
            else
                result.ShouldHaveValidationErrorFor(x => x.EmployerName)
                .WithErrorMessage(EmployerDetailsSubmitModelValidator.EmployerNameEmptyMessage);
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

        [TestCase("Farringdon Rd", true)]
        [TestCase("", false)]
        [TestCase(null, false)]
        [TestCase(" ", false)]
        public void Validates_AddressLine1_NotNull(string? addressLine1, bool isValid)
        {
            var sut = new EmployerDetailsSubmitModelValidator();

            var result = sut.TestValidate(new EmployerDetailsSubmitModel { AddressLine1 = addressLine1 });

            if (isValid)
                result.ShouldNotHaveValidationErrorFor(c => c.AddressLine1);
            else
                result.ShouldHaveValidationErrorFor(x => x.AddressLine1)
                .WithErrorMessage(EmployerDetailsSubmitModelValidator.AddressLine1EmptyMessage);
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
                    .WithErrorMessage(EmployerDetailsSubmitModelValidator.CountyMaxLengthMessage);
        }

        [TestCase("London", true)]
        [TestCase("", false)]
        [TestCase(null, false)]
        [TestCase(" ", false)]
        public void Validates_TownOrCity_NotNull(string? town, bool isValid)
        {
            var sut = new EmployerDetailsSubmitModelValidator();

            var result = sut.TestValidate(new EmployerDetailsSubmitModel { Town = town });

            if (isValid)
                result.ShouldNotHaveValidationErrorFor(c => c.Town);
            else
                result.ShouldHaveValidationErrorFor(x => x.Town)
                    .WithErrorMessage(EmployerDetailsSubmitModelValidator.TownOrCityEmptyMessage);
        }

        [TestCase(5, true)]
        [TestCase(201, false)]
        public void Validates_Town_Length(int length, bool isValid)
        {
            var sut = new EmployerDetailsSubmitModelValidator();

            var result = sut.TestValidate(new EmployerDetailsSubmitModel { Town = new string('a', length) });

            if (isValid)
                result.ShouldNotHaveValidationErrorFor(c => c.Town);
            else
                result.ShouldHaveValidationErrorFor(x => x.Town)
                    .WithErrorMessage(EmployerDetailsSubmitModelValidator.TownOrCityMaxLengthMessage);
        }

        [TestCase("", false)]
        [TestCase(null, false)]
        [TestCase(" ", false)]
        [TestCase("M60 1NW", true)]
        [TestCase("CR2 6HP", true)]
        [TestCase("DN55 1PT", true)]
        [TestCase("W1P 1HQ", true)]
        [TestCase("EC1A 1BB", true)]
        [TestCase("eC1A1bB", true)]
        public void Validate_Postcode_NullOrEmpty(string postcode, bool isValid)
        {
            var sut = new EmployerDetailsSubmitModelValidator();

            var result = sut.TestValidate(new EmployerDetailsSubmitModel { Postcode = postcode });

            if (isValid)
                result.ShouldNotHaveValidationErrorFor(c => c.Postcode);
            else
                result.ShouldHaveValidationErrorFor(c => c.Postcode).WithErrorMessage(EmployerDetailsSubmitModelValidator.PostcodeEmptyMessage);
        }

        [TestCase("M1", false)]
        [TestCase("M1 1AA", true)]
        public void Validate_Postcode_Bonafide(string postcode, bool isValid)
        {
            var sut = new EmployerDetailsSubmitModelValidator();

            var result = sut.TestValidate(new EmployerDetailsSubmitModel { Postcode = postcode });

            if (isValid)
                result.ShouldNotHaveValidationErrorFor(c => c.Postcode);
            else
                result.ShouldHaveValidationErrorFor(c => c.Postcode).WithErrorMessage(EmployerDetailsSubmitModelValidator.PostcodeInvalidMessage);
        }
    }
}