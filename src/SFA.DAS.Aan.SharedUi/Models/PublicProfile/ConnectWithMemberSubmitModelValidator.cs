using FluentValidation;
using SFA.DAS.Aan.SharedUi.Models.PublicProfile;

namespace SFA.DAS.ApprenticeAan.Web.Validators.MemberProfile;

public class ConnectWithMemberSubmitModelValidator : AbstractValidator<ConnectWithMemberSubmitModel>
{
    public const string ReasonToConnectValidationMessage = "You must tell us why you want to get in touch";
    public const string HasAgreedToSharePersonalDetailsValidationMessage = "You must agree to sharing your details";
    public const string HasAgreedToCodeOfConductValidationMessage = "You must agree to adhere the code of conduct";

    public ConnectWithMemberSubmitModelValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;
        RuleFor(x => x.ReasonToGetInTouch).GreaterThan(0).WithMessage(ReasonToConnectValidationMessage);
        RuleFor(x => x.HasAgreedToSharePersonalDetails).Must(beTrue => beTrue).WithMessage(HasAgreedToSharePersonalDetailsValidationMessage);
        RuleFor(x => x.HasAgreedToCodeOfConduct).Must(beTrue => beTrue).WithMessage(HasAgreedToCodeOfConductValidationMessage);
    }
}
