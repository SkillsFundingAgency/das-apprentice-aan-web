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

        Assert.That(checkYourAnswersViewModel.JobTitleChangeLink.Equals(TestConstants.DefaultUrl));
    }

    [Test]
    public void Ctor_SetsJobTitle()
    {
        var expectedJobTitle = "Some Title";
        Mock<IUrlHelper> mockUrlHelper = new();
        OnboardingSessionModel sessionModel = new();
        sessionModel.ProfileData.Add(new ProfileModel { Id = ProfileDataId.JobTitle, Value = expectedJobTitle });

        var checkYourAnswersViewModel = new CheckYourAnswersViewModel(mockUrlHelper.Object, sessionModel);

        Assert.That(checkYourAnswersViewModel.JobTitle.Equals(expectedJobTitle));
    }
}