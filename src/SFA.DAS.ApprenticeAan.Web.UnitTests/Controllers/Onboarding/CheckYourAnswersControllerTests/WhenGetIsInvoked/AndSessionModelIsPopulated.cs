using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.ApprenticeAan.Domain.Constants;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses;
using SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;
using SFA.DAS.ApprenticeAan.Web.UnitTests.TestHelpers;
using SFA.DAS.ApprenticePortal.Authentication;
using SFA.DAS.ApprenticePortal.Authentication.TestHelpers;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers.Onboarding.CheckYourAnswersControllerTests.WhenGetIsInvoked;

public class AndSessionModelIsPopulated
{
    static readonly string JobTitleUrl = Guid.NewGuid().ToString();
    static readonly string RegionUrl = Guid.NewGuid().ToString();
    static readonly string ReasonForJoiningTheNetworkUrl = Guid.NewGuid().ToString();
    static readonly string AreasOfInterestUrl = Guid.NewGuid().ToString();
    static readonly string PreviousEngagementUrl = Guid.NewGuid().ToString();
    static readonly string EmployerSearchUrl = Guid.NewGuid().ToString();
    static readonly IEnumerable<int> AddressIds = Enumerable.Range(31, 5);


    private readonly Fixture _fixture = new();
    private CheckYourAnswersViewModel _actualViewModel;
    private CheckYourAnswersController _sut;
    private ViewResult _actualViewResult;
    private OnboardingSessionModel _sessionModel;
    private MyApprenticeship _myApprenticeship;

    private List<ProfileModel> GetProfileData()
    {
        int[] profileIds = new[] { 20, 30 };
        var profileData = _fixture.Build<ProfileModel>().WithValues(p => p.Id, profileIds).CreateMany(profileIds.Length).ToList();

        var i = 0;
        profileData.AddRange(_fixture.Build<ProfileModel>().WithValues(p => p.Id, AddressIds.ToArray()).CreateMany(AddressIds.Count()));

        profileData
            .Add(_fixture.Build<ProfileModel>()
            .With(p => p.Id, ProfileDataId.HasPreviousEngagement)
            .With(p => p.Value, "true")
            .Create());

        string[] areasOfInterestCategories = new[] { Category.Promotions, Category.Events };
        profileData.AddRange(_fixture.Build<ProfileModel>().WithValues(p => p.Id, 1, 2).WithValues(p => p.Category, areasOfInterestCategories).CreateMany(areasOfInterestCategories.Length));
        // Add null values for the same categories for exclusion
        profileData.AddRange(_fixture.Build<ProfileModel>().WithValues(p => p.Id, 3, 4).WithValues(p => p.Category, areasOfInterestCategories).Without(p => p.Value).CreateMany(areasOfInterestCategories.Length));
        return profileData;
    }

    [SetUp]
    public async Task SetUp()
    {
        // Arrange
        _sessionModel = _fixture
            .Build<OnboardingSessionModel>()
            .With(m => m.ProfileData, GetProfileData())
            .Create();

        Mock<ISessionService> sessionServiceMock = new();
        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(_sessionModel);

        var apprenticeId = _fixture.Create<Guid>();

        _myApprenticeship = _fixture.Create<MyApprenticeship>();
        Mock<IOuterApiClient> outerApiClientMock = new();
        outerApiClientMock.Setup(o => o.GetMyApprenticeship(apprenticeId)).ReturnsAsync(_myApprenticeship);

        _sut = new(sessionServiceMock.Object, outerApiClientMock.Object);
        var user = AuthenticatedUsersForTesting.FakeLocalUserFullyVerifiedClaim(apprenticeId);
        _sut.ControllerContext = new() { HttpContext = new DefaultHttpContext() { User = user } };

        _sut.AddUrlHelperMock()
            .AddUrlForRoute(RouteNames.Onboarding.CurrentJobTitle, JobTitleUrl)
            .AddUrlForRoute(RouteNames.Onboarding.Regions, RegionUrl)
            .AddUrlForRoute(RouteNames.Onboarding.ReasonToJoin, ReasonForJoiningTheNetworkUrl)
            .AddUrlForRoute(RouteNames.Onboarding.AreasOfInterest, AreasOfInterestUrl)
            .AddUrlForRoute(RouteNames.Onboarding.PreviousEngagement, PreviousEngagementUrl)
            .AddUrlForRoute(RouteNames.Onboarding.EmployerSearch, EmployerSearchUrl);

        // Action
        var result = await _sut.Get();

        _actualViewResult = result.As<ViewResult>();
        _actualViewModel = _actualViewResult.Model.As<CheckYourAnswersViewModel>();
    }

    [Test]
    public void ThenReturnsViewResults()
    {
        _actualViewResult.Should().NotBeNull();
        _actualViewResult.ViewName.Should().Be(CheckYourAnswersController.ViewPath);
    }

    [Test]
    public void ThenSetsJobTitleInViewModel()
    {
        _actualViewModel.JobTitle.Should().Be(_sessionModel.ProfileData.First(p => p.Id == ProfileDataId.JobTitle).Value);
        _actualViewModel.JobTitleChangeLink.Should().Be(JobTitleUrl);
    }

    [Test]
    public void ThenSetsRegionInViewModel()
    {
        _actualViewModel.Region.Should().Be(_sessionModel.RegionName);
        _actualViewModel.RegionChangeLink.Should().Be(RegionUrl);
    }

    [Test]
    public void ThenSetsReasonToJoinTheNetworkInViewModel()
    {
        _actualViewModel.ReasonForJoiningTheNetwork.Should().Be(_sessionModel.ApprenticeDetails.ReasonForJoiningTheNetwork);
        _actualViewModel.ReasonForJoiningTheNetworkChangeLink.Should().Be(ReasonForJoiningTheNetworkUrl);
    }

    [Test]
    public void ThenSetsAreaOfInterestsInViewModel()
    {
        _actualViewModel.AreasOfInterest.Should().Equal(_sessionModel.ProfileData.Where(x => (x.Category == Category.Events || x.Category == Category.Promotions) && x.Value != null).Select(x => x.Description).ToList());
        _actualViewModel.AreasOfInterestChangeLink.Should().Be(AreasOfInterestUrl);
    }

    [TestCase("true", "Yes")]
    [TestCase("false", "No")]
    [TestCase(null, null)]
    public async Task ThenSetsPreviousEngagementInViewModel(string? isPreviouslyEngaged, string? expectedValue)
    {
        _sessionModel.SetProfileValue(ProfileDataId.HasPreviousEngagement, isPreviouslyEngaged!);
        var actualResult = await _sut.Get();
        var actualViewModel = actualResult.As<ViewResult>().Model.As<CheckYourAnswersViewModel>();

        actualViewModel.PreviousEngagement.Should().Be(expectedValue);
        actualViewModel.PreviousEngagementChangeLink.Should().Be(PreviousEngagementUrl);
    }

    [Test]
    public void ThenSetsEmployerNameInViewModel()
    {
        _actualViewModel.CurrentEmployerName.Should().Be(_sessionModel.ProfileData.First(p => p.Id == ProfileDataId.EmployerName).Value);
        _actualViewModel.CurrentEmployerChangeLink.Should().Be(EmployerSearchUrl);
    }

    [Test]
    public void ThenSetsAddressInViewModel()
    {
        string completeAddress = string.Join(", ", _sessionModel.ProfileData.Where(p => AddressIds.Contains(p.Id)).OrderBy(p => p.Id).Select(p => p.Value));
        _actualViewModel.CurrentEmployerAddress.Should().Be(completeAddress);
    }

    [Test]
    public void ThenSetsApprenticesFullName()
    {
        _actualViewModel.FullName.Should().Be(_sut.User.FullName());
    }

    [Test]
    public void ThenSetsApprenticesEmail()
    {
        _actualViewModel.Email.Should().Be(_sut.User.EmailAddressClaim()!.Value);
    }

    [Test]
    public void ThenSetsApprenticeshipDuration()
    {
        _actualViewModel.ApprenticeshipDuration.Should().Contain(_myApprenticeship.StartDate.GetValueOrDefault().Date.ToString("dd-MM-yyyy"));
        _actualViewModel.ApprenticeshipDuration.Should().Contain(_myApprenticeship.EndDate.GetValueOrDefault().Date.ToString("dd-MM-yyyy"));
    }

    [Test]
    public void ThenSetsApprenticeshipSector()
    {
        _actualViewModel.ApprenticeshipSector.Should().Be(_myApprenticeship.TrainingCourse.Sector);
    }

    [Test]
    public void ThenSetsApprenticeshipProgram()
    {
        _actualViewModel.ApprenticeshipProgram.Should().Be(_myApprenticeship.TrainingCourse.Name);
    }

    [Test]
    public void ThenSetsApprenticeshipLevel()
    {
        _actualViewModel.ApprenticeshipLevel.Should().Be($"Level {_myApprenticeship.TrainingCourse.Level}");
    }
}
