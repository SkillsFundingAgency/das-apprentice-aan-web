using FluentValidation;
using SFA.DAS.ApprenticeAan.Web.Models.Shared;

namespace SFA.DAS.ApprenticeAan.Web.Validators.Shared;

public class NotificationLocationDisambiguationSubmitModelValidator : AbstractValidator<INotificationLocationDisambiguationPartialSubmitModel>
{
    public const string NoSelectionErrorMessage = "Select a location";

    public NotificationLocationDisambiguationSubmitModelValidator()
    {
        RuleFor(x => x.SelectedLocation)
            .NotNull().WithMessage(NoSelectionErrorMessage);
    }
}
