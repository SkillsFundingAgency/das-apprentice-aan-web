using FluentValidation;
using SFA.DAS.ApprenticeAan.Web.Models.Shared;

namespace SFA.DAS.ApprenticeAan.Web.Validators.Onboarding;

public class ReceiveNotificationsSubmitModelValidator : AbstractValidator<ReceiveNotificationsSubmitModel>
{
    public const string NoSelectionErrorMessage = "Select if you want to receive a monthly email about upcoming events";

    public ReceiveNotificationsSubmitModelValidator()
    {
        RuleFor(m => m.ReceiveNotifications)
            .NotNull()
            .WithMessage(NoSelectionErrorMessage);
    }
}