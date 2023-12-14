using System.Text.RegularExpressions;
using FluentValidation;
using SFA.DAS.Aan.SharedUi.Models.EditApprenticeshipInformation;
using static SFA.DAS.ApprenticeAan.Application.Constants.RegularExpressions;

namespace SFA.DAS.ApprenticeAan.Web.Validators.EditApprenticeshipInformation;

public class SubmitApprenticeshipInformationModelValidator : AbstractValidator<SubmitApprenticeshipInformationModel>
{
    public const string EmployerNameEmptyMessage = "Enter an Employer Name";
    public const string EmployerNameMaxLengthMessage = "Your Employer Name must be up to 200 characters";
    public const string EmployerNameHasExcludedCharacter = "Your Employer Name must not include any special characters: @, #, $, ^, =, +, \\, /, <, >, %";

    public const string AddressLine1EmptyMessage = "Enter the Employer Address Line 1";
    public const string AddressLine1MaxLengthMessage = "Your Employer Address Line 1 must be up to 200 characters";
    public const string AddressLine1HasExcludedCharacter = "Your Employer Address Line 1 must not include any special characters: @, #, $, ^, =, +, \\, /, <, >, %";

    public const string AddressLine2MaxLengthMessage = "Your Employer Address Line 2 must be up to 200 characters";
    public const string AddressLine2HasExcludedCharacter = "Your Employer Address Line 2 must not include any special characters: @, #, $, ^, =, +, \\, /, <, >, %";

    public const string CountyMaxLengthMessage = "Your Employer county must be up to 200 characters";
    public const string CountyHasExcludedCharacter = "Your Employer county must not include any special characters: @, #, $, ^, =, +, \\, /, <, >, %";

    public const string TownOrCityEmptyMessage = "Enter a Town or city";
    public const string TownOrCityMaxLengthMessage = "Your Employer Town or city must be up to 200 characters";
    public const string TownOrCityHasExcludedCharacter = "Your Employer Town or city must not include any special characters: @, #, $, ^, =, +, \\, /, <, >, %";

    public const string PostcodeEmptyMessage = "Enter a Postcode";
    public const string PostcodeInvalidMessage = "Enter a real Postcode";

    public SubmitApprenticeshipInformationModelValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;
        RuleFor(e => e.EmployerName)
            .NotEmpty()
            .WithMessage(EmployerNameEmptyMessage)
            .MaximumLength(200)
            .WithMessage(EmployerNameMaxLengthMessage)
            .Matches(ExcludedCharactersRegex)
            .WithMessage(EmployerNameHasExcludedCharacter);

        RuleFor(e => e.EmployerAddress1)
            .NotEmpty()
            .WithMessage(AddressLine1EmptyMessage)
            .MaximumLength(200)
            .WithMessage(AddressLine1MaxLengthMessage)
            .Matches(ExcludedCharactersRegex)
            .WithMessage(AddressLine1HasExcludedCharacter);

        RuleFor(e => e.EmployerAddress2)
            .MaximumLength(200)
            .WithMessage(AddressLine2MaxLengthMessage)
            .Matches(ExcludedCharactersRegex)
            .WithMessage(AddressLine2HasExcludedCharacter);

        RuleFor(e => e.EmployerCounty)
            .MaximumLength(200)
            .WithMessage(CountyMaxLengthMessage)
            .Matches(ExcludedCharactersRegex)
            .WithMessage(CountyHasExcludedCharacter);

        RuleFor(e => e.EmployerTownOrCity)
            .NotEmpty()
            .WithMessage(TownOrCityEmptyMessage)
            .MaximumLength(200)
            .WithMessage(TownOrCityMaxLengthMessage)
            .Matches(ExcludedCharactersRegex)
            .WithMessage(TownOrCityHasExcludedCharacter);

        RuleFor(e => e.EmployerPostcode)
            .NotEmpty()
            .WithMessage(PostcodeEmptyMessage)
            .Matches(PostcodeRegex, RegexOptions.IgnoreCase)
            .WithMessage(PostcodeInvalidMessage);
    }
}