using FluentValidation;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;

namespace SFA.DAS.ApprenticeAan.Web.Validators.Onboarding;

public class EmployerSearchSubmitModelValidator : AbstractValidator<EmployerSearchSubmitModel>
{
    private const string MissingAddressErrorMessage = "Enter your employer's name or address";

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
