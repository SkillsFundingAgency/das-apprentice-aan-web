using FluentValidation;
using SFA.DAS.ApprenticeAan.Web.Models.NetworkEvents;

namespace SFA.DAS.ApprenticeAan.Web.Validators.NetworkEvents;

public class SubmitAttendanceCommandValidator : AbstractValidator<SubmitAttendanceCommand>
{
    public const string EventTimeDateHasPassed = "This event has now passed and cannot be changed";

    public SubmitAttendanceCommandValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;
        RuleFor(x => x.StartDateTime).Must(c => c > DateTime.UtcNow).WithMessage(EventTimeDateHasPassed);
    }
}
