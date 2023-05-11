using System.Reflection;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Web.Controllers;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers;
public class ControllerAuthorizeAttributeTests
{
    private readonly List<string> _controllersThatDoNotRequireAuthorize = new()
    {
        "ErrorController"
    };

    [Test]
    public void ControllersShouldHaveAuthorizeAttribute()
    {
        var webAssembly = typeof(HomeController).GetTypeInfo().Assembly;

        var controllers = webAssembly.DefinedTypes.Where(c => c.IsSubclassOf(typeof(Controller))).ToList();

        foreach (var controller in controllers.Where(c => !_controllersThatDoNotRequireAuthorize.Contains(c.Name)))
        {
            controller.Should().BeDecoratedWith<AuthorizeAttribute>();
        }
    }
}
