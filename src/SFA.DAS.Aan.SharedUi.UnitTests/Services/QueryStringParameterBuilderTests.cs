using AutoFixture.NUnit3;
using FluentAssertions;
using SFA.DAS.Aan.SharedUi.Constants;
using SFA.DAS.Aan.SharedUi.Models.NetworkEvents;
using SFA.DAS.ApprenticeAan.Web.Services;

namespace SFA.DAS.Aan.SharedUi.UnitTests.Services;

public class QueryStringParameterBuilderTests
{

    [Test, AutoData]
    public void Builder_PopulatesDictionaryBuiltFromModel(DateTime? fromDate, DateTime? toDate,
        List<EventFormat> eventFormats, List<int> calendarIds, List<int> regionIds, int? page, int? pageSize)
    {
        var request = new GetNetworkEventsRequest
        {
            FromDate = fromDate,
            ToDate = toDate,
            EventFormat = eventFormats,
            CalendarId = calendarIds,
            RegionId = regionIds,
            Page = page,
            PageSize = pageSize
        };

        var parameters = QueryStringParameterBuilder.BuildQueryStringParameters(request);

        parameters.TryGetValue("fromDate", out string[]? fromDateResult);
        fromDateResult![0].Should().Be(fromDate?.ToString("yyyy-MM-dd"));

        parameters.TryGetValue("toDate", out var toDateResult);
        toDateResult![0].Should().Be(toDate?.ToString("yyyy-MM-dd"));

        parameters.TryGetValue("eventFormat", out var eventFormatResult);
        eventFormatResult!.Length.Should().Be(eventFormats.Count);
        eventFormats.Select(x => x.ToString()).Should().BeEquivalentTo(eventFormatResult.ToList());

        parameters.TryGetValue("calendarId", out var calendarIdsResult);
        calendarIdsResult!.Length.Should().Be(calendarIds.Count);
        calendarIds.Select(x => x.ToString()).Should().BeEquivalentTo(calendarIdsResult.ToList());

        parameters.TryGetValue("regionId", out var regionIdsResult);
        regionIdsResult!.Length.Should().Be(regionIds.Count);
        regionIds.Select(x => x.ToString()).Should().BeEquivalentTo(regionIdsResult.ToList());

        parameters.TryGetValue("page", out string[]? pageResult);
        pageResult![0].Should().Be(page?.ToString());

        parameters.TryGetValue("pageSize", out string[]? pageSizeResult);
        pageSizeResult![0].Should().Be(pageSize?.ToString());
    }

    [TestCase(null)]
    [TestCase("2030-06-01")]
    public void Builder_ConstructParameters_FromDate(DateTime? fromDate)
    {
        var parameters = QueryStringParameterBuilder.BuildQueryStringParameters(new GetNetworkEventsRequest
        {
            FromDate = fromDate
        });
        parameters.TryGetValue("fromDate", out var fromDateResult);
        if (fromDate != null)
        {
            fromDateResult![0].Should().Be(fromDate?.ToString("yyyy-MM-dd"));
        }
        else
        {
            fromDateResult.Should().BeNull();
        }
    }

    [TestCase(null)]
    [TestCase("2030-06-01")]
    public void Builder_ConstructParameters_ToDate(DateTime? toDate)
    {
        var parameters = QueryStringParameterBuilder.BuildQueryStringParameters(new GetNetworkEventsRequest
        {
            ToDate = toDate
        });
        parameters.TryGetValue("toDate", out var toDateResult);
        if (toDate != null)
        {
            toDateResult![0].Should().Be(toDate?.ToString("yyyy-MM-dd"));
        }
        else
        {
            toDateResult.Should().BeNull();
        }
    }

    [TestCase(null)]
    [TestCase(3)]
    public void Builder_ConstructParameters_ToPage(int? page)
    {
        var parameters = QueryStringParameterBuilder.BuildQueryStringParameters(new GetNetworkEventsRequest
        {
            Page = page
        });

        parameters.TryGetValue("page", out var pageResult);
        if (pageResult != null)
        {
            pageResult![0].Should().Be(page?.ToString());
        }
        else
        {
            pageResult.Should().BeNull();
        }
    }

    [TestCase(null)]
    [TestCase(6)]
    public void Builder_ConstructParameters_ToPageSize(int? pageSize)
    {
        var parameters = QueryStringParameterBuilder.BuildQueryStringParameters(new GetNetworkEventsRequest
        {
            PageSize = pageSize
        });

        parameters.TryGetValue("pageSize", out var pageResult);
        if (pageResult != null)
        {
            pageResult![0].Should().Be(pageSize?.ToString());
        }
        else
        {
            pageResult.Should().BeNull();
        }
    }


    [Test, AutoData]
    public void Builder_PopulatesParametersFromRequestWithNullPageAndPageSize(DateTime? fromDate, DateTime? toDate,
        List<EventFormat> eventFormats, List<int> calendarIds, List<int> regionIds)
    {
        var request = new GetNetworkEventsRequest
        {
            FromDate = fromDate,
            ToDate = toDate,
            EventFormat = eventFormats,
            CalendarId = calendarIds,
            RegionId = regionIds
        };

        var parameters = QueryStringParameterBuilder.BuildQueryStringParameters(request);

        parameters.TryGetValue("fromDate", out string[]? fromDateResult);
        fromDateResult![0].Should().Be(fromDate?.ToString("yyyy-MM-dd"));

        parameters.TryGetValue("toDate", out var toDateResult);
        toDateResult![0].Should().Be(toDate?.ToString("yyyy-MM-dd"));

        parameters.TryGetValue("eventFormat", out var eventFormatResult);
        eventFormatResult!.Length.Should().Be(eventFormats.Count);
        eventFormats.Select(x => x.ToString()).Should().BeEquivalentTo(eventFormatResult.ToList());

        parameters.TryGetValue("calendarId", out var calendarIdsResult);
        calendarIdsResult!.Length.Should().Be(calendarIds.Count);
        calendarIds.Select(x => x.ToString()).Should().BeEquivalentTo(calendarIdsResult.ToList());

        parameters.TryGetValue("regionId", out var regionIdsResult);
        regionIdsResult!.Length.Should().Be(regionIds.Count);
        regionIds.Select(x => x.ToString()).Should().BeEquivalentTo(regionIdsResult.ToList());

        parameters.ContainsKey("page").Should().BeFalse();
        parameters.ContainsKey("pageSize").Should().BeFalse();


    }
}