using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.ApprenticeAan.Domain.Constants;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;
using SFA.DAS.ApprenticeAan.Web.UnitTests.TestHelpers;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers.Onboarding.CheckYourAnswersControllerTests.WhenGetIsInvoked;

public class AndSessionModelIsNotPopulated
{
    ViewResult getResult;
    CheckYourAnswersViewModel viewModel;
    CheckYourAnswersController sut;

    [SetUp]
    public void Init()
    {
        var fixture = new Fixture();
        OnboardingSessionModel sessionModel = fixture.Create<OnboardingSessionModel>();
        Mock<ISessionService> sessionServiceMock = new();
        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);
        sut = new(sessionServiceMock.Object);

        sut.AddUrlHelperMock();

        sessionModel.ProfileData.Add(new ProfileModel { Id = ProfileDataId.JobTitle, Value = null });
        sessionModel.RegionName = null;
        sessionModel.ApprenticeDetails.ReasonForJoiningTheNetwork = null;
        sessionModel.ProfileData.Add(new ProfileModel { Id = ProfileDataId.HasPreviousEngagement, Value = null });

        getResult = sut.Get().As<ViewResult>();
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
}
