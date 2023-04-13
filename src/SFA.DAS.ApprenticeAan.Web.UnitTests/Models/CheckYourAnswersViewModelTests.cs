using AutoFixture.NUnit3;
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
    [AutoData]
    public void Ctor_SetsJobTitleChangeLink(string currentJobTitleUrl)
    {
        var expectedJobTitle = "Some Title";
        Mock<IUrlHelper> mockUrlHelper = new();
        mockUrlHelper.AddUrlForRoute(RouteNames.Onboarding.CurrentJobTitle, currentJobTitleUrl);

        OnboardingSessionModel sessionModel = new();
        sessionModel.ProfileData.Add(new ProfileModel { Id = ProfileDataId.JobTitle, Value = expectedJobTitle });

        var checkYourAnswersViewModel = new CheckYourAnswersViewModel(mockUrlHelper.Object, sessionModel);

        Assert.That(checkYourAnswersViewModel.JobTitleChangeLink, Is.EqualTo(currentJobTitleUrl));
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

    [AutoData]
    public void Ctor_SetsRegionChangeLink(string regionsUrl)
    {
        var expectedRegion = 101;
        var expectedJobTitle = "Some Title";

        Mock<IUrlHelper> mockUrlHelper = new();
        mockUrlHelper.AddUrlForRoute(RouteNames.Onboarding.Regions, regionsUrl);

        OnboardingSessionModel sessionModel = new();
        sessionModel.ProfileData.Add(new ProfileModel { Id = ProfileDataId.JobTitle, Value = expectedJobTitle });

        sessionModel.RegionId = expectedRegion;

        var checkYourAnswersViewModel = new CheckYourAnswersViewModel(mockUrlHelper.Object, sessionModel);

        Assert.That(checkYourAnswersViewModel.RegionChangeLink, Is.EqualTo(regionsUrl));
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

    [Test]
    public void Ctor_SetsReasonForJoiningTheNetworkChangeLink()
    {
        var expectedJobTitle = "Some Title";

        Mock<IUrlHelper> mockUrlHelper = new();
        mockUrlHelper.AddUrlForRoute(RouteNames.Onboarding.ReasonToJoin);

        OnboardingSessionModel sessionModel = new();
        sessionModel.ProfileData.Add(new ProfileModel { Id = ProfileDataId.JobTitle, Value = expectedJobTitle });

        var checkYourAnswersViewModel = new CheckYourAnswersViewModel(mockUrlHelper.Object, sessionModel);

        Assert.That(checkYourAnswersViewModel.ReasonForJoiningTheNetworkChangeLink, Is.EqualTo(TestConstants.DefaultUrl));
    }

    [Test]
    public void Ctor_SetsReasonForJoiningTheNetwork()
    {
        var expectedJobTitle = "Some Title";
        var expectedReasonForJoiningTheNetwork = "The reason to join the network.";

        Mock<IUrlHelper> mockUrlHelper = new();
        OnboardingSessionModel sessionModel = new();
        sessionModel.ProfileData.Add(new ProfileModel { Id = ProfileDataId.JobTitle, Value = expectedJobTitle });
        sessionModel.ApprenticeDetails.ReasonForJoiningTheNetwork = expectedReasonForJoiningTheNetwork;

        var checkYourAnswersViewModel = new CheckYourAnswersViewModel(mockUrlHelper.Object, sessionModel);

        Assert.That(checkYourAnswersViewModel.ReasonForJoiningTheNetwork, Is.EqualTo(expectedReasonForJoiningTheNetwork));
    }
}