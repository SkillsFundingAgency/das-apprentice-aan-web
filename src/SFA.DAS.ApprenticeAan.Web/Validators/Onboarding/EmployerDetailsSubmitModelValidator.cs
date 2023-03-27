using FluentValidation;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;
using System.Text.RegularExpressions;
using static SFA.DAS.ApprenticeAan.Application.Constants.RegularExpressions;

namespace SFA.DAS.ApprenticeAan.Web.Validators.Onboarding;

public class EmployerDetailsSubmitModelValidator : AbstractValidator<EmployerDetailsSubmitModel>
{
    public const string EmployerNameEmptyMessage = "Please enter an Employer Name";
    public const string EmployerNameMaxLengthMessage = "There is a problem.Your employer name must be up to 200 characters";
    
    public const string AddressLine1EmptyMessage = "Please enter the Building and Street";
    public const string AddressLine1MaxLengthMessage = "There is a problem.Your employer address line1 must be up to 200 characters";

    public const string AddressLine2MaxLengthMessage = "There is a problem.Your employer address line2 must be up to 200 characters";

    public const string TownOrCityEmptyMessage = "Please enter a Town or City";
    public const string TownOrCityMaxLengthMessage = "There is a problem.Your employer town or city must be up to 200 characters";

    public const string CountyMaxLengthMessage = "There is a problem.Your employer county must be up to 200 characters";

    public const string PostcodeEmptyMessage = "Please enter a postcode";
    public const string PostcodeInvalidMessage = "Please enter a real postcode";


    public EmployerDetailsSubmitModelValidator()
    {
        RuleFor(e => e.EmployerName)
            .NotEmpty()
            .WithMessage(EmployerNameEmptyMessage)
            .MaximumLength(200)
            .WithMessage(EmployerNameMaxLengthMessage);

        RuleFor(e => e.AddressLine1)
            .NotEmpty()
            .WithMessage(AddressLine1EmptyMessage)
            .MaximumLength(200)
            .WithMessage(AddressLine1MaxLengthMessage);

        RuleFor(e => e.AddressLine2)
            .MaximumLength(200)
            .WithMessage(AddressLine2MaxLengthMessage);

        RuleFor(e => e.Town)
            .NotEmpty()
            .WithMessage(TownOrCityEmptyMessage)
            .MaximumLength(200)
            .WithMessage(TownOrCityMaxLengthMessage);

        RuleFor(e => e.County)
            .MaximumLength(200)
            .WithMessage(CountyMaxLengthMessage);

        RuleFor(e => e.Postcode)
            .NotEmpty()
            .WithMessage(PostcodeEmptyMessage)
            .Matches(PostcodeRegex, RegexOptions.IgnoreCase)
            .WithMessage(PostcodeInvalidMessage);
    }
}