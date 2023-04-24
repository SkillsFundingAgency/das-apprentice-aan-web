using AutoFixture;
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

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers.Onboarding.CheckYourAnswersControllerTests.WhenGetIsInvoked;

public class AndSessionModelIsPopulated
{
    ViewResult getResult;
    CheckYourAnswersViewModel viewModel;
    static readonly string? JobTitle = Guid.NewGuid().ToString();
    static readonly string JobTitleUrl = Guid.NewGuid().ToString();
    static readonly string? RegionName = Guid.NewGuid().ToString();
    static readonly string RegionUrl = Guid.NewGuid().ToString();
    static readonly string? ReasonForJoiningTheNetwork = Guid.NewGuid().ToString();
    static readonly string ReasonForJoiningTheNetworkUrl = Guid.NewGuid().ToString();
    static readonly List<ProfileModel> AreasOfInterest = new List<ProfileModel>() {
        new ProfileModel { Id = 1, Category = Category.Events, Value = "Presenting at online events" },
        new ProfileModel { Id = 2, Category = Category.Events, Value = "Networking at events in person" },
        new ProfileModel { Id = 3, Category = Category.Events, Value = null },
        new ProfileModel { Id = 4, Category = Category.Promotions, Value = "Carrying out and writing up case studies" },
        new ProfileModel { Id = 5, Category = Category.Promotions, Value = "Promoting the network on social media channels" },
        new ProfileModel { Id = 6, Category = Category.Promotions, Value = null }
    };
    static readonly string AreasOfInterestUrl = Guid.NewGuid().ToString();
    static readonly string? IsPreviouslyEngagedWithNetwork = "true";
    static readonly string PreviousEngagementUrl = Guid.NewGuid().ToString();
    static readonly string EmployerName = Guid.NewGuid().ToString();
    static readonly string AddressLine1 = Guid.NewGuid().ToString();
    static readonly string AddressLine2 = Guid.NewGuid().ToString();
    static readonly string Town = Guid.NewGuid().ToString();
    static readonly string County = Guid.NewGuid().ToString();
    static readonly string Postcode = Guid.NewGuid().ToString();
    static readonly string EmployerSearchUrl = Guid.NewGuid().ToString();

    OnboardingSessionModel sessionModel;
    CheckYourAnswersController sut;

    [SetUp]
    public void Init()
    {
        var fixture = new Fixture();
        sessionModel = fixture.Create<OnboardingSessionModel>();
        Mock<ISessionService> sessionServiceMock = new();
        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);
        sut = new(sessionServiceMock.Object);

        sut.AddUrlHelperMock()
        .AddUrlForRoute(RouteNames.Onboarding.CurrentJobTitle, JobTitleUrl)
        .AddUrlForRoute(RouteNames.Onboarding.Regions, RegionUrl)
        .AddUrlForRoute(RouteNames.Onboarding.ReasonToJoin, ReasonForJoiningTheNetworkUrl)
        .AddUrlForRoute(RouteNames.Onboarding.AreasOfInterest, AreasOfInterestUrl)
        .AddUrlForRoute(RouteNames.Onboarding.PreviousEngagement, PreviousEngagementUrl)
        .AddUrlForRoute(RouteNames.Onboarding.EmployerSearch, EmployerSearchUrl);

        sessionModel.ProfileData.Add(new ProfileModel { Id = ProfileDataId.JobTitle, Value = JobTitle });
        sessionModel.RegionName = RegionName;
        sessionModel.ApprenticeDetails.ReasonForJoiningTheNetwork = ReasonForJoiningTheNetwork;
        sessionModel.ProfileData.Add(new ProfileModel { Id = ProfileDataId.HasPreviousEngagement, Value = IsPreviouslyEngagedWithNetwork });
        sessionModel.ProfileData.AddRange(AreasOfInterest);
        sessionModel.ProfileData.Add(new ProfileModel { Id = ProfileDataId.EmployerName, Value = EmployerName });
        sessionModel.ProfileData.Add(new ProfileModel { Id = ProfileDataId.AddressLine1, Value = AddressLine1 });
        sessionModel.ProfileData.Add(new ProfileModel { Id = ProfileDataId.AddressLine2, Value = AddressLine2 });
        sessionModel.ProfileData.Add(new ProfileModel { Id = ProfileDataId.Town, Value = Town });
        sessionModel.ProfileData.Add(new ProfileModel { Id = ProfileDataId.County, Value = County });
        sessionModel.ProfileData.Add(new ProfileModel { Id = ProfileDataId.Postcode, Value = Postcode });

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
    public void ThenSetsJobTitleInViewModel()
    {
        viewModel.JobTitle.Should().Be(JobTitle);
        viewModel.JobTitleChangeLink.Should().Be(JobTitleUrl);
    }

    [Test]
    public void ThenSetsRegionInViewModel()
    {
        viewModel.Region.Should().Be(RegionName);
        viewModel.RegionChangeLink.Should().Be(RegionUrl);
    }

    [Test]
    public void ThenSetsReasonToJoinTheNetworkInViewModel()
    {
        viewModel.ReasonForJoiningTheNetwork.Should().Be(ReasonForJoiningTheNetwork);
        viewModel.ReasonForJoiningTheNetworkChangeLink.Should().Be(ReasonForJoiningTheNetworkUrl);
    }

    [Test]
    public void ThenSetsAreaOfInterestsInViewModel()
    {
        viewModel.AreasOfInterest.Should().Equal(AreasOfInterest.Where(x => (x.Category == Category.Events || x.Category == Category.Promotions) && x.Value != null).Select(x => x.Description).ToList());
        viewModel.AreasOfInterestChangeLink.Should().Be(AreasOfInterestUrl);
    }

    [TestCase("true")]
    [TestCase("false")]
    [TestCase(null)]
    public void ThenSetsPreviousEngagementInViewModel(string isPreviouslyEngagged)
    {
        sessionModel.SetProfileValue(ProfileDataId.HasPreviousEngagement, isPreviouslyEngagged);
        getResult = sut.Get().As<ViewResult>();
        viewModel = getResult.Model.As<CheckYourAnswersViewModel>();

        viewModel.PreviousEngagement.Should().Be(CheckYourAnswersViewModel.GetPreviousEngagementValue(isPreviouslyEngagged));
        viewModel.PreviousEngagementChangeLink.Should().Be(PreviousEngagementUrl);
    }

    [Test]
    public void ThenSetsEmployerNameAndAddressInViewModel()
    {
        viewModel.CurrentEmployerName.Should().Be(EmployerName);
        viewModel.CurrentEmployerChangeLink.Should().Be(EmployerSearchUrl);
        viewModel.CurrentEmployerAddress.Should().Be($@"{AddressLine1} {AddressLine2} {County} {Town} {Postcode}");
    }
}
