using FluentValidation.TestHelper;
using SFA.DAS.Aan.SharedUi.Models;
using SFA.DAS.Aan.SharedUi.Models.AmbassadorProfile;
using SFA.DAS.ApprenticeAan.Web.Validators.MemberProfile;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Validators.NetworkEvents;

[TestFixture]
public class SubmitPersonalDetailCommandValidatorTests
{
    [Test]
    public void Validate_BiographyIsNull_Invalid()
    {
        var model = new SubmitPersonalDetailCommand
        {
            Biography = new string('x', 550)
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
            Biography = SubmitPersonalDetailCommandValidator.BiographyValidationMessage,
            UserType = Aan.SharedUi.Models.AmbassadorProfile.MemberUserType.Employer
        };

        var sut = new SubmitPersonalDetailCommandValidator();
        var result = sut.TestValidate(model);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void Validate_JobTitleIsNull_Invalid()
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
            JobTitle = SubmitPersonalDetailCommandValidator.JobTitleValidationMessage,
            UserType = Aan.SharedUi.Models.AmbassadorProfile.MemberUserType.Employer
        };

        var sut = new SubmitPersonalDetailCommandValidator();
        var result = sut.TestValidate(model);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [MoqInlineAutoData("test", MemberUserType.Apprentice)]
    [MoqInlineAutoData(null, MemberUserType.Apprentice)]
    [MoqInlineAutoData("test", MemberUserType.Employer)]
    [MoqInlineAutoData(null, MemberUserType.Employer)]
    public void Validate_JobTitleRequired(string? jobTitle, MemberUserType memberUserType)
    {
        var model = new SubmitPersonalDetailCommand
        {
            JobTitle = jobTitle,
            UserType = memberUserType
        };

        var sut = new SubmitPersonalDetailCommandValidator();
        var result = sut.TestValidate(model);

        if (string.IsNullOrEmpty(jobTitle) && memberUserType == Aan.SharedUi.Models.AmbassadorProfile.MemberUserType.Apprentice)
        {
            result.ShouldHaveValidationErrorFor(x => x.JobTitle);
        }
        else
        {
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
