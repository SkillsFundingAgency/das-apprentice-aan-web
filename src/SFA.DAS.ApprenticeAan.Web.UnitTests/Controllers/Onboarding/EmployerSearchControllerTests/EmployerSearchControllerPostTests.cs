using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;
using SFA.DAS.ApprenticeAan.Web.UnitTests.TestHelpers;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers.Onboarding.EmployerSearchControllerTests;

public class EmployerSearchControllerPostTests
{
    [Test]
    [MoqAutoData]
    public void Post_InvalidModel_ReturnsView(
        [Greedy] EmployerSearchController sut)
    {
        sut.AddUrlHelperMock();
        var result = sut.Post(new EmployerSearchSubmitModel());

        result.As<ViewResult>().ViewData.ModelState.IsValid.Should().BeFalse();
    }
}
