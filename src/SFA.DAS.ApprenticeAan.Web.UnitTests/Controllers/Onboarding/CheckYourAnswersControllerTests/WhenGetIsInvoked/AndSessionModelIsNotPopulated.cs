using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.ApprenticeAan.Domain.Constants;
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
    public async Task Init()
    {
        var fixture = new Fixture();
        var apprenticeId = Guid.NewGuid();
        OnboardingSessionModel sessionModel = new();
        Mock<ISessionService> sessionServiceMock = new();
        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);

        Mock<IOuterApiClient> outerApiClientMock = new();
        outerApiClientMock.Setup(o => o.GetMyApprenticeship(apprenticeId)).ReturnsAsync(fixture.Create<MyApprenticeship>());
        outerApiClientMock.Setup(a => a.PostApprenticeMember(It.IsAny<CreateApprenticeMemberRequest>())).ReturnsAsync(new CreateApprenticeMemberResponse(Guid.NewGuid()));

        CheckYourAnswersController sut = new(sessionServiceMock.Object, outerApiClientMock.Object);
        var user = AuthenticatedUsersForTesting.FakeLocalUserFullyVerifiedClaim(apprenticeId);
        sut.ControllerContext = new() { HttpContext = new DefaultHttpContext() { User = user } };

        sut.AddUrlHelperMock();

        sessionModel.ProfileData.Add(new ProfileModel { Id = ProfileDataId.JobTitle, Value = null });
        sessionModel.RegionName = null;
        sessionModel.ApprenticeDetails.ReasonForJoiningTheNetwork = null;
        sessionModel.ProfileData.Add(new ProfileModel { Id = ProfileDataId.HasPreviousEngagement, Value = null });
        sessionModel.ProfileData.Add(new ProfileModel { Id = ProfileDataId.EmployerName, Value = null });
        sessionModel.ProfileData.Add(new ProfileModel { Id = ProfileDataId.AddressLine1, Value = null });
        sessionModel.ProfileData.Add(new ProfileModel { Id = ProfileDataId.AddressLine2, Value = null });
        sessionModel.ProfileData.Add(new ProfileModel { Id = ProfileDataId.Town, Value = null });
        sessionModel.ProfileData.Add(new ProfileModel { Id = ProfileDataId.County, Value = null });
        sessionModel.ProfileData.Add(new ProfileModel { Id = ProfileDataId.Postcode, Value = null });

        var response = await sut.Get();
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
