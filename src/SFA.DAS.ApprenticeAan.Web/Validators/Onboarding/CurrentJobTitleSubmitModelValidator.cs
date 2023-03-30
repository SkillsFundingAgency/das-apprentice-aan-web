using FluentValidation;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;

namespace SFA.DAS.ApprenticeAan.Web.Validators.Onboarding;

public class CurrentJobTitleSubmitModelValidator : AbstractValidator<CurrentJobTitleSubmitModel>
{
    public const string JobTitleLengthInvalidErrorMessage = "Your current job title must be 200 character or less";
    public const string NotValidJobTitleErrorMessage = "Your job title must not include special characters: @, #, $, ^, =, +, \\, /, <, >,%";
    public const string JobTitleEmpty = "Enter a job title";

    private const string regExAlphanumeric = "^[a-zA-Z0-9\\s.\\-\\(\\)]+$";
    public CurrentJobTitleSubmitModelValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;
        RuleFor(m => m.JobTitle)
            .NotEmpty()
            .WithMessage(JobTitleEmpty)
            .MaximumLength(200)
            .MinimumLength(1)
            .WithMessage(JobTitleLengthInvalidErrorMessage)
            .Matches(regExAlphanumeric)
            .WithMessage(NotValidJobTitleErrorMessage);
    }
}