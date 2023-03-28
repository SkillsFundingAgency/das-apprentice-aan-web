using FluentValidation;
using Newtonsoft.Json.Linq;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;
using System.Text.RegularExpressions;
using static SFA.DAS.ApprenticeAan.Application.Constants.RegularExpressions;

namespace SFA.DAS.ApprenticeAan.Web.Validators.Onboarding;

public class EmployerDetailsSubmitModelValidator : AbstractValidator<EmployerDetailsSubmitModel>
{
    public const string EmployerNameEmptyMessage = "Please enter an Employer Name";
    public const string EmployerNameMaxLengthMessage = "Your Employer Name must be up to 200 characters";
    public const string EmployerNameDisallowedCharsMessage = "Your Employer Name must not include any special characters: @, #, $, ^, =, +, \\, /, <, >,% or 'Employer Name'";

    public const string AddressLine1EmptyMessage = "Please enter the Building and Street";
    public const string AddressLine1MaxLengthMessage = "Your Employer Address Line 1 must be up to 200 characters";
    public const string AddressLine1DisallowedCharsMessage = "Your Employer Address Line 1 must not include any special characters: @, #, $, ^, =, +, \\, /, <, >,% or 'Building and Street'";

    public const string AddressLine2MaxLengthMessage = "Your Employer Address Line 2 must be up to 200 characters";
    public const string AddressLine2DisallowedCharsMessage = "Your Employer Address Line 2 must not include any special characters: @, #, $, ^, =, +, \\, /, <, >,% or 'Building and Street'";

    public const string AddressLine3MaxLengthMessage = "Your Employer Address Line 3 must be up to 200 characters";
    public const string AddressLine3DisallowedCharsMessage = "Your Employer Address Line 3 must not include any special characters: @, #, $, ^, =, +, \\, /, <, >,% or 'Building and Street'";

    public const string TownOrCityEmptyMessage = "Please enter a Town or city";
    public const string TownOrCityMaxLengthMessage = "Your Employer Town or city must be up to 200 characters";
    public const string TownOrCityDisallowedCharsMessage = "Your Employer Town or city must not include any special characters: @, #, $, ^, =, +, \\, /, <, >,% or 'Town or city'";

    public const string PostcodeEmptyMessage = "Please enter a Postcode";
    public const string PostcodeInvalidMessage = "Please enter a real Postcode";

    private readonly string[] disallowedChars = { "@", "#", "$", "^", "=", "+", "\\", "/", "<", ">", "%", "Employer name", "Building and street", "Town or city", "Postcode" };

    public EmployerDetailsSubmitModelValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;
        RuleFor(e => e.EmployerName)
            .NotEmpty()
            .WithMessage(EmployerNameEmptyMessage)
            .MaximumLength(200)
            .WithMessage(EmployerNameMaxLengthMessage)
            .Must(m => disallowedChars.All(a => !m.Contains(a)))
            .WithMessage(EmployerNameDisallowedCharsMessage);

        RuleFor(e => e.AddressLine1)
            .NotEmpty()
            .WithMessage(AddressLine1EmptyMessage)
            .MaximumLength(200)
            .WithMessage(AddressLine1MaxLengthMessage)
            .Must(m => disallowedChars.All(a => !m.Contains(a)))
            .WithMessage(AddressLine1DisallowedCharsMessage);

        RuleFor(e => e.AddressLine2)
            .MaximumLength(200)
            .WithMessage(AddressLine2MaxLengthMessage)
            .Must(m => disallowedChars.All(a => m == null || !m.Contains(a)))
            .WithMessage(AddressLine2DisallowedCharsMessage);

        RuleFor(e => e.County)
            .MaximumLength(200)
            .WithMessage(AddressLine3MaxLengthMessage)
            .Must(m => disallowedChars.All(a => m == null || !m.Contains(a)))
            .WithMessage(AddressLine3DisallowedCharsMessage);

        RuleFor(e => e.Town)
            .NotEmpty()
            .WithMessage(TownOrCityEmptyMessage)
            .MaximumLength(200)
            .WithMessage(TownOrCityMaxLengthMessage)
            .Must(m => disallowedChars.All(a => !m.Contains(a)))
            .WithMessage(TownOrCityDisallowedCharsMessage);

        RuleFor(e => e.Postcode)
            .NotEmpty()
            .WithMessage(PostcodeEmptyMessage)
            .Matches(PostcodeRegex, RegexOptions.IgnoreCase)
            .WithMessage(PostcodeInvalidMessage);
    }
}