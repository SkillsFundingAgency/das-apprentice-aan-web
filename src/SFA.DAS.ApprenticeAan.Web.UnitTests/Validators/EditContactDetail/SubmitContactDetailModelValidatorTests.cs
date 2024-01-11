using FluentValidation.TestHelper;
using SFA.DAS.Aan.SharedUi.Models.EditContactDetail;
using SFA.DAS.ApprenticeAan.Web.Validators.EditContactDetail;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Validators.EditContactDetail;

[TestFixture]
public class SubmitContactDetailModelValidatorTests
{
    [Test]
    public void Validate_LinkedinUrlIsLongerThanAllowableCharacters_Invalid()
    {
        var model = new SubmitContactDetailModel
        {
            LinkedinUrl = new string('x', 110)
        };

        var sut = new SubmitContactDetailModelValidator();
        var result = sut.TestValidate(model);

        result.ShouldHaveValidationErrorFor(c => c.LinkedinUrl)
            .WithErrorMessage(SubmitContactDetailModelValidator.LinkedinUrlLengthValidationMessage);
    }

    [Test]
    public void Validate_LinkedinUrlIsSmallerThanAllowableCharacters_Invalid()
    {
        var model = new SubmitContactDetailModel
        {
            LinkedinUrl = new string('x', 2)
        };

        var sut = new SubmitContactDetailModelValidator();
        var result = sut.TestValidate(model);

        result.ShouldHaveValidationErrorFor(c => c.LinkedinUrl)
            .WithErrorMessage(SubmitContactDetailModelValidator.LinkedinUrlLengthValidationMessage);
    }

    [Test]
    public void LinkedinUrl_BoundaryCheck_MaximumLength()
    {
        var model = new SubmitContactDetailModel
        {
            LinkedinUrl = new string('x', 100)
        };

        var sut = new SubmitContactDetailModelValidator();
        var result = sut.TestValidate(model);

        result.ShouldNotHaveValidationErrorFor(c => c.LinkedinUrl);
    }

    [Test]
    public void LinkedinUrl_BoundaryCheck_MinimumLength()
    {
        var model = new SubmitContactDetailModel
        {
            LinkedinUrl = new string('x', 3)
        };

        var sut = new SubmitContactDetailModelValidator();
        var result = sut.TestValidate(model);

        result.ShouldNotHaveValidationErrorFor(c => c.LinkedinUrl);
    }

    [Test]
    public void LinkedinUrl_RegexCheckWithInvalidCharacters_Invalid()
    {
        string linkedinUrl = "test test@";
        var model = new SubmitContactDetailModel
        {
            LinkedinUrl = linkedinUrl
        };

        var sut = new SubmitContactDetailModelValidator();
        var result = sut.TestValidate(model);

        result.ShouldHaveValidationErrorFor(c => c.LinkedinUrl).WithErrorMessage(SubmitContactDetailModelValidator.LinkedinUrlPatternValidationMessage);
    }

    [Test]
    public void LinkedinUrl_RegexCheckWithValidCharacters_Valid()
    {
        var model = new SubmitContactDetailModel
        {
            LinkedinUrl = new string('x', 8)
        };

        var sut = new SubmitContactDetailModelValidator();
        var result = sut.TestValidate(model);

        result.ShouldNotHaveValidationErrorFor(c => c.LinkedinUrl);
    }

    [Test]
    public void Validate_LinkedinUrlIsNull_Invalid()
    {
        var model = new SubmitContactDetailModel
        {
            LinkedinUrl = null
        };

        var sut = new SubmitContactDetailModelValidator();
        var result = sut.TestValidate(model);

        result.ShouldHaveValidationErrorFor(c => c.LinkedinUrl)
            .WithErrorMessage(SubmitContactDetailModelValidator.LinkedinUrlLengthValidationMessage);
    }

    [Test]
    public void Validate_LinkedinUrlIsNotNull_Valid()
    {
        var model = new SubmitContactDetailModel
        {
            LinkedinUrl = new string('x', 5)
        };

        var sut = new SubmitContactDetailModelValidator();
        var result = sut.TestValidate(model);

        result.ShouldNotHaveValidationErrorFor(x => x.LinkedinUrl);
    }

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
            LinkedinUrl = new string('x', 100),
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
            LinkedinUrl = new string('x', 100),
            ShowLinkedinUrl = false
        };

        var sut = new SubmitContactDetailModelValidator();

        var result = sut.TestValidate(model);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void Validate_LinkedinUrlIsEmptyAndShowLinkedinUrlIsTrue_InValid()
    {
        var model = new SubmitContactDetailModel
        {
            LinkedinUrl = string.Empty,
            ShowLinkedinUrl = false
        };

        var sut = new SubmitContactDetailModelValidator();

        var result = sut.TestValidate(model);

        result.ShouldHaveValidationErrorFor(c => c.LinkedinUrl)
    .WithErrorMessage(SubmitContactDetailModelValidator.LinkedinUrlLengthValidationMessage);
    }
}
