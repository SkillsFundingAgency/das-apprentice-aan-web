using FluentValidation;
using SFA.DAS.Aan.SharedUi.Models.EditContactDetail;

namespace SFA.DAS.ApprenticeAan.Web.Validators.EditContactDetail;

public class SubmitContactDetailModelValidator : AbstractValidator<SubmitContactDetailModel>
{
    public const string ShowLinkedinUrlValidationMessage = "You cannot select display check box without a value";
    public SubmitContactDetailModelValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;
        RuleFor(x => x.ShowLinkedinUrl)
            .Must((contactdetail, showlinkedinurl) => !showlinkedinurl)
            .When(x => string.IsNullOrEmpty(x.LinkedinUrl))
            .WithMessage(ShowLinkedinUrlValidationMessage);
    }
}
