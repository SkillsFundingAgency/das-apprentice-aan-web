using FluentValidation;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;

namespace SFA.DAS.ApprenticeAan.Web.Validators.Onboarding;

public class ReasonToJoinSubmitModelValidator : AbstractValidator<ReasonToJoinSubmitModel>
{
    public const string ReasonForJoiningTheNetworkEmptyMessage = "Enter why you want to join the network";
    public const string ReasonForJoiningTheNetworkMaxWordsMessage = "Your answer must be 250 words or less";

    public ReasonToJoinSubmitModelValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;
        RuleFor(m => m.ReasonForJoiningTheNetwork)
            .NotEmpty()
            .WithMessage(ReasonForJoiningTheNetworkEmptyMessage)
            .Must((m, x) => m.ReasonForJoiningTheNetwork!.Trim().Split(new[] { ' ', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Length <= m.maxWordCount)
            .WithMessage(ReasonForJoiningTheNetworkMaxWordsMessage);
    }
}