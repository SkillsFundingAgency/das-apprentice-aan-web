using FluentValidation;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;

namespace SFA.DAS.ApprenticeAan.Web.Validators.Onboarding;

public class PreviousEngagementSubmitModelValidator : AbstractValidator<PreviousEngagementSubmitModel>
{
    public const string NoSelectionErrorMessage = "Select whether you have met another apprenticeship ambassador before";

    public PreviousEngagementSubmitModelValidator()
    {
        RuleFor(m => m.HasPreviousEngagement)
            .NotNull()
            .WithMessage(NoSelectionErrorMessage);
    }
}