using FluentValidation;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;

namespace SFA.DAS.ApprenticeAan.Web.Validators.Onboarding;

public class AreasOfInterestModelValidator : AbstractValidator<AreasOfInterestSubmitModel>
{
    public const string NoSelectionErrorMessage = "Select an area of interest";

    public AreasOfInterestModelValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;
        RuleFor(x => x.AreasOfInterest).Must(c => c.Any(p => p.IsSelected)).WithMessage(NoSelectionErrorMessage);
    }
}