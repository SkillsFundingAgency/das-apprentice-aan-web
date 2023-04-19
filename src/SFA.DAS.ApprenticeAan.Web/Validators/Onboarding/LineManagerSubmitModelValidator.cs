using FluentValidation;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;

namespace SFA.DAS.ApprenticeAan.Web.Validators.Onboarding;

public class LineManagerSubmitModelValidator : AbstractValidator<LineManagerSubmitModel>
{
    public const string NoSelectionErrorMessage = "Tell us if you have approval from your line manager to join the network.";

    public LineManagerSubmitModelValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;
        RuleFor(m => m.HasEmployersApproval)
            .NotNull()
            .WithMessage(NoSelectionErrorMessage);
    }
}