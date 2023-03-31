using FluentValidation;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;

namespace SFA.DAS.ApprenticeAan.Web.Validators.Onboarding;

public class ReasonToJoinTheNetworkSubmitModelValidator : AbstractValidator<ReasonToJoinTheNetworkSubmitModel>
{
    public const string NoSelectionErrorMessage = "Tell us if you are you joining because you have engaged with an ambassador in the network?";

    public ReasonToJoinTheNetworkSubmitModelValidator()
    {
        RuleFor(m => m.EngagedWithAPreviousAmbassadorInTheNetwork)
            .NotNull()
            .WithMessage(NoSelectionErrorMessage);
    }
}