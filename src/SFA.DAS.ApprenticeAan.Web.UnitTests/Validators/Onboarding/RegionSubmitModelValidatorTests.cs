using FluentValidation.TestHelper;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;
using SFA.DAS.ApprenticeAan.Web.Validators.Onboarding;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Validators.Onboarding
{
    [TestFixture]
    public class RegionSubmitModelValidatorTests
    {
        [TestCase(1, true)]
        [TestCase(null, false)]
        public void SelectedRegion_Validation_ErrorNoError(int? value, bool isValid)
        {
            var model = new RegionSubmitModel { SelectedRegionId = value };
            var sut = new RegionSubmitModelValidator();

            var result = sut.TestValidate(model);

            if (isValid)
                result.ShouldNotHaveValidationErrorFor(c => c.SelectedRegionId);
            else
                result.ShouldHaveValidationErrorFor(c => c.SelectedRegionId).WithErrorMessage(RegionSubmitModelValidator.NoSelectionErrorMessage);
        }
    }
}
