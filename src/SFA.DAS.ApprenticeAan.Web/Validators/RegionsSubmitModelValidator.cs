using FluentValidation;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;

namespace SFA.DAS.ApprenticeAan.Web.Validators
{
    public class RegionsSubmitModelValidator : AbstractValidator<RegionsSubmitModel>
    {
        public const string NoSelectionErrorMessage = "Select a region where you work as an apprentice.";

        public RegionsSubmitModelValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;
            RuleFor(m => m.SelectedRegionId)
                .NotNull()
                .WithMessage(NoSelectionErrorMessage);
        }
    }
}