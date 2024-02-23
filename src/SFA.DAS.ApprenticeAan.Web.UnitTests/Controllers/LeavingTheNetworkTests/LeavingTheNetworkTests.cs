using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.Aan.SharedUi.Infrastructure;
using SFA.DAS.Aan.SharedUi.Models.LeaveTheNetwork;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Controllers;
using SFA.DAS.ApprenticeAan.Web.Extensions;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.ApprenticeAan.Web.UnitTests.TestHelpers;
using SFA.DAS.ApprenticePortal.Authentication.TestHelpers;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers.LeavingTheNetworkTests;
public class LeavingTheNetworkTests
{
    static readonly string ProfileSettingsUrl = Guid.NewGuid().ToString();
    static readonly string LeaveTheNetworkConfirmationUrl = Guid.NewGuid().ToString();

    private const int Reason20Selected = 20;
    private const int ExperienceReason400Selected = 400;

    [Test, MoqAutoData]
    public async Task Index_ReturnLeaveTheNetworkViewModel(
        [Frozen] Mock<IOuterApiClient> outerApiMock,
        [Greedy] LeaveTheNetworkController sut)
    {
        var leavingCategoryReasons = GetLeavingCategoryReasons();
        var leavingCategoryBenefits = GetLeavingCategoryBenefits();
        var leavingCategoryExperience = GetLeavingCategoryExperience();
        var leavingCategories = new List<LeavingCategory>
        {
            leavingCategoryReasons,
            leavingCategoryBenefits,
            leavingCategoryExperience
        };

        var memberId = Guid.NewGuid();
        var user = AuthenticatedUsersForTesting.FakeLocalUserFullyVerifiedClaim(memberId);
        sut.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };
        sut.AddContextWithClaim(ClaimsPrincipalExtensions.ClaimTypes.AanMemberId, Guid.NewGuid().ToString());
        sut.AddUrlHelperMock()
            .AddUrlForRoute(SharedRouteNames.ProfileSettings, ProfileSettingsUrl);

        outerApiMock.Setup(x => x.GetLeavingReasons()).ReturnsAsync(leavingCategories);

        var result = await sut.Index();

        result.Should().BeOfType<ViewResult>();
        var viewResult = result as ViewResult;
        var model = viewResult!.Model as LeaveTheNetworkViewModel;

        Assert.Multiple(() =>
        {
            Assert.That(model!.LeavingReasonsTitle, Is.EqualTo(leavingCategoryReasons.Category));
            Assert.That(model!.LeavingReasons, Is.EqualTo(leavingCategoryReasons.LeavingReasons));
            Assert.That(model!.LeavingBenefitsTitle, Is.EqualTo(leavingCategoryBenefits.Category));
            Assert.That(model!.LeavingBenefits, Is.EqualTo(leavingCategoryBenefits.LeavingReasons));
            Assert.That(model!.LeavingExperienceTitle, Is.EqualTo(leavingCategoryExperience.Category));
            Assert.That(model!.LeavingExperience, Is.EqualTo(leavingCategoryExperience.LeavingReasons));
            Assert.That(model!.ProfileSettingsLink, Is.EqualTo(ProfileSettingsUrl));
        });
    }

    [TestCase(0)]
    [TestCase(ExperienceReason400Selected)]
    public void Post_SetSessionModelAndRedirectToLeaveTheNetworkConfirmation(int experienceRatingSelected)
    {
        var sessionServiceMock = new Mock<ISessionService>();

        var sut = new LeaveTheNetworkController(Mock.Of<IOuterApiClient>(), sessionServiceMock.Object);

        var model = new SubmitLeaveTheNetworkViewModel
        {
            LeavingReasons = GetLeavingCategoryReasons().LeavingReasons,
            LeavingBenefits = GetLeavingCategoryBenefits().LeavingReasons,
            LeavingExperience = GetLeavingCategoryExperience().LeavingReasons,
            SelectedLeavingExperienceRating = experienceRatingSelected
        };

        var memberId = Guid.NewGuid();
        var user = AuthenticatedUsersForTesting.FakeLocalUserFullyVerifiedClaim(memberId);
        sut.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };
        sut.AddContextWithClaim(ClaimsPrincipalExtensions.ClaimTypes.AanMemberId, Guid.NewGuid().ToString());
        sut.AddUrlHelperMock()
            .AddUrlForRoute(SharedRouteNames.LeaveTheNetworkConfirmation, LeaveTheNetworkConfirmationUrl);

        var result = sut.Post(model);

        result.Should().BeOfType<RedirectToRouteResult>();

        var actualResult = result as RedirectToRouteResult;

        actualResult!.RouteName.Should().Be(SharedRouteNames.LeaveTheNetworkConfirmation);

        if (experienceRatingSelected == 0)
        {
            sessionServiceMock.Verify(x => x.Set(
                It.Is<ReasonsForLeavingSessionModel>(m => m.ReasonsForLeaving.Count == 1
                                                          && m.ReasonsForLeaving.Contains(Reason20Selected)
                )));
        }
        else
        {
            sessionServiceMock.Verify(x => x.Set(
                It.Is<ReasonsForLeavingSessionModel>(m => m.ReasonsForLeaving.Count == 2
                                                          && m.ReasonsForLeaving.Contains(Reason20Selected)
                                                          && m.ReasonsForLeaving.Contains(ExperienceReason400Selected)
                )));
        }
    }

    private static LeavingCategory GetLeavingCategoryExperience()
    {
        return new LeavingCategory
        {
            Category = "What was your experience of the AAN portal in enhancing your role as an ambassador",
            LeavingReasons =
            [
                new() { Id = 100, Description = "reason 100", Ordering = 1 },
                new() { Id = 200, Description = "reason 200", Ordering = 2 },
                new() { Id = 300, Description = "reason 300", Ordering = 3 },
                new() { Id = ExperienceReason400Selected, Description = "reason 400", Ordering = 4 }
            ]
        };
    }

    private static LeavingCategory GetLeavingCategoryBenefits()
    {
        return new LeavingCategory
        {
            Category = "Which of the following did you benefit from while you were a member",
            LeavingReasons =
            [
                new() { Id = 10, Description = "reason 10", Ordering = 1 },
                new() { Id = Reason20Selected, Description = "reason 20", Ordering = 2, IsSelected = true },
            ]
        };
    }

    private static LeavingCategory GetLeavingCategoryReasons()
    {
        return new LeavingCategory
        {
            Category = "What are your reasons for leaving the network",
            LeavingReasons =
            [
                new() { Id = 1, Description = "reason 1", Ordering = 1 },
                new() { Id = 2, Description = "reason 2", Ordering = 2 },
                new() { Id = 3, Description = "reason 3", Ordering = 2 }
            ]
        };
    }
}
