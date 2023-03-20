using FluentValidation.TestHelper;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;
using SFA.DAS.ApprenticeAan.Web.Validators.Onboarding;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Validators.Onboarding;

[TestFixture]
public class CurrentJobTitleSubmitModelValidatorTests
{
    [TestCase("FirstName LastName", true)]
    [TestCase("Name having more than 200 characters, This is dummy characters to increase the text length. This is dummy characters to increase the text length. This is dummy characters to increase the text length. This is dummy characters to increase the text length.", false)]
    [TestCase("FirstName", true)]
    [TestCase(" ", false)]
    [TestCase(null, false)]
    public void SelectedRegion_Validation_ErrorNoError(string? value, bool isValid)
    {
        var model = new CurrentJobTitleSubmitModel { EnteredJobTitle = value };
        var sut = new CurrentJobTitleSubmitModelValidator();

        var result = sut.TestValidate(model);

        if (isValid)
            result.ShouldNotHaveValidationErrorFor(c => c.EnteredJobTitle);
        else
            result.ShouldHaveValidationErrorFor(c => c.EnteredJobTitle);
    }
}