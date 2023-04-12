using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.ApprenticeAan.Domain.Constants;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;
using SFA.DAS.ApprenticeAan.Web.UnitTests.TestHelpers;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Models;

[TestFixture]
public class CheckYourAnswersViewModelTests
{
    [Test]
    public void Ctor_SetsJobTitleChangeLink()
    {
        var expectedJobTitle = "Some Title";
        Mock<IUrlHelper> mockUrlHelper = new();
        mockUrlHelper.AddUrlForRoute(RouteNames.Onboarding.CurrentJobTitle);

        OnboardingSessionModel sessionModel = new();
        sessionModel.ProfileData.Add(new ProfileModel { Id = ProfileDataId.JobTitle, Value = expectedJobTitle });

        var checkYourAnswersViewModel = new CheckYourAnswersViewModel(mockUrlHelper.Object, sessionModel);

        Assert.That(checkYourAnswersViewModel.JobTitleChangeLink, Is.EqualTo(TestConstants.DefaultUrl));
    }

    [Test]
    public void Ctor_SetsJobTitle()
    {
        var expectedJobTitle = "Some Title";
        Mock<IUrlHelper> mockUrlHelper = new();
        OnboardingSessionModel sessionModel = new();
        sessionModel.ProfileData.Add(new ProfileModel { Id = ProfileDataId.JobTitle, Value = expectedJobTitle });

        var checkYourAnswersViewModel = new CheckYourAnswersViewModel(mockUrlHelper.Object, sessionModel);

        Assert.That(checkYourAnswersViewModel.JobTitle, Is.EqualTo(expectedJobTitle));
    }

    [Test]
    public void Ctor_SetsRegionChangeLink()
    {
        var expectedRegion = 101;
        var expectedJobTitle = "Some Title";

        Mock<IUrlHelper> mockUrlHelper = new();
        mockUrlHelper.AddUrlForRoute(RouteNames.Onboarding.Regions);

        OnboardingSessionModel sessionModel = new();
        sessionModel.ProfileData.Add(new ProfileModel { Id = ProfileDataId.JobTitle, Value = expectedJobTitle });

        sessionModel.RegionId = expectedRegion;

        var checkYourAnswersViewModel = new CheckYourAnswersViewModel(mockUrlHelper.Object, sessionModel);

        Assert.That(checkYourAnswersViewModel.RegionChangeLink, Is.EqualTo(TestConstants.DefaultUrl));
    }

    [Test]
    public void Ctor_SetsRegion()
    {
        var expectedJobTitle = "Some Title";
        var expectedRegion = "London";

        Mock<IUrlHelper> mockUrlHelper = new();
        OnboardingSessionModel sessionModel = new();
        sessionModel.ProfileData.Add(new ProfileModel { Id = ProfileDataId.JobTitle, Value = expectedJobTitle });
        sessionModel.RegionName = expectedRegion;

        var checkYourAnswersViewModel = new CheckYourAnswersViewModel(mockUrlHelper.Object, sessionModel);

        Assert.That(checkYourAnswersViewModel.Region, Is.EqualTo(expectedRegion));
    }
}