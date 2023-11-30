using FluentValidation;
using SFA.DAS.Aan.SharedUi.Models.EditAreaOfInterest;

namespace SFA.DAS.ApprenticeAan.Web.Validators.EditAreaOfInterest;

public class SubmitAreaOfInterestModelValidator : AbstractValidator<SubmitAreaOfInterestModel>
{
    public const string NoSelectionErrorMessage = "You must choose at least one option to assign to your profile";

    public SubmitAreaOfInterestModelValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;
        RuleFor(x => x.AreasOfInterest).Must(c => c.Any(p => p.IsSelected)).WithMessage(NoSelectionErrorMessage);
    }
}