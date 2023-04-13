using FluentAssertions;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Models.Onboarding;

public class EmployerDetailsViewModelTests
{
    [TestCase("CV1 1EF", EmployerDetailsViewModel.CheckDetailsHeader)]
    [TestCase(" ", EmployerDetailsViewModel.AddDetailsHeader)]
    [TestCase("", EmployerDetailsViewModel.AddDetailsHeader)]
    [TestCase(null, EmployerDetailsViewModel.AddDetailsHeader)]
    public void HasAddress_IsSetBasedOnPostcode(string? postcode, string expectedValue)
    {
        EmployerDetailsViewModel sut = new() { Postcode = postcode };

        sut.PageHeader.Should().Be(expectedValue);
    }
}
