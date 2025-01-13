using FluentValidation;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;

namespace SFA.DAS.ApprenticeAan.Web.Validators.Onboarding;

public class SelectNotificationsSubmitModelValidator : AbstractValidator<SelectNotificationsSubmitModel>
{
    public const string NoSelectionErrorMessage = "Select which types of events you want to be notified about";

    public SelectNotificationsSubmitModelValidator()
    {
        RuleFor(m => m.EventTypes)
            .NotNull()
            .WithMessage(NoSelectionErrorMessage)
            .Must(eventTypes => eventTypes != null && eventTypes.Any(e => e.IsSelected))
            .WithMessage(NoSelectionErrorMessage);
    }
}
