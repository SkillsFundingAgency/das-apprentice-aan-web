﻿using FluentValidation.TestHelper;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;
using SFA.DAS.ApprenticeAan.Web.Validators.Onboarding;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Validators.Onboarding;

[TestFixture]
public class RegionsSubmitModelValidatorTests
{
    [TestCase(1, true)]
    [TestCase(null, false)]
    public void SelectedRegion_Validation_ErrorNoError(int? value, bool isValid)
    {
        var model = new RegionsSubmitModel { SelectedRegionId = value };
        var sut = new RegionsSubmitModelValidator();

        var result = sut.TestValidate(model);

        if (isValid)
            result.ShouldNotHaveValidationErrorFor(c => c.SelectedRegionId);
        else
            result.ShouldHaveValidationErrorFor(c => c.SelectedRegionId).WithErrorMessage(RegionsSubmitModelValidator.NoSelectionErrorMessage);
    }
}