using FluentValidation.TestHelper;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;
using SFA.DAS.ApprenticeAan.Web.Validators.Onboarding;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Validators.Onboarding;

[TestFixture]
public class CurrentJobTitleSubmitModelValidatorTests
{
    [TestCase(1, true)]
    [TestCase(200, true)]
    [TestCase(201, false)]
    [TestCase(null, false)]
    public void EnteredJobTitle_Validation_ErrorNoError(int? length, bool isValid)
    {
        CurrentJobTitleSubmitModel model;
        if (length != null)
            model = new CurrentJobTitleSubmitModel { EnteredJobTitle = new string('a', (int)length) };
        else
            model = new CurrentJobTitleSubmitModel { EnteredJobTitle = null };

        var sut = new CurrentJobTitleSubmitModelValidator();

        var result = sut.TestValidate(model);

        if (isValid)
            result.ShouldNotHaveValidationErrorFor(c => c.EnteredJobTitle);
        else
            result.ShouldHaveValidationErrorFor(c => c.EnteredJobTitle);
    }

    [TestCase("", false)]
    [TestCase("Line1 support analyst", true)]
    [TestCase("Line 1 support analyst", true)]
    [TestCase("2nd Line support analyst", true)]
    [TestCase("2 nd Line support analyst", true)]
    [TestCase("£ 2nd Line support analyst", false)]
    [TestCase("2nd Line support analyst $", false)]
    [TestCase("2nd Line£ support analyst", false)]
    [TestCase("2nd Line $ support analyst", false)]
    public void EnteredJobTitle_AlphanumericValidation_ErrorNoError(string jobTitle, bool isValid)
    {
        var model = new CurrentJobTitleSubmitModel { EnteredJobTitle = jobTitle };

        var sut = new CurrentJobTitleSubmitModelValidator();

        var result = sut.TestValidate(model);

        if (isValid)
            result.ShouldNotHaveValidationErrorFor(c => c.EnteredJobTitle);
        else
            result.ShouldHaveValidationErrorFor(c => c.EnteredJobTitle);
    }
}