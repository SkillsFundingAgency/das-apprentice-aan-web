using FluentValidation.TestHelper;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;
using SFA.DAS.ApprenticeAan.Web.Validators.Onboarding;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Validators.Onboarding;

public class EmployerSearchSubmitModelValidatorTests
{
    [TestCase("", false)]
    [TestCase(null, false)]
    [TestCase(" ", false)]
    [TestCase("some postcode", true)]
    public void Validate_SearchTerm(string? postcode, bool isValid)
    {
        EmployerSearchSubmitModelValidator sut = new();

        EmployerSearchSubmitModel model = new() { Postcode = postcode };

        var result = sut.TestValidate(model);

        if (isValid)
        {
            result.ShouldNotHaveValidationErrorFor(m => m.SearchTerm);
        }
        else
        {
            result.ShouldHaveValidationErrorFor(m => m.SearchTerm);
        }
    }
}
