using AutoFixture.NUnit3;
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
    private static readonly string AllNetworksUrl = Guid.NewGuid().ToString();

    [Test, MoqAutoData]
    public void GetNetworkDirectory_ReturnsApiResponse(
    [Frozen] Mock<IOuterApiClient> outerApiMock,
    [Greedy] NetworkDirectoryController sut,
    GetNetworkDirectoryQueryResult expectedResult,
    string keyword,
    Guid apprenticeId)
    {
        var userTyoes = new List<Role>
        {
            Role.Employer,
            Role.Apprentice,
            Role.IsRegionalChair
        };
        bool? isRegionalChair = null;
        var regions = new List<int>();

        var request = new GetNetworkDirectoryRequest
        {
            Keyword = keyword,
            UserType = userTyoes,
            IsRegionalChair = isRegionalChair,
            RegionId = regions,
            Page = expectedResult.Page,
            PageSize = expectedResult.PageSize,
        };

        var user = AuthenticatedUsersForTesting.FakeLocalUserFullyVerifiedClaim(apprenticeId);
        outerApiMock.Setup(o => o.GetMembers(It.IsAny<Dictionary<string, string[]>>(), It.IsAny<CancellationToken>())).ReturnsAsync(expectedResult);

        sut.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };
        sut.AddUrlHelperMock().AddUrlForRoute(SharedRouteNames.NetworkDirectory, AllNetworksUrl);

        //action
        var actualResult = sut.Index(request, new CancellationToken());

        var expectedEventFormatChecklistLookup = new ChecklistLookup[]
        {
            new(Role.Apprentice.GetDescription()!, Role.Apprentice.ToString(),
                request.UserType.Exists(x => x == Role.Apprentice)),
            new(Role.Employer.GetDescription()!, Role.Employer.ToString(),
                request.UserType.Exists(x => x == Role.Employer)),
            new(Role.IsRegionalChair.GetDescription()!, Role.IsRegionalChair.ToString(),
                request.UserType.Exists(x => x == Role.IsRegionalChair))
        };

        var viewResult = actualResult.Result.As<ViewResult>();
        var model = viewResult.Model as NetworkDirectoryViewModel;
        model!.PaginationViewModel.CurrentPage.Should().Be(expectedResult.Page);
        model!.PaginationViewModel.PageSize.Should().Be(expectedResult.PageSize);
        model!.PaginationViewModel.TotalPages.Should().Be(expectedResult.TotalPages);
        model!.TotalCount.Should().Be(expectedResult.TotalCount);
        model.FilterChoices.Keyword.Should().Be(keyword);
        model.FilterChoices.RoleChecklistDetails.Lookups.Should().BeEquivalentTo(expectedEventFormatChecklistLookup);
        model.ClearSelectedFiltersLink.Should().Be(AllNetworksUrl);

        outerApiMock.Verify(o => o.GetMembers(It.IsAny<Dictionary<string, string[]>>(), It.IsAny<CancellationToken>()), Times.Once);
    }
    [Test, MoqAutoData]
    public void GetNetworkDirectoryNoFilters_ReturnsApiResponse(
   [Frozen] Mock<IOuterApiClient> outerApiMock,
   [Greedy] NetworkDirectoryController sut,
           GetNetworkDirectoryQueryResult expectedResult,
           Guid apprenticeId)
    {
        var request = new GetNetworkDirectoryRequest();

        var user = AuthenticatedUsersForTesting.FakeLocalUserFullyVerifiedClaim(apprenticeId);
        outerApiMock.Setup(o => o.GetMembers(It.IsAny<Dictionary<string, string[]>>(), It.IsAny<CancellationToken>())).ReturnsAsync(expectedResult);

        sut.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };
        sut.AddUrlHelperMock().AddUrlForRoute(SharedRouteNames.NetworkDirectory, AllNetworksUrl);

        var actualResult = sut.Index(request, new CancellationToken());
        var expectedEventFormatChecklistLookup = new ChecklistLookup[]
        {
            new(Role.Apprentice.GetDescription()!, Role.Apprentice.ToString(),
                request.UserType.Exists(x => x == Role.Apprentice)),
            new(Role.Employer.GetDescription()!, Role.Employer.ToString(),
                request.UserType.Exists(x => x == Role.Employer)),
            new(Role.IsRegionalChair.GetDescription()!, Role.IsRegionalChair.ToString(),
                request.UserType.Exists(x => x == Role.IsRegionalChair))
        };

        var viewResult = actualResult.Result.As<ViewResult>();
        var model = viewResult.Model as NetworkDirectoryViewModel;
        model!.PaginationViewModel.CurrentPage.Should().Be(expectedResult.Page);
        model!.PaginationViewModel.PageSize.Should().Be(expectedResult.PageSize);
        model!.PaginationViewModel.TotalPages.Should().Be(expectedResult.TotalPages);
        model!.TotalCount.Should().Be(expectedResult.TotalCount);
        model.FilterChoices.Keyword.Should().BeNull();
        outerApiMock.Verify(o => o.GetMembers(It.IsAny<Dictionary<string, string[]>>(), It.IsAny<CancellationToken>()), Times.Once);
    }

}
