using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.ApprenticeAan.Domain.Constants;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;
using SFA.DAS.ApprenticeAan.Web.UnitTests.TestHelpers;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers.Onboarding.EmployerDetailsControllerTests;

[TestFixture]
public class EmployerDetailsControllerGetTests
{
    private const string Category = "Employer";
    private const string EmployerName = "Department for Education";
    private const string AddressLine1 = "20 Great Smith St";
    private const string Town = "London";
    private const string Postcode = "SW1P 3BT";

    private readonly List<ProfileModel> profileData = new()
    {
        new() { Id = ProfileDataId.EmployerName, Category = Category, Value = EmployerName},
        new() { Id = ProfileDataId.AddressLine1, Category = Category, Value = AddressLine1},
        new() { Id = ProfileDataId.AddressLine2, Category = Category},
        new() { Id = ProfileDataId.Town, Category = Category, Value = Town},
        new() { Id = ProfileDataId.County, Category = Category, },
        new() { Id = ProfileDataId.Postcode, Category = Category, Value = Postcode}
    };

    [MoqAutoData]
    public void Get_ReturnsViewResult(
        [Frozen] Mock<ISessionService> sessionServiceMock,
        OnboardingSessionModel sessionModel,
        [Greedy] EmployerDetailsController sut)
    {
        sut.AddUrlHelperMock().AddUrlForRoute(RouteNames.Onboarding.LineManager);
        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);
        sessionModel.ProfileData = profileData;

        var result = sut.GetEmployerDetails();

        result.As<ViewResult>().Should().NotBeNull();
    }


    [MoqAutoData]
    public void Get_ViewResult_HasCorrectViewPath(
        [Frozen] Mock<ISessionService> sessionServiceMock,
        OnboardingSessionModel sessionModel,
        [Greedy] EmployerDetailsController sut)
    {
        sut.AddUrlHelperMock().AddUrlForRoute(RouteNames.Onboarding.LineManager);
        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);
        sessionModel.ProfileData = profileData;

        var result = sut.GetEmployerDetails();

        result.As<ViewResult>().ViewName.Should().Be(EmployerDetailsController.ViewPath);
    }

    [MoqAutoData]
    public void Get_ViewModel_HasBackLink(
        [Frozen] Mock<ISessionService> sessionServiceMock,
        OnboardingSessionModel sessionModel,
        [Greedy] EmployerDetailsController sut)
    {
        sut.AddUrlHelperMock().AddUrlForRoute(RouteNames.Onboarding.LineManager);
        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);
        sessionModel.ProfileData = profileData;

        var result = sut.GetEmployerDetails();

        result.As<ViewResult>().Model.As<EmployerDetailsViewModel>().BackLink.Should().Be(TestConstants.DefaultUrl);
    }

    [MoqAutoData]
    public void Get_ViewModelProfileData_RestoreFromSession(
        [Frozen] Mock<ISessionService> sessionServiceMock,
        OnboardingSessionModel sessionModel,
        [Greedy] EmployerDetailsController sut)
    {
        sut.AddUrlHelperMock().AddUrlForRoute(RouteNames.Onboarding.LineManager);

        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);
        sessionModel.ProfileData = profileData;

        sessionServiceMock.Object.Set(sessionModel);

        var result = (EmployerDetailsViewModel)((ViewResult)sut.GetEmployerDetails()).Model!;

        result.EmployerName.Should().Be(EmployerName);
        result.AddressLine1.Should().Be(AddressLine1);
        result.AddressLine2.Should().BeNull();
        result.Town.Should().Be(Town);
        result.County.Should().BeNull();
        result.Postcode.Should().Be(Postcode);
    }
}