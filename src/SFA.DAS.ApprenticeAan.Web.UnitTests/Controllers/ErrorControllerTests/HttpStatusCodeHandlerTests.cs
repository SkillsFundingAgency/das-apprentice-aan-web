using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SFA.DAS.ApprenticeAan.Web.Controllers;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers.ErrorControllerTests;
[TestFixture]
public class ErrorControllerTests
{
    const string PageNotFoundViewName = "PageNotFound";
    const string ErrorInServiceViewName = "ErrorInService";

    [TestCase(403, PageNotFoundViewName)]
    [TestCase(404, PageNotFoundViewName)]
    [TestCase(500, ErrorInServiceViewName)]
    public void HttpStatusCodeHandler_ReturnsRespectiveView(int statusCode, string expectedViewName)
    {
        var sut = new ErrorController(Mock.Of<ILogger<ErrorController>>());

        ViewResult result = (ViewResult)sut.HttpStatusCodeHandler(statusCode);

        result.ViewName.Should().Be(expectedViewName);
    }
}
