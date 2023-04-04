using FluentValidation;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;
using System.Text.RegularExpressions;
using static SFA.DAS.ApprenticeAan.Application.Constants.RegularExpressions;

namespace SFA.DAS.ApprenticeAan.Web.Validators.Onboarding;

public class EmployerDetailsSubmitModelValidator : AbstractValidator<EmployerDetailsSubmitModel>
{
    public const string EmployerNameEmptyMessage = "Enter an Employer Name";
    public const string EmployerNameMaxLengthMessage = "Your Employer Name must be up to 200 characters";
    public const string EmployerNameHasExcludedCharacter = "Your Employer Name must not include any special characters: @, #, $, ^, =, +, \\, /, <, >, %";

    public const string AddressLine1EmptyMessage = "Enter the Building and Street";
    public const string AddressLine1MaxLengthMessage = "Your Employer Address Line 1 must be up to 200 characters";
    public const string AddressLine1HasExcludedCharacter = "Your Employer Address Line 1 must not include any special characters: @, #, $, ^, =, +, \\, /, <, >, %";

    public const string AddressLine2MaxLengthMessage = "Your Employer Address Line 2 must be up to 200 characters";
    public const string AddressLine2HasExcludedCharacter = "Your Employer Address Line 2 must not include any special characters: @, #, $, ^, =, +, \\, /, <, >, %";

    public const string AddressLine3MaxLengthMessage = "Your Employer Address Line 3 must be up to 200 characters";
    public const string AddressLine3HasExcludedCharacter = "Your Employer Address Line 3 must not include any special characters: @, #, $, ^, =, +, \\, /, <, >, %";

    public const string TownOrCityEmptyMessage = "Enter a Town or city";
    public const string TownOrCityMaxLengthMessage = "Your Employer Town or city must be up to 200 characters";
    public const string TownOrCityHasExcludedCharacter = "Your Employer Town or city must not include any special characters: @, #, $, ^, =, +, \\, /, <, >, %";

    public const string PostcodeEmptyMessage = "Enter a Postcode";
    public const string PostcodeInvalidMessage = "Enter a real Postcode";

    public EmployerDetailsSubmitModelValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;
        RuleFor(e => e.EmployerName)
            .NotEmpty()
            .WithMessage(EmployerNameEmptyMessage)
            .MaximumLength(200)
            .WithMessage(EmployerNameMaxLengthMessage)
            .Matches(ExcludedCharactersRegex)
            .WithMessage(EmployerNameHasExcludedCharacter);

        RuleFor(e => e.AddressLine1)
            .NotEmpty()
            .WithMessage(AddressLine1EmptyMessage)
            .MaximumLength(200)
            .WithMessage(AddressLine1MaxLengthMessage)
            .Matches(ExcludedCharactersRegex)
            .WithMessage(AddressLine1HasExcludedCharacter);

        RuleFor(e => e.AddressLine2)
            .MaximumLength(200)
            .WithMessage(AddressLine2MaxLengthMessage)
            .Matches(ExcludedCharactersRegex)
            .WithMessage(AddressLine2HasExcludedCharacter);

        RuleFor(e => e.County)
            .MaximumLength(200)
            .WithMessage(AddressLine3MaxLengthMessage)
            .Matches(ExcludedCharactersRegex)
            .WithMessage(AddressLine3HasExcludedCharacter);

        RuleFor(e => e.Town)
            .NotEmpty()
            .WithMessage(TownOrCityEmptyMessage)
            .MaximumLength(200)
            .WithMessage(TownOrCityMaxLengthMessage)
            .Matches(ExcludedCharactersRegex)
            .WithMessage(TownOrCityHasExcludedCharacter);

        RuleFor(e => e.Postcode)
            .NotEmpty()
            .WithMessage(PostcodeEmptyMessage)
            .Matches(PostcodeRegex, RegexOptions.IgnoreCase)
            .WithMessage(PostcodeInvalidMessage);
    }
}