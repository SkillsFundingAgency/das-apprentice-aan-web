using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Web.Controllers;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers;
public class AccessDeniedControllerTests
{
    [Test]
    public void WhenGettingRemovedShutter_ReturnsViewResult()
    {
        AccessDeniedController sut = new();

        var result = sut.RemovedShutter();
        var viewResult = result as ViewResult;

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.InstanceOf<ViewResult>());
            Assert.That(viewResult!.ViewName, Does.Contain(AccessDeniedController.RemovedShutterPath));
        });
    }
}
