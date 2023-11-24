using FluentValidation.TestHelper;
using SFA.DAS.Aan.SharedUi.Models.EditAreaOfInterest;
using SFA.DAS.ApprenticeAan.Web.Validators.EditAreaOfInterest;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Validators.EditAreaOfInterest;

[TestFixture]
public class SubmitAreaOfInterestModelValidatorTests
{

    [Test]
    public void Validate_EventsAndPromotionsAreEmpty_Invalid()
    {
        var model = new SubmitAreaOfInterestModel
        {
            Events = new List<SelectProfileViewModel>(),
            Promotions = new List<SelectProfileViewModel>()
        };

        var sut = new SubmitAreaOfInterestModelValidator();
        var result = sut.TestValidate(model);

        result.ShouldHaveValidationErrorFor(c => c.AreasOfInterest)
            .WithErrorMessage(SubmitAreaOfInterestModelValidator.NoSelectionErrorMessage);
    }
}
