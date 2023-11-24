﻿using FluentValidation.TestHelper;
using SFA.DAS.Aan.SharedUi.Models;
using SFA.DAS.ApprenticeAan.Web.Validators.MemberProfile;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Validators.EditPersonalInformation;

[TestFixture]
public class SubmitPersonalDetailCommandValidatorTests
{
    [Test]
    public void Validate_BiographyIsLongerThenAllowableCharacters_Invalid()
    {
        var model = new SubmitPersonalDetailModel
        {
            Biography = new string('x', 550),
            JobTitle = "JobTitle"
        };

        var sut = new SubmitPersonalDetailModelValidator();
        var result = sut.TestValidate(model);

        result.ShouldHaveValidationErrorFor(c => c.Biography)
            .WithErrorMessage(SubmitPersonalDetailModelValidator.BiographyValidationMessage);
    }

    [Test]
    public void Validate_BiographyIsNotNull_Valid()
    {
        var model = new SubmitPersonalDetailModel
        {
            Biography = "Biography",
            JobTitle = "JobTitle"
        };

        var sut = new SubmitPersonalDetailModelValidator();
        var result = sut.TestValidate(model);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void Validate_JobTitleIsLongerThenAllowableCharacters_Invalid()
    {
        var model = new SubmitPersonalDetailModel
        {
            JobTitle = new string('x', 250)
        };

        var sut = new SubmitPersonalDetailModelValidator();
        var result = sut.TestValidate(model);

        result.ShouldHaveValidationErrorFor(c => c.JobTitle)
            .WithErrorMessage(SubmitPersonalDetailModelValidator.JobTitleValidationMessage);
    }

    [Test]
    public void Validate_JobTitleIsNotNull_Valid()
    {
        var model = new SubmitPersonalDetailModel
        {
            JobTitle = SubmitPersonalDetailModelValidator.JobTitleValidationMessage
        };

        var sut = new SubmitPersonalDetailModelValidator();
        var result = sut.TestValidate(model);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [MoqInlineAutoData("test")]
    [MoqInlineAutoData(null)]
    public void Validate_JobTitleRequired(string? jobTitle)
    {
        var model = new SubmitPersonalDetailModel
        {
            JobTitle = jobTitle
        };

        var sut = new SubmitPersonalDetailModelValidator();
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
