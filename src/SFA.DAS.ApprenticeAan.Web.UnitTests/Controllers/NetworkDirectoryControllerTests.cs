using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.Aan.SharedUi.Constants;
using SFA.DAS.Aan.SharedUi.Extensions;
using SFA.DAS.Aan.SharedUi.Infrastructure;
using SFA.DAS.Aan.SharedUi.Models.NetworkDirectory;
using SFA.DAS.Aan.SharedUi.Models.NetworkEvents;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses;
using SFA.DAS.ApprenticeAan.Web.Controllers;
using SFA.DAS.ApprenticeAan.Web.UnitTests.TestHelpers;
using SFA.DAS.ApprenticePortal.Authentication.TestHelpers;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers;

public class NetworkDirectoryControllerTests
{
    private NetworkDirectoryController _sut = null!;
    private static readonly string AllNetworksUrl = Guid.NewGuid().ToString();
    private Mock<IOuterApiClient> _outerApiClientMock = null!;
    private CancellationToken _cancellationToken;
    private GetNetworkDirectoryQueryResult expectedResult = null!;
    private IActionResult _result = null!;

    [SetUp]
    public async Task WhenGettingNetworkDirectory()
    {
        Fixture fixture = new();
        expectedResult = fixture.Create<GetNetworkDirectoryQueryResult>();
        GetRegionsResult expectedRegions = fixture.Create<GetRegionsResult>();
        var request = new NetworkDirectoryRequestModel();
        Guid apprenticeId = Guid.NewGuid();
        _cancellationToken = new();
        _outerApiClientMock = new();
        _outerApiClientMock.Setup(o => o.GetMembers(It.IsAny<Dictionary<string, string[]>>(), _cancellationToken)).ReturnsAsync(expectedResult);
        _outerApiClientMock.Setup(o => o.GetRegions()).ReturnsAsync(expectedRegions);

        var user = AuthenticatedUsersForTesting.FakeLocalUserFullyVerifiedClaim(apprenticeId);

        _sut = new(_outerApiClientMock.Object);
        _sut.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };
        _sut.AddUrlHelperMock().AddUrlForRoute(SharedRouteNames.NetworkDirectory, AllNetworksUrl);
        _result = await _sut.Index(request, _cancellationToken);
    }

    [Test]
    public void ThenReturnsView()
        => _result.Should().BeOfType<ViewResult>();

    [Test]
    public void ThenRetrievesMembers()
        => _outerApiClientMock.Verify(o => o.GetMembers(It.IsAny<Dictionary<string, string[]>>(), _cancellationToken), Times.Once);

    [Test]
    public void ThenRetrievesRegions()
        => _outerApiClientMock.Verify(o => o.GetRegions());


    [Test, MoqAutoData]
    public async Task Index_ApplyFilterForKeyword_KeywordTotalCountAndTotalCountDescriptionAreEqual(string keyword)
    {
        var userTyoes = new List<Role>
        {
            Role.Employer,
            Role.Apprentice,
            Role.RegionalChair
        };
        bool? isRegionalChair = null;
        var regions = new List<int>();

        var request = new NetworkDirectoryRequestModel
        {
            Keyword = keyword,
            UserRole = userTyoes,
            IsRegionalChair = isRegionalChair,
            RegionId = regions,
            Page = expectedResult.Page,
            PageSize = expectedResult.PageSize,
        };

        //action
        var actualResult = await _sut.Index(request, new CancellationToken());
        var viewResult = actualResult.As<ViewResult>();
        var sut = viewResult.Model as NetworkDirectoryViewModel;

        sut!.TotalCount.Should().Be(expectedResult.TotalCount);
        if (sut!.TotalCount == 1)
        {
            sut!.TotalCountDescription.Should().Be("1 result");
        }
        else
        {
            sut!.TotalCountDescription.Should().Be($"{sut!.TotalCount} results");
        }
        sut.FilterChoices.Keyword.Should().Be(keyword);
    }

    [Test]
    public void Index_NoFilters_ClearLinkAndRolelistAreEqual()
    {
        var request = new NetworkDirectoryRequestModel();
        var expectedEventFormatChecklistLookup = GetUserRoleCheckListLookup(request);

        //action
        var actualResult = _sut.Index(request, new CancellationToken());
        var viewResult = actualResult.Result.As<ViewResult>();
        var sut = viewResult.Model as NetworkDirectoryViewModel;

        sut!.FilterChoices.RoleChecklistDetails.Lookups.Should().BeEquivalentTo(expectedEventFormatChecklistLookup);
        sut!.SelectedFiltersModel.ClearSelectedFiltersLink.Should().Be(AllNetworksUrl);
    }

    [Test]
    public void Index_NoFilters_KeywordIsEqualWithNullAndTotalCountDescriptionIsEqual()
    {
        var request = new NetworkDirectoryRequestModel();
        var actualResult = _sut.Index(request, new CancellationToken());
        var viewResult = actualResult.Result.As<ViewResult>();
        var sut = viewResult.Model as NetworkDirectoryViewModel;
        sut!.TotalCount.Should().Be(expectedResult.TotalCount);
        if (sut!.TotalCount == 1)
        {
            sut!.TotalCountDescription.Should().Be("1 result");
        }
        else
        {
            sut!.TotalCountDescription.Should().Be($"{sut!.TotalCount} results");
        }
        sut.FilterChoices.Keyword.Should().BeNull();
    }

    [Test]
    public void Index_NoFilters_PaginationViewModelIsEqual()
    {
        var request = new NetworkDirectoryRequestModel();
        var actualResult = _sut.Index(request, new CancellationToken());

        var viewResult = actualResult.Result.As<ViewResult>();
        var sut = viewResult.Model as NetworkDirectoryViewModel;
        sut!.PaginationViewModel.CurrentPage.Should().Be(expectedResult.Page);
        sut!.PaginationViewModel.PageSize.Should().Be(expectedResult.PageSize);
        sut!.PaginationViewModel.TotalPages.Should().Be(expectedResult.TotalPages);
    }


    private static ChecklistLookup[] GetUserRoleCheckListLookup(NetworkDirectoryRequestModel networkDirectoryRequestModel)
    {
        return new ChecklistLookup[]
        {
            new(Role.Apprentice.GetDescription(), Role.Apprentice.ToString(),
                networkDirectoryRequestModel.UserRole.Exists(x => x == Role.Apprentice)),
            new(Role.Employer.GetDescription(), Role.Employer.ToString(),
                networkDirectoryRequestModel.UserRole.Exists(x => x == Role.Employer)),
            new(Role.RegionalChair.GetDescription(), Role.RegionalChair.ToString(),
                networkDirectoryRequestModel.UserRole.Exists(x => x == Role.RegionalChair))
        };
    }

}
