using FluentValidation;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;

namespace SFA.DAS.ApprenticeAan.Web.Validators.Onboarding;

public class LineManagerSubmitModelValidator : AbstractValidator<LineManagerSubmitModel>
{
    public const string NoSelectionErrorMessage = "Tell us if you have approval from your line manager to join the network.";
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