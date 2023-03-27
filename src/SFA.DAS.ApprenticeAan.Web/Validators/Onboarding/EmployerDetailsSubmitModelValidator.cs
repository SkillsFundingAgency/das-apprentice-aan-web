using FluentValidation;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;
using System.Text.RegularExpressions;
using static SFA.DAS.ApprenticeAan.Application.Constants.RegularExpressions;

namespace SFA.DAS.ApprenticeAan.Web.Validators.Onboarding;

public class EmployerDetailsSubmitModelValidator : AbstractValidator<EmployerDetailsSubmitModel>
{
    public const string EmployerNameEmptyMessage = "Please enter an Employer Name.";
    public const string AddressLine1EmptyMessage = "Please enter the Building and Street.";
    public const string TownOrCityEmptyMessage = "Please enter a Town or City.";
    public const string PostcodeEmptyMessage = "Please enter a postcode";
    public const string PostcodeInvalidMessage = "Please enter a real postcode";

    public EmployerDetailsSubmitModelValidator()
    {
        RuleFor(e => e.EmployerName)
            .NotEmpty()
            .WithMessage(EmployerNameEmptyMessage)
            .MaximumLength(200);

        RuleFor(e => e.AddressLine1)
            .NotEmpty()
            .WithMessage(AddressLine1EmptyMessage)
            .MaximumLength(200);

        RuleFor(e => e.AddressLine2)
            .MaximumLength(200);

        RuleFor(e => e.Town)
            .NotEmpty()
            .WithMessage(TownOrCityEmptyMessage)
            .MaximumLength(50);

        RuleFor(e => e.County)
            .MaximumLength(200);

        RuleFor(e => e.Postcode)
            .NotEmpty()
            .WithMessage(PostcodeEmptyMessage)
            .Matches(PostcodeRegex, RegexOptions.IgnoreCase)
            .WithMessage(PostcodeInvalidMessage)
            .MaximumLength(8);
    }
}