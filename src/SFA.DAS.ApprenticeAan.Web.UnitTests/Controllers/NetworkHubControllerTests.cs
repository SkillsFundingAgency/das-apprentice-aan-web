using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Web.Controllers;
using SFA.DAS.ApprenticeAan.Web.Models;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers;

public class NetworkHubControllerTests
{
    [Test]
    public void Index_ReturnsView()
    {
        NetworkHubController sut = new();

        var result = sut.Index();

        result.As<ViewResult>().Model.As<NetworkHubViewModel>().Should().NotBeNull();
    }
}
