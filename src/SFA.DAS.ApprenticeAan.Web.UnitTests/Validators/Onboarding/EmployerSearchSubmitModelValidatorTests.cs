using FluentAssertions;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;
using SFA.DAS.ApprenticeAan.Web.Validators.Onboarding;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Validators.Onboarding;

public class EmployerSearchSubmitModelValidatorTests
{
    [TestCase("", false)]
    [TestCase(null, false)]
    [TestCase(" ", false)]
    [TestCase("some text", true)]
    public void Validate_SearchTerm(string? searchTerm, bool expectedResult)
    {
        EmployerSearchSubmitModelValidator sut = new();

        EmployerSearchSubmitModel model = new() { SearchTerm = searchTerm };

        var result = sut.Validate(model);

        result.IsValid.Should().Be(expectedResult);
    }
}
