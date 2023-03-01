using FluentValidation;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;

namespace SFA.DAS.ApprenticeAan.Web.Validators.Onboarding;

public class LineManagerSubmitModelValidator : AbstractValidator<LineManagerSubmitModel>
{
    public const string NoSelectionErrorMessage = "There is a problem. Please make a selection.";
    public const string NoApprovalErrorMessage = "You must have your manager's approval before you complete and submit your application.";

    public LineManagerSubmitModelValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;
        RuleFor(m => m.HasEmployersApproval)
            .NotNull()
            .WithMessage(NoSelectionErrorMessage)
            .Equal(true)
            .WithMessage(NoApprovalErrorMessage);
    }
}