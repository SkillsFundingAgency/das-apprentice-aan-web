using FluentValidation;
using SFA.DAS.Aan.SharedUi.Models.EditContactDetail;
using static SFA.DAS.ApprenticeAan.Application.Constants.RegularExpressions;

namespace SFA.DAS.ApprenticeAan.Web.Validators.EditContactDetail;

public class SubmitContactDetailModelValidator : AbstractValidator<SubmitContactDetailModel>
{
    public const string ShowLinkedinUrlValidationMessage = "You cannot select display check box without a value";
    public const string LinkedinUrlLengthValidationMessage = "Your custom URL must contain 3-100 letters or numbers";
    public const string LinkedinUrlPatternValidationMessage = "Your custom URL must not use spaces, symbols or special characters";
    public SubmitContactDetailModelValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;
        RuleFor(x => x.LinkedinUrl).NotNull().WithMessage(LinkedinUrlLengthValidationMessage).Length(3, 100).WithMessage(LinkedinUrlLengthValidationMessage).Matches(AlphaNumericCharactersRegex)
            .WithMessage(LinkedinUrlPatternValidationMessage);
        RuleFor(x => x.ShowLinkedinUrl)
            .Must((contactdetail, showlinkedinurl) => !showlinkedinurl)
            .When(x => string.IsNullOrEmpty(x.LinkedinUrl))
            .WithMessage(ShowLinkedinUrlValidationMessage);
    }
}
