using FluentValidation;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;

namespace SFA.DAS.ApprenticeAan.Web.Validators.Onboarding;

public class JoinTheNetworkSubmitModelValidator : AbstractValidator<JoinTheNetworkSubmitModel>
{
    public const string ReasonForJoiningTheNetworkEmptyMessage = "Enter why you want to join the network";
    public const string ReasonForJoiningTheNetworkMaxWordsMessage = "Why do you want to join the network must be 250 words or less";

    public JoinTheNetworkSubmitModelValidator()
    {
        const int MaxWords = 250;
        RuleLevelCascadeMode = CascadeMode.Stop;
        RuleFor(m => m.ReasonForJoiningTheNetwork)
            .NotEmpty()
            .WithMessage(ReasonForJoiningTheNetworkEmptyMessage)
            .Must((m, x) => m.ReasonForJoiningTheNetwork!.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries).Length <= MaxWords)
            .WithMessage(ReasonForJoiningTheNetworkMaxWordsMessage);
    }
}