using FluentValidation.TestHelper;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;
using SFA.DAS.ApprenticeAan.Web.Validators.Onboarding;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Validators.Onboarding;

[TestFixture]
public class AreasOfInterestModelValidatorTests
{
    [Test]
    public void Validate_NoSelection_Invalid()
    {
        var model = new AreasOfInterestSubmitModel
        {
            Events = [new SelectProfileModel { Id = 1, IsSelected = false }],
            Promotions = [new SelectProfileModel { Id = 2, IsSelected = false }]
        };

        var sut = new AreasOfInterestSubmitModelValidator();
        var result = sut.TestValidate(model);

        result.ShouldHaveValidationErrorFor(c => c.AreasOfInterest).WithErrorMessage(AreasOfInterestSubmitModelValidator.NoSelectionErrorMessage);
    }

    [Test]
    public void Validate_EventSelected_Valid()
    {
        var model = new AreasOfInterestSubmitModel
        {
            Events = [new SelectProfileModel { Id = 1, IsSelected = true }],
            Promotions = [new SelectProfileModel { Id = 2, IsSelected = false }]
        };

        var sut = new AreasOfInterestSubmitModelValidator();
        var result = sut.TestValidate(model);

        result.ShouldNotHaveValidationErrorFor(c => c.AreasOfInterest);
    }

    [Test]
    public void Validate_PromotionSelected_Valid()
    {
        var model = new AreasOfInterestSubmitModel
        {
            Events = [new SelectProfileModel { Id = 1, IsSelected = false }],
            Promotions = [new SelectProfileModel { Id = 2, IsSelected = true }]
        };

        var sut = new AreasOfInterestSubmitModelValidator();
        var result = sut.TestValidate(model);

        result.ShouldNotHaveValidationErrorFor(c => c.AreasOfInterest);
    }

    [Test]
    public void Validate_EventAndPromotionSelected_Valid()
    {
        var model = new AreasOfInterestSubmitModel
        {
            Events = [new SelectProfileModel { Id = 1, IsSelected = true }],
            Promotions = [new SelectProfileModel { Id = 2, IsSelected = true }]
        };

        var sut = new AreasOfInterestSubmitModelValidator();
        var result = sut.TestValidate(model);

        result.ShouldNotHaveValidationErrorFor(c => c.AreasOfInterest);
    }
}