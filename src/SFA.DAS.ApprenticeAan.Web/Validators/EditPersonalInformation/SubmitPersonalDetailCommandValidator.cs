using FluentValidation;
using SFA.DAS.Aan.SharedUi.Models;

namespace SFA.DAS.ApprenticeAan.Web.Validators.MemberProfile;

public class SubmitPersonalDetailCommandValidator : AbstractValidator<SubmitPersonalDetailCommand>
{
    public const string BiographyValidationMessage = "Your biography must be 500 characters or less";
    public const string JobTitleValidationMessage = "Your job title must be 200 characters or less";
    public SubmitPersonalDetailCommandValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;
        RuleFor(x => x.Biography).Length(0, 500).WithMessage(BiographyValidationMessage);
        RuleFor(x => x.JobTitle).Length(0, 200).WithMessage(JobTitleValidationMessage);
        RuleFor(x => x.JobTitle)
        .Custom((model, context) =>
            {
                var userType = ((SubmitPersonalDetailCommand)context.InstanceToValidate).UserType;
                if (model == null && userType == Aan.SharedUi.Models.AmbassadorProfile.MemberUserType.Apprentice)
                {
                    context.AddFailure("Job tittle is compulsory for apprentice user");
                }
            });
    }
}
