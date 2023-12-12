using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Web.Controllers;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers;
public class ContactUsControllerTests
{
    [Test]
    public void WhenGettingContactUs_ReturnsViewResult()
    {
        ContactUsController sut = new();

        var result = sut.Index() as ViewResult;
        result.Should().NotBeNull();
    }
}
