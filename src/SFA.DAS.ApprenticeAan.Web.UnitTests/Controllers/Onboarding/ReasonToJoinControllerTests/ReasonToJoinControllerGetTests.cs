﻿using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;
using SFA.DAS.ApprenticeAan.Web.UnitTests.TestHelpers;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers.Onboarding.ReasonToJoinControllerTests;

[TestFixture]
public class ReasonToJoinControllerGetTests
{
    [MoqAutoData]
    public void Get_ViewResult_HasCorrectViewPath([Greedy] ReasonToJoinController sut)
    {
        sut.AddUrlHelperMock();
        var result = sut.Get();

        result.As<ViewResult>().ViewName.Should().Be(ReasonToJoinController.ViewPath);
    }

    [MoqAutoData]
    public void Get_ViewModel_HasBackLink([Greedy] ReasonToJoinController sut)
    {
        sut.AddUrlHelperMock().AddUrlForRoute(RouteNames.Onboarding.Regions);
        var result = sut.Get();

        result.As<ViewResult>().Model.As<ReasonToJoinViewModel>().BackLink.Should().Be(TestConstants.DefaultUrl);
    }

    [MoqAutoData]
    public void Get_ViewModelHasEmployersApproval_RestoreFromSession(
                        [Frozen] Mock<ISessionService> sessionServiceMock,
                        OnboardingSessionModel sessionModel,
                        [Greedy] ReasonToJoinController sut)
    {
        sut.AddUrlHelperMock().AddUrlForRoute(RouteNames.Onboarding.CurrentJobTitle);
        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);

        var result = sut.Get();

        result.As<ViewResult>().Model.As<ReasonToJoinViewModel>().ReasonForJoiningTheNetwork.Should().Be(sessionModel.ApprenticeDetails.ReasonForJoiningTheNetwork);
    }
}