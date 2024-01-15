using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Aan.SharedUi.Models;
using SFA.DAS.ApprenticeAan.Web.Configuration;
using SFA.DAS.ApprenticeAan.Web.Controllers;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers;
public class ContactUsControllerTests
{
    [Test, MoqAutoData]
    public void WhenGettingContactUs_ReturnsViewResult(
        [Frozen] ApplicationConfiguration applicationConfiguration
        )
    {
        var contactUsEmails = applicationConfiguration.ContactUsEmails;

        ContactUsController sut = new(applicationConfiguration);

        var actualResult = sut.Index() as ViewResult;
        var result = actualResult!.Model as ContactUsViewModel;

        result.Should().NotBeNull();
        result!.EastOfEnglandEmailAddress.Should().Be(contactUsEmails.EastOfEngland);
        result!.EastMidlandsEmailAddress.Should().Be(contactUsEmails.EastMidlands);
        result!.LondonEmailAddress.Should().Be(contactUsEmails.London);
        result!.NorthEastEmailAddress.Should().Be(contactUsEmails.NorthEast);
        result!.NorthWestEmailAddress.Should().Be(contactUsEmails.NorthWest);
        result!.SouthEastEmailAddress.Should().Be(contactUsEmails.SouthEast);
        result!.SouthWestEmailAddress.Should().Be(contactUsEmails.SouthWest);
        result!.WestMidlandsEmailAddress.Should().Be(contactUsEmails.WestMidlands);
        result!.YorkshireAndTheHumberEmailAddress.Should().Be(contactUsEmails.YorkshireAndTheHumber);
        result!.NetworkHubLink.Should().BeNull();
    }
}
