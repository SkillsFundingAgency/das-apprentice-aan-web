using FluentValidation;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;

namespace SFA.DAS.ApprenticeAan.Web.Validators.Onboarding;

public class RegionsSubmitModelValidator : AbstractValidator<RegionsSubmitModel>
{
    public const string NoSelectionErrorMessage = "Select where you work";

    public RegionsSubmitModelValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;
        RuleFor(m => m.SelectedRegionId)
            .NotNull()
            .WithMessage(NoSelectionErrorMessage);
    }
}