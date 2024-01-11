using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Aan.SharedUi.Infrastructure;
using SFA.DAS.Aan.SharedUi.Models.LeaveTheNetwork;
using SFA.DAS.ApprenticeAan.Web.Controllers;
using SFA.DAS.ApprenticeAan.Web.UnitTests.TestHelpers;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers.LeavingTheNetworkTests;
public class LeavingTheNetworkConfirmationControllerTests
{
    static readonly string HomeUrl = Guid.NewGuid().ToString();

    [Test, MoqAutoData]
    public void LeavingNetworkComplete_ConfirmationPage([Greedy] LeavingTheNetworkConfirmationController sut)
    {
        sut.AddUrlHelperMock()
            .AddUrlForRoute(SharedRouteNames.Home, HomeUrl);

        var result = sut.LeavingNetworkComplete();

        result.Should().BeOfType<ViewResult>();
        var viewResult = result as ViewResult;

        var model = viewResult!.Model as LeaveTheNetworkConfirmedViewModel;

        Assert.Multiple(() =>
        {
            Assert.That(model!.HomeUrl, Is.EqualTo(HomeUrl));

            Assert.That(viewResult!.ViewName, Is.EqualTo(LeavingTheNetworkConfirmationController.LeaveTheNetworkConfirmedViewPath));
        });
    }
}
