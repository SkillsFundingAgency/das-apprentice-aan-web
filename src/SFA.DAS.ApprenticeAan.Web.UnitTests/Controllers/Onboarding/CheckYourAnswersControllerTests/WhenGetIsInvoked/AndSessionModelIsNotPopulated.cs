using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.Aan.SharedUi.Constants;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Requests;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses;
using SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;
using SFA.DAS.ApprenticeAan.Web.UnitTests.TestHelpers;
using SFA.DAS.ApprenticePortal.Authentication.TestHelpers;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers.Onboarding.CheckYourAnswersControllerTests.WhenGetIsInvoked;

public class AndSessionModelIsNotPopulated
{
    ViewResult getResult;
    CheckYourAnswersViewModel viewModel;

    [SetUp]
    public void Init()
    {
        var fixture = new Fixture();
        var apprenticeId = Guid.NewGuid();
        OnboardingSessionModel sessionModel = new() { MyApprenticeship = fixture.Create<MyApprenticeship>(), ApprenticeDetails = fixture.Create<ApprenticeDetailsModel>() };
        Mock<ISessionService> sessionServiceMock = new();
        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);

        Mock<IOuterApiClient> outerApiClientMock = new();
        outerApiClientMock.Setup(a => a.PostApprenticeMember(It.IsAny<CreateApprenticeMemberRequest>())).ReturnsAsync(new CreateApprenticeMemberResponse(Guid.NewGuid()));

        CheckYourAnswersController sut = new(sessionServiceMock.Object, outerApiClientMock.Object);
        var user = AuthenticatedUsersForTesting.FakeLocalUserFullyVerifiedClaim(apprenticeId);
        sut.ControllerContext = new() { HttpContext = new DefaultHttpContext() { User = user.HttpContext!.User } };

        sut.AddUrlHelperMock();

        sessionModel.ProfileData.Add(new ProfileModel { Id = ProfileConstants.ProfileIds.JobTitle, Value = null });
        sessionModel.RegionName = null;
        sessionModel.ProfileData.Add(new ProfileModel { Id = ProfileConstants.ProfileIds.ReasonToJoinAmbassadorNetwork, Value = null });
        sessionModel.ProfileData.Add(new ProfileModel { Id = ProfileConstants.ProfileIds.EngagedWithAPreviousAmbassadorInTheNetworkApprentice, Value = null });
        sessionModel.ProfileData.Add(new ProfileModel { Id = ProfileConstants.ProfileIds.EmployerName, Value = null });
        sessionModel.ProfileData.Add(new ProfileModel { Id = ProfileConstants.ProfileIds.EmployerAddress1, Value = null });
        sessionModel.ProfileData.Add(new ProfileModel { Id = ProfileConstants.ProfileIds.EmployerAddress2, Value = null });
        sessionModel.ProfileData.Add(new ProfileModel { Id = ProfileConstants.ProfileIds.EmployerTownOrCity, Value = null });
        sessionModel.ProfileData.Add(new ProfileModel { Id = ProfileConstants.ProfileIds.EmployerCounty, Value = null });
        sessionModel.ProfileData.Add(new ProfileModel { Id = ProfileConstants.ProfileIds.EmployerPostcode, Value = null });

        var response = sut.Get();
        getResult = response.As<ViewResult>();
        viewModel = getResult.Model.As<CheckYourAnswersViewModel>();
    }

    [Test]
    public void ThenReturnsViewResults()
    {
        getResult.Should().NotBeNull();
        getResult.ViewName.Should().Be(CheckYourAnswersController.ViewPath);
    }

    [Test]
    public void ThenSetsJobTitleToNullInViewModel()
    {
        viewModel.JobTitle.Should().BeNull();
    }

    [Test]
    public void ThenSetsRegionToNullInViewModel()
    {
        viewModel.Region.Should().BeNull();
    }

    [Test]
    public void ThenSetsReasonToJoinTheNetworkToNullInViewModel()
    {
        viewModel.ReasonForJoiningTheNetwork.Should().BeNull();
    }

    [Test]
    public void ThenSetsPreviousEngagementToNullInViewModel()
    {
        viewModel.PreviousEngagement.Should().BeNull();
    }

    [Test]
    public void ThenSetsCurrentEmployerNameAndAddressToNullInViewModel()
    {
        viewModel.CurrentEmployerName.Should().BeNull();
        viewModel.CurrentEmployerAddress.Should().BeNull();
    }
}
