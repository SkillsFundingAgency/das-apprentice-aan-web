using FluentValidation;
using SFA.DAS.ApprenticeAan.Web.Models.Shared;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;

namespace SFA.DAS.ApprenticeAan.Web.Validators.Onboarding;

public class ReceiveNotificationsSubmitModelValidator : AbstractValidator<Models.Shared.ReceiveNotificationsSubmitModel>
{
    public const string NoSelectionErrorMessage = "Select if you want to receive a monthly email about upcoming events";

    public ReceiveNotificationsSubmitModelValidator()
    {
        RuleFor(m => m.ReceiveNotifications)
            .NotNull()
            .WithMessage(NoSelectionErrorMessage);
    }
}