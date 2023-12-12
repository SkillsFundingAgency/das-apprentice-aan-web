using FluentValidation.TestHelper;
using SFA.DAS.Aan.SharedUi.Models.EditContactDetail;
using SFA.DAS.ApprenticeAan.Web.Validators.EditContactDetail;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Validators.EditContactDetail;

[TestFixture]
public class SubmitContactDetailCommandValidatorTests
{
    [Test]
    public void Validate_LinkedinUrlIsEmptyAndShowLinkedinUrlIsTrue_Invalid()
    {
        var model = new SubmitContactDetailModel
        {
            LinkedinUrl = string.Empty,
            ShowLinkedinUrl = true
        };

        var sut = new SubmitContactDetailModelValidator();

        var result = sut.TestValidate(model);

        result.ShouldHaveValidationErrorFor(c => c.ShowLinkedinUrl)
            .WithErrorMessage(SubmitContactDetailModelValidator.ShowLinkedinUrlValidationMessage);
    }

    [Test]
    public void Validate_LinkedinUrlHasValueAndShowLinkedinUrlIsTrue_Valid()
    {
        var model = new SubmitContactDetailModel
        {
            LinkedinUrl = new string('x', 200),
            ShowLinkedinUrl = true
        };

        var sut = new SubmitContactDetailModelValidator();

        var result = sut.TestValidate(model);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void Validate_LinkedinUrlHasValueAndShowLinkedinUrlIsFalse_Valid()
    {
        var model = new SubmitContactDetailModel
        {
            LinkedinUrl = new string('x', 200),
            ShowLinkedinUrl = false
        };

        var sut = new SubmitContactDetailModelValidator();

        var result = sut.TestValidate(model);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void Validate_LinkedinUrlIsEmptyAndShowLinkedinUrlIsTrue_Valid()
    {
        var model = new SubmitContactDetailModel
        {
            LinkedinUrl = string.Empty,
            ShowLinkedinUrl = false
        };

        var sut = new SubmitContactDetailModelValidator();

        var result = sut.TestValidate(model);

        result.ShouldNotHaveAnyValidationErrors();
    }
}
