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
    public void EnteredJobTitle_LengthValidation_ErrorNoError(int? length, bool isValid)
    {
        CurrentJobTitleSubmitModel model;
        if (length != null)
            model = new CurrentJobTitleSubmitModel { JobTitle = new string('a', (int)length) };
        else
            model = new CurrentJobTitleSubmitModel { JobTitle = null };

        var sut = new CurrentJobTitleSubmitModelValidator();

        var result = sut.TestValidate(model);

        if (isValid)
            result.ShouldNotHaveValidationErrorFor(c => c.JobTitle);
        else
            result.ShouldHaveValidationErrorFor(c => c.JobTitle);
    }

    [TestCase("Support analyst", true)]
    [TestCase("", false)]
    [TestCase(null, false)]
    public void EnteredJobTitle_EmptyTextValidation_ErrorNoError(string? jobTitle, bool isValid)
    {
        CurrentJobTitleSubmitModel model = new() { JobTitle = jobTitle };

        var sut = new CurrentJobTitleSubmitModelValidator();

        var result = sut.TestValidate(model);

        if (isValid)
            result.ShouldNotHaveValidationErrorFor(c => c.JobTitle);
        else
            result.ShouldHaveValidationErrorFor(c => c.JobTitle).WithErrorMessage(CurrentJobTitleSubmitModelValidator.JobTitleEmpty);
    }

    [TestCase("Line1 support analyst", true)]
    [TestCase("Line 1 support analyst", true)]
    [TestCase("2nd Line support analyst", true)]
    [TestCase("2 nd Line support analyst", true)]
    [TestCase("£ 2nd Line support analyst", false)]
    [TestCase("2nd Line support analyst $", false)]
    [TestCase("2nd Line£ support analyst", false)]
    [TestCase("2nd Line $ support analyst", false)]
    [TestCase("Chief Technology Officer (CTO)", true)]
    public void EnteredJobTitle_AlphanumericValidation_ErrorNoError(string jobTitle, bool isValid)
    {
        var model = new CurrentJobTitleSubmitModel { JobTitle = jobTitle };

        var sut = new CurrentJobTitleSubmitModelValidator();

        var result = sut.TestValidate(model);

        if (isValid)
            result.ShouldNotHaveValidationErrorFor(c => c.JobTitle);
        else
            result.ShouldHaveValidationErrorFor(c => c.JobTitle).WithErrorMessage(CurrentJobTitleSubmitModelValidator.NotValidJobTitleErrorMessage);
    }
}