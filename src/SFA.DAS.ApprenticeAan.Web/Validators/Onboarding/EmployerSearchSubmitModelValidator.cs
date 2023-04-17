using FluentValidation;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;

namespace SFA.DAS.ApprenticeAan.Web.Validators.Onboarding;

public class EmployerSearchSubmitModelValidator : AbstractValidator<EmployerSearchSubmitModel>
{
    private const string MissingAddressErrorMessage = "Enter an employer name or postcode";

    public EmployerSearchSubmitModelValidator()
    {
        When(m => string.IsNullOrWhiteSpace(m.Postcode), () =>
        {
            RuleFor(e => e.SearchTerm)
                .NotEmpty()
                .WithMessage(MissingAddressErrorMessage);
        });
    }
}
