using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Web.Controllers;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers
{
    public class HomeControllerTests
    {
        [Test]
        public void Index_ReturnsPage()
        {
            var controller = new HomeController();
            var result = controller.Index() as ViewResult;

            result.Should().NotBeNull();
        }
    }
}