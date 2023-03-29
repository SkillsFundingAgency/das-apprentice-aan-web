using FluentValidation;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;

namespace SFA.DAS.ApprenticeAan.Web.Validators.Onboarding;

public class JoinTheNetworkSubmitModelValidator : AbstractValidator<JoinTheNetworkSubmitModel>
{
    public const string ReasonForJoiningTheNetworkEmptyMessage = "Enter why you want to join the network.";
    public const string ReasonForJoiningTheNetworkMaxWordsMessage = "You have entered more than 250 words.";

    public JoinTheNetworkSubmitModelValidator()
    {
        const int MaxWords = 250;
        RuleLevelCascadeMode = CascadeMode.Stop;
        RuleFor(m => m.ReasonForJoiningTheNetwork)
            .NotNull()
            .WithMessage(ReasonForJoiningTheNetworkEmptyMessage)
            .NotEmpty()
            .WithMessage(ReasonForJoiningTheNetworkEmptyMessage)
            .Must((m, x) => m.ReasonForJoiningTheNetwork!.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries).Length <= MaxWords)
            .WithMessage(ReasonForJoiningTheNetworkMaxWordsMessage);
    }
}