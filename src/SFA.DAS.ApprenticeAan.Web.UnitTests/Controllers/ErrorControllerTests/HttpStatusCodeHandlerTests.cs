using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SFA.DAS.Aan.SharedUi.Models;
using SFA.DAS.ApprenticeAan.Web.Controllers;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.UnitTests.TestHelpers;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers.ErrorControllerTests;
[TestFixture]
public class ErrorControllerTests
{
    const string PageNotFoundViewName = "PageNotFound";
    const string ErrorInServiceViewName = "ErrorInService";
    private static string NetworkHubUrl = Guid.NewGuid().ToString();

    [TestCase(403, PageNotFoundViewName)]
    [TestCase(404, PageNotFoundViewName)]
    [TestCase(500, ErrorInServiceViewName)]
    public void HttpStatusCodeHandler_ReturnsRespectiveView(int statusCode, string expectedViewName)
    {
        var sut = new ErrorController(Mock.Of<ILogger<ErrorController>>());
        sut.AddUrlHelperMock().AddUrlForRoute(RouteNames.NetworkHub, NetworkHubUrl);

        ViewResult result = (ViewResult)sut.HttpStatusCodeHandler(statusCode);

        result.ViewName.Should().Be(expectedViewName);
    }

    [Test]
    public void HttpStatusCodeHandler_InternalServerError_ReturnsRespectiveView()
    {
        // Arrange
        var sut = new ErrorController(Mock.Of<ILogger<ErrorController>>());
        sut.AddUrlHelperMock().AddUrlForRoute(RouteNames.NetworkHub, NetworkHubUrl);

        // Act
        ViewResult result = (ViewResult)sut.HttpStatusCodeHandler(500);
        var viewModel = result!.Model as ErrorViewModel;

        // Assert
        Assert.That(viewModel, Is.InstanceOf<ErrorViewModel>());
    }

    [Test]
    public void HttpStatusCodeHandler_InternalServerError_ShouldReturnExpectedValue()
    {
        // Arrange
        var sut = new ErrorController(Mock.Of<ILogger<ErrorController>>());
        sut.AddUrlHelperMock().AddUrlForRoute(RouteNames.NetworkHub, NetworkHubUrl);

        // Act
        ViewResult result = (ViewResult)sut.HttpStatusCodeHandler(500);
        var viewModel = result!.Model as ErrorViewModel;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(viewModel!.HomePageUrl, Is.EqualTo(NetworkHubUrl));
        });
    }
}
