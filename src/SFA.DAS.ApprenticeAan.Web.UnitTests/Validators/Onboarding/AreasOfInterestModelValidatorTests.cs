using FluentValidation.TestHelper;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;
using SFA.DAS.ApprenticeAan.Web.Validators.Onboarding;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Validators.Onboarding;

[TestFixture]
public class AreasOfInterestModelValidatorTests
{
    [Test]
    public void NoSelection_Validation_Error()
    {
        var model = new AreasOfInterestSubmitModel
        {
            Events = new List<SelectProfileModel> { new SelectProfileModel { Id = 1, IsSelected = false } },
            Promotions = new List<SelectProfileModel> { new SelectProfileModel { Id = 2, IsSelected = false } }
        };

        var sut = new AreasOfInterestSubmitModelValidator();
        var result = sut.TestValidate(model);

        result.ShouldHaveValidationErrorFor(c => c.AreasOfInterest).WithErrorMessage(AreasOfInterestSubmitModelValidator.NoSelectionErrorMessage);
    }

    [Test]
    public void EventsSelection_Validation_NoError()
    {
        var model = new AreasOfInterestSubmitModel
        {
            Events = new List<SelectProfileModel> { new SelectProfileModel { Id = 1, IsSelected = true } },
            Promotions = new List<SelectProfileModel> { new SelectProfileModel { Id = 2, IsSelected = false } }
        };

        var sut = new AreasOfInterestSubmitModelValidator();
        var result = sut.TestValidate(model);

        result.ShouldNotHaveValidationErrorFor(c => c.AreasOfInterest);
    }

    [Test]
    public void PromotionsSelection_Validation_NoError()
    {
        var model = new AreasOfInterestSubmitModel
        {
            Events = new List<SelectProfileModel> { new SelectProfileModel { Id = 1, IsSelected = false } },
            Promotions = new List<SelectProfileModel> { new SelectProfileModel { Id = 2, IsSelected = true } }
        };

        var sut = new AreasOfInterestSubmitModelValidator();
        var result = sut.TestValidate(model);

        result.ShouldNotHaveValidationErrorFor(c => c.AreasOfInterest);
    }

    [Test]
    public void EventsAndPromotionsSelection_Validation_NoError()
    {
        var model = new AreasOfInterestSubmitModel
        {
            Events = new List<SelectProfileModel> { new SelectProfileModel { Id = 1, IsSelected = true } },
            Promotions = new List<SelectProfileModel> { new SelectProfileModel { Id = 2, IsSelected = true } }
        };

        var sut = new AreasOfInterestSubmitModelValidator();
        var result = sut.TestValidate(model);

        result.ShouldNotHaveValidationErrorFor(c => c.AreasOfInterest);
    }
}