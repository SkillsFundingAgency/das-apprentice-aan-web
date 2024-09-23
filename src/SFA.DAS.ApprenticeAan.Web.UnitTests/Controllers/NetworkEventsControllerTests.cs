using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.Aan.SharedUi.Constants;
using SFA.DAS.Aan.SharedUi.Extensions;
using SFA.DAS.Aan.SharedUi.Infrastructure;
using SFA.DAS.Aan.SharedUi.Models.NetworkEvents;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses;
using SFA.DAS.ApprenticeAan.Web.Controllers;
using SFA.DAS.ApprenticeAan.Web.UnitTests.TestHelpers;
using SFA.DAS.ApprenticePortal.Authentication.TestHelpers;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers;

public class NetworkEventsControllerTests
{
    private static readonly string AllNetworksUrl = Guid.NewGuid().ToString();

    [Test, MoqAutoData]
    public void GetCalendarEvents_ReturnsApiResponse(
        [Frozen] Mock<IOuterApiClient> outerApiMock,
        [Greedy] NetworkEventsController sut,
        GetCalendarEventsQueryResult expectedResult,
        string keyword,
        DateTime? fromDate,
        DateTime? toDate,
        Guid apprenticeId)
    {
        var eventFormats = new List<EventFormat>
        {
            EventFormat.InPerson,
            EventFormat.Online,
            EventFormat.Hybrid
        };
        var eventTypes = new List<int>();
        var regions = new List<int>();
        var fromDateFormatted = fromDate?.ToString("yyyy-MM-dd")!;
        var toDateFormatted = toDate?.ToString("yyyy-MM-dd")!;

        var request = new GetNetworkEventsRequest
        {
            Keyword = keyword,
            FromDate = fromDate,
            ToDate = toDate,
            EventFormat = eventFormats,
            CalendarId = eventTypes,
            RegionId = regions,
            Page = expectedResult.Page,
            PageSize = expectedResult.PageSize,
        };

        var user = AuthenticatedUsersForTesting.FakeLocalUserFullyVerifiedClaim(apprenticeId);
        outerApiMock.Setup(o => o.GetCalendarEvents(It.IsAny<Guid>(), It.IsAny<Dictionary<string, string[]>>(), It.IsAny<CancellationToken>())).ReturnsAsync(expectedResult);

        sut.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user.HttpContext!.User } };
        sut.AddUrlHelperMock().AddUrlForRoute(SharedRouteNames.NetworkEvents, AllNetworksUrl);

        //action
        var actualResult = sut.Index(request, new CancellationToken());

        var expectedEventFormatChecklistLookup = new ChecklistLookup[]
        {
            new(EventFormat.InPerson.GetDescription(), EventFormat.InPerson.ToString(),
                request.EventFormat.Exists(x => x == EventFormat.InPerson)),
            new(EventFormat.Online.GetDescription(), EventFormat.Online.ToString(),
                request.EventFormat.Exists(x => x == EventFormat.Online)),
            new(EventFormat.Hybrid.GetDescription(), EventFormat.Hybrid.ToString(),
                request.EventFormat.Exists(x => x == EventFormat.Hybrid))
        };

        var viewResult = actualResult.Result.As<ViewResult>();
        var model = viewResult.Model as NetworkEventsViewModel;
        model!.PaginationViewModel.CurrentPage.Should().Be(expectedResult.Page);
        model!.PaginationViewModel.PageSize.Should().Be(expectedResult.PageSize);
        model!.PaginationViewModel.TotalPages.Should().Be(expectedResult.TotalPages);
        model!.TotalCount.Should().Be(expectedResult.TotalCount);
        model.FilterChoices.FromDate?.ToApiString().Should().Be(fromDateFormatted);
        model.FilterChoices.ToDate?.ToApiString().Should().Be(toDateFormatted);
        model.FilterChoices.Keyword.Should().Be(keyword);
        model.FilterChoices.EventFormatChecklistDetails.Lookups.Should().BeEquivalentTo(expectedEventFormatChecklistLookup);
        model.SelectedFiltersModel.ClearSelectedFiltersLink.Should().Be(AllNetworksUrl); ;
        outerApiMock.Verify(o => o.GetCalendarEvents(It.IsAny<Guid>(), It.IsAny<Dictionary<string, string[]>>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test, MoqAutoData]
    public void GetCalendarEventsNoFilters_ReturnsApiResponse(
       [Frozen] Mock<IOuterApiClient> outerApiMock,
       [Greedy] NetworkEventsController sut,
       GetCalendarEventsQueryResult expectedResult,
       Guid apprenticeId)
    {
        var request = new GetNetworkEventsRequest();

        var user = AuthenticatedUsersForTesting.FakeLocalUserFullyVerifiedClaim(apprenticeId);
        outerApiMock.Setup(o => o.GetCalendarEvents(It.IsAny<Guid>(), It.IsAny<Dictionary<string, string[]>>(), It.IsAny<CancellationToken>())).ReturnsAsync(expectedResult);

        sut.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user.HttpContext!.User } };
        sut.AddUrlHelperMock().AddUrlForRoute(SharedRouteNames.NetworkEvents, AllNetworksUrl);

        var actualResult = sut.Index(request, new CancellationToken());
        var expectedEventFormatChecklistLookup = new ChecklistLookup[]
        {
            new(EventFormat.InPerson.GetDescription(), EventFormat.InPerson.ToString(),
                request.EventFormat.Exists(x => x == EventFormat.InPerson)),
            new(EventFormat.Online.GetDescription(), EventFormat.Online.ToString(),
                request.EventFormat.Exists(x => x == EventFormat.Online)),
            new(EventFormat.Hybrid.GetDescription(), EventFormat.Hybrid.ToString(),
                request.EventFormat.Exists(x => x == EventFormat.Hybrid))
        };

        var viewResult = actualResult.Result.As<ViewResult>();
        var model = viewResult.Model as NetworkEventsViewModel;
        model!.PaginationViewModel.CurrentPage.Should().Be(expectedResult.Page);
        model!.PaginationViewModel.PageSize.Should().Be(expectedResult.PageSize);
        model!.PaginationViewModel.TotalPages.Should().Be(expectedResult.TotalPages);
        model!.TotalCount.Should().Be(expectedResult.TotalCount);
        model.FilterChoices.FromDate.Should().BeNull();
        model.FilterChoices.ToDate.Should().BeNull();
        model.FilterChoices.Keyword.Should().BeNull();
        outerApiMock.Verify(o => o.GetCalendarEvents(It.IsAny<Guid>(), It.IsAny<Dictionary<string, string[]>>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test, MoqAutoData]
    public void GetCalendarEvents_RegionLookup_NationalAddedWithIdZero(
       [Frozen] Mock<IOuterApiClient> outerApiMock,
       [Greedy] NetworkEventsController sut,
       GetCalendarEventsQueryResult expectedResult,
       GetRegionsResult regionsResult,
       Guid apprenticeId)
    {
        var request = new GetNetworkEventsRequest();

        var regionCountFromApi = regionsResult.Regions.Count;
        var user = AuthenticatedUsersForTesting.FakeLocalUserFullyVerifiedClaim(apprenticeId);
        outerApiMock.Setup(o => o.GetCalendarEvents(It.IsAny<Guid>(), It.IsAny<Dictionary<string, string[]>>(), It.IsAny<CancellationToken>())).ReturnsAsync(expectedResult);
        outerApiMock.Setup(o => o.GetRegions()).ReturnsAsync(regionsResult);

        sut.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user.HttpContext!.User } };
        sut.AddUrlHelperMock().AddUrlForRoute(SharedRouteNames.NetworkEvents, AllNetworksUrl);

        var actualResult = sut.Index(request, new CancellationToken());

        var viewResult = actualResult.Result.As<ViewResult>();
        var model = viewResult.Model as NetworkEventsViewModel;

        var regionLookup = model!.FilterChoices.RegionChecklistDetails.Lookups;
        regionLookup!.Count().Should().Be(regionCountFromApi + 1);
        regionLookup!.First(x => x.Value == "0").Name.Should().Be("National");
    }
}
