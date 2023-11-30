using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using SFA.DAS.Aan.SharedUi.Infrastructure;
using SFA.DAS.Aan.SharedUi.Models;
using SFA.DAS.Aan.SharedUi.Models.AmbassadorProfile;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses;
using SFA.DAS.ApprenticeAan.Web.Controllers;
using SFA.DAS.ApprenticeAan.Web.Extensions;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.UnitTests.TestHelpers;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers;
public class AmbassadorProfileControllerTests
{
    static readonly string YourAmbassadorProfileUrl = Guid.NewGuid().ToString();
    private IActionResult _result = null!;
    private Mock<IOuterApiClient> _outerApiClientMock = null!;
    private AmbassadorProfileController _sut = null!;
    private GetMemberProfileResponse memberProfile = null!;
    private CancellationToken _cancellationToken;
    private readonly List<Profile> profiles = new()
    {
        new Profile { Id = 1, Description = "Networking at events in person", Category = "Events", Ordering = 1 },
        new Profile { Id = 2, Description = "Presenting at events in person", Category = "Events", Ordering = 2 },
        new Profile { Id = 10, Description = "Carrying out and writing up case studies", Category = "Promotions", Ordering = 1 },
        new Profile { Id = 11, Description = "Designing and creating marketing materials to champion the network", Category = "Promotions", Ordering = 2 }
    };

    [SetUp]
    public async Task Setup()
    {
        var memberId = Guid.NewGuid();
        _cancellationToken = new();

        Fixture fixture = new();
        memberProfile = fixture.Create<GetMemberProfileResponse>();
        _outerApiClientMock = new();
        _outerApiClientMock.Setup(o => o.GetMemberProfile(memberId, memberId, false, _cancellationToken)).ReturnsAsync(memberProfile);
        _outerApiClientMock.Setup(o => o.GetProfilesByUserType(MemberUserType.Apprentice.ToString(), It.IsAny<CancellationToken>())).ReturnsAsync(new GetProfilesResult() { Profiles = profiles });

        _sut = new(_outerApiClientMock.Object);
        _sut.AddUrlHelperMock().AddUrlForRoute(SharedRouteNames.YourAmbassadorProfile, YourAmbassadorProfileUrl);
        _sut.AddContextWithClaim(ClaimsPrincipalExtensions.ClaimTypes.AanMemberId, memberId.ToString());
        var viewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary());
        _sut.ViewData = viewData;
        Mock<ITempDataDictionary> tempDataMock = new Mock<ITempDataDictionary>();
        tempDataMock.Setup(t => t.ContainsKey(TempDataKeys.YourAmbassadorProfileSuccessMessage)).Returns(true);
        _sut.TempData = tempDataMock.Object;

        _result = await _sut.Index(_cancellationToken);
    }

    [Test]
    public void ThenReturnsView()
        => _result.Should().BeOfType<ViewResult>();

    [Test]
    public void ThenRetrievesMemberProfiles()
        => _outerApiClientMock.Verify(o => o.GetMemberProfile(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()));

    [Test]
    public void ThenRetrievesProfiles()
        => _outerApiClientMock.Verify(o => o.GetProfilesByUserType(It.IsAny<string>(), It.IsAny<CancellationToken>()));

    [Test]
    public void ThenSetsViewModel()
    => _result.As<ViewResult>().Model.Should().BeOfType<AmbassadorProfileViewModel>();

    [Test]
    public void ThenSetsViewModelWithApprenticeshipDetails()
    {
        if (memberProfile.Apprenticeship == null)
        {
            _result.Invoking(r => r.As<ViewResult>().Model.As<AmbassadorProfileViewModel>().ApprenticeshipDetails.ApprenticeshipLevel.Should().BeNull());
        }
        else
        {
            _result.Invoking(r => r.As<ViewResult>().Model.As<AmbassadorProfileViewModel>().ApprenticeshipDetails.ApprenticeshipLevel.Should().Be(memberProfile.Apprenticeship.Level));
        }
    }

    [Test]
    public void ThenSetsViewModelWithPersonalDetails()
        => _result.Invoking(r => r.As<ViewResult>().Model.As<AmbassadorProfileViewModel>().PersonalDetails.FullName.Should().Be(memberProfile.FullName));

    private static IEnumerable<ApprenticeshipDetails?> GetApprenticeshipDetails()
    {
        yield return new ApprenticeshipDetails { Sector = string.Empty, Level = string.Empty, Programme = string.Empty };
        yield return null;
    }

    [Test]
    public async Task Index_SetApprenticeshipDetails_ReturnsView([ValueSource(nameof(GetApprenticeshipDetails))] ApprenticeshipDetails? apprenticeshipDetails)
    {
        //Arrange
        AmbassadorProfileController sut = null!;
        var memberId = Guid.NewGuid();
        Fixture fixture = new();
        memberProfile = fixture.Create<GetMemberProfileResponse>();
        memberProfile.Apprenticeship = apprenticeshipDetails;
        _outerApiClientMock = new();
        _outerApiClientMock.Setup(o => o.GetMemberProfile(memberId, memberId, false, _cancellationToken)).ReturnsAsync(memberProfile);
        _outerApiClientMock.Setup(o => o.GetProfilesByUserType(MemberUserType.Apprentice.ToString(), It.IsAny<CancellationToken>())).ReturnsAsync(new GetProfilesResult() { Profiles = profiles });
        sut = new(_outerApiClientMock.Object);
        sut.AddUrlHelperMock().AddUrlForRoute(SharedRouteNames.YourAmbassadorProfile, YourAmbassadorProfileUrl);
        sut.AddContextWithClaim(ClaimsPrincipalExtensions.ClaimTypes.AanMemberId, memberId.ToString());
        var viewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary());
        sut.ViewData = viewData;
        Mock<ITempDataDictionary> tempDataMock = new Mock<ITempDataDictionary>();
        tempDataMock.Setup(t => t.ContainsKey(TempDataKeys.YourAmbassadorProfileSuccessMessage)).Returns(true);
        sut.TempData = tempDataMock.Object;

        //Act
        _result = await sut.Index(_cancellationToken);

        //Assert
        Assert.That(_result, Is.InstanceOf<ViewResult>());
    }
}