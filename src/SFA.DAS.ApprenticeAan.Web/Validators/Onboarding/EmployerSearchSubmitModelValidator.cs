using FluentValidation;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;

namespace SFA.DAS.ApprenticeAan.Web.Validators.Onboarding;

public class EmployerSearchSubmitModelValidator : AbstractValidator<EmployerSearchSubmitModel>
{
    private const string MissingSearchTermErrorMessage = "Enter an employer name or postcode";

    public EmployerSearchSubmitModelValidator()
    {
        RuleFor(m => m.SearchTerm)
            .NotEmpty()
            .WithMessage(MissingSearchTermErrorMessage);
    }
}
