using FluentValidation.TestHelper;
using SFA.DAS.Aan.SharedUi.Models;
using SFA.DAS.ApprenticeAan.Web.Validators.MemberProfile;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Validators.NetworkEvents;

[TestFixture]
public class SubmitPersonalDetailCommandValidatorTests
{
    [Test]
    public void Validate_BiographyIsLongerThenAllowableCharacters_Invalid()
    {
        var model = new SubmitPersonalDetailCommand
        {
            Biography = new string('x', 550),
            JobTitle = "JobTitle"
        };

        var sut = new SubmitPersonalDetailCommandValidator();
        var result = sut.TestValidate(model);

        result.ShouldHaveValidationErrorFor(c => c.Biography)
            .WithErrorMessage(SubmitPersonalDetailCommandValidator.BiographyValidationMessage);
    }

    [Test]
    public void Validate_BiographyIsNotNull_Valid()
    {
        var model = new SubmitPersonalDetailCommand
        {
            Biography = "Biography",
            JobTitle = "JobTitle"
        };

        var sut = new SubmitPersonalDetailCommandValidator();
        var result = sut.TestValidate(model);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void Validate_JobTitleIsLongerThenAllowableCharacters_Invalid()
    {
        var model = new SubmitPersonalDetailCommand
        {
            JobTitle = new string('x', 250)
        };

        var sut = new SubmitPersonalDetailCommandValidator();
        var result = sut.TestValidate(model);

        result.ShouldHaveValidationErrorFor(c => c.JobTitle)
            .WithErrorMessage(SubmitPersonalDetailCommandValidator.JobTitleValidationMessage);
    }

    [Test]
    public void Validate_JobTitleIsNotNull_Valid()
    {
        var model = new SubmitPersonalDetailCommand
        {
            JobTitle = SubmitPersonalDetailCommandValidator.JobTitleValidationMessage
        };

        var sut = new SubmitPersonalDetailCommandValidator();
        var result = sut.TestValidate(model);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [MoqInlineAutoData("test")]
    [MoqInlineAutoData(null)]
    public void Validate_JobTitleRequired(string? jobTitle)
    {
        var model = new SubmitPersonalDetailCommand
        {
            JobTitle = jobTitle
        };

        var sut = new SubmitPersonalDetailCommandValidator();
        var result = sut.TestValidate(model);

        if (string.IsNullOrEmpty(jobTitle))
        {
            result.ShouldHaveValidationErrorFor(x => x.JobTitle);
        }
        else
        {
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
