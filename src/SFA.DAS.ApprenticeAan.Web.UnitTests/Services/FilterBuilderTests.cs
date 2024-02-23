using FluentAssertions;
using SFA.DAS.Aan.SharedUi.Constants;
using SFA.DAS.Aan.SharedUi.Extensions;
using SFA.DAS.Aan.SharedUi.Models.NetworkDirectory;
using SFA.DAS.Aan.SharedUi.Models.NetworkEvents;
using SFA.DAS.Aan.SharedUi.Services;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Services;

[TestFixture]
public class FilterBuilderTests
{
    private const string LocationUrl = "network-events";
    private const string NetworkDirectoryLocationUrl = "network-directory";

    [Test]
    public void BuildFilterChoicesForNoFilters()
    {
        var request = new GetNetworkEventsRequest
        {
            FromDate = null,
            ToDate = null
        };

        var actual = FilterBuilder.Build(request, () => { return LocationUrl; }, new List<ChecklistLookup>(), new List<ChecklistLookup>(), new List<ChecklistLookup>());
        actual.Count.Should().Be(0);
    }

    [TestCase(null, "", 0)]
    [TestCase("event", "Network events", 1)]
    public void BuildEventSearchFiltersForKeyword(string? keyword, string fieldName1, int expectedNumberOfFilters)
    {
        var request = new GetNetworkEventsRequest
        {
            Keyword = keyword
        };

        var actual = FilterBuilder.Build(request, () => { return LocationUrl; }, new List<ChecklistLookup>(), new List<ChecklistLookup>(), new List<ChecklistLookup>());
        actual.Count.Should().Be(expectedNumberOfFilters);
        if (expectedNumberOfFilters > 0)
        {
            var firstItem = actual.First();
            firstItem.FieldName.Should().Be(fieldName1);
            firstItem.FieldOrder.Should().Be(1);
            firstItem.Filters.First().ClearFilterLink.Should().Be(LocationUrl);
            firstItem.Filters.First().Order.Should().Be(1);
            if (fieldName1 != "")
            {
                firstItem.Filters.First().Value.Should().Be(keyword);
            }
        }
    }

    [TestCase(null, "", 0)]
    [TestCase("2023-05-31", "From date", 1)]
    public void BuildEventSearchFiltersForFromDate(DateTime? fromDate, string fieldName1, int expectedNumberOfFilters)
    {
        var request = new GetNetworkEventsRequest
        {
            FromDate = fromDate
        };

        var actual = FilterBuilder.Build(request, () => { return LocationUrl; }, new List<ChecklistLookup>(), new List<ChecklistLookup>(), new List<ChecklistLookup>());
        actual.Count.Should().Be(expectedNumberOfFilters);
        if (expectedNumberOfFilters > 0)
        {
            var firstItem = actual.First();
            firstItem.FieldName.Should().Be(fieldName1);
            firstItem.FieldOrder.Should().Be(1);
            firstItem.Filters.First().ClearFilterLink.Should().Be(LocationUrl);
            firstItem.Filters.First().Order.Should().Be(1);
            if (fieldName1 != "")
            {
                firstItem.Filters.First().Value.Should().Be(fromDate?.ToString("dd/MM/yyyy"));
            }
        }
    }

    [TestCase(null, "", 0)]
    [TestCase("2024-01-01", "To date", 1)]
    public void BuildEventSearchFiltersForToDate(DateTime? toDate, string fieldName1, int expectedNumberOfFilters)
    {
        var request = new GetNetworkEventsRequest
        {
            ToDate = toDate
        };

        var actual = FilterBuilder.Build(request, () => { return LocationUrl; }, new List<ChecklistLookup>(), new List<ChecklistLookup>(), new List<ChecklistLookup>());

        actual.Count.Should().Be(expectedNumberOfFilters);
        if (expectedNumberOfFilters > 0)
        {
            var firstItem = actual.First();
            firstItem.FieldName.Should().Be(fieldName1);
            firstItem.FieldOrder.Should().Be(1);
            firstItem.Filters.First().ClearFilterLink.Should().Be(LocationUrl);
            firstItem.Filters.First().Order.Should().Be(1);
            if (fieldName1 != "")
            {
                firstItem.Filters.First().Value.Should().Be(toDate?.ToString("dd/MM/yyyy"));
            }
        }
    }

    [TestCase("2023-05-31", "2024-01-01", "?toDate=2024-01-01", "?fromDate=2023-05-31", "From date", "To date")]
    public void BuildEventSearchFiltersForFromDateAndToDate(DateTime? fromDate, DateTime? toDate, string expectedFirst, string expectedSecond, string fieldName1, string fieldName2)
    {
        var request = new GetNetworkEventsRequest
        {
            FromDate = fromDate,
            ToDate = toDate
        };

        var actual = FilterBuilder.Build(request, () => { return LocationUrl; }, new List<ChecklistLookup>(), new List<ChecklistLookup>(), new List<ChecklistLookup>());

        actual.Count.Should().Be(2);

        var firstItem = actual.First();
        firstItem.FieldName.Should().Be(fieldName1);
        firstItem.FieldOrder.Should().Be(1);
        firstItem.Filters.First().ClearFilterLink.Should().Be(LocationUrl + expectedFirst);
        firstItem.Filters.First().Order.Should().Be(1);
        if (fieldName1 == "From date")
        {
            firstItem.Filters.First().Value.Should().Be(fromDate?.ToString("dd/MM/yyyy"));
        }
        else if (fieldName1 == "To date")
        {
            firstItem.Filters.First().Value.Should().Be(toDate?.ToString("dd/MM/yyyy"));
        }

        var secondItem = actual.Skip(1).First();

        secondItem.FieldName.Should().Be(fieldName2);
        secondItem.FieldOrder.Should().Be(2);
        secondItem.Filters.First().ClearFilterLink.Should().Be(LocationUrl + expectedSecond);
        secondItem.Filters.First().Order.Should().Be(1);
        if (fieldName2 == "To date")
        {
            secondItem.Filters.First().Value.Should().Be(toDate?.ToString("dd/MM/yyyy"));
        }
    }

    [TestCase(null, null, null, "", 0, null)]
    [TestCase(EventFormat.InPerson, null, null, "Event format", 1, "In person")]
    [TestCase(null, EventFormat.Online, null, "Event format", 1, "Online")]
    [TestCase(null, null, EventFormat.Hybrid, "Event format", 1, "Hybrid")]
    public void BuildEventSearchFiltersForEventFormatsSingleChoice(EventFormat? inPerson, EventFormat? online, EventFormat? hybrid,
         string? fieldName,
        int expectedNumberOfFilters, string? firstValue)
    {
        var request = new GetNetworkEventsRequest
        {
            EventFormat = [],
        };

        if (inPerson != null)
            request.EventFormat.Add(inPerson.Value);
        if (online != null)
            request.EventFormat.Add(online.Value);
        if (hybrid != null)
            request.EventFormat.Add(hybrid.Value);

        var actual = FilterBuilder.Build(request, () => { return LocationUrl; }, ChecklistLookupEventFormats(), new List<ChecklistLookup>(), new List<ChecklistLookup>());

        if (expectedNumberOfFilters == 0)
        {
            actual.Count.Should().Be(0);
            return;
        }

        var firstItem = actual.First();
        firstItem.Filters.Count.Should().Be(expectedNumberOfFilters);
        firstItem.FieldName.Should().Be(fieldName);
        firstItem.FieldOrder.Should().Be(1);
        if (firstItem.Filters.Count > 0)
        {
            var filter = firstItem.Filters.First();
            filter.ClearFilterLink.Should().Be(LocationUrl);
            filter.Order.Should().Be(1);
            filter.Value.Should().Be(firstValue);
        }
    }

    [TestCase(EventFormat.InPerson, EventFormat.Online, "?eventFormat=Online", "?eventFormat=InPerson", "In person", "Online")]
    [TestCase(EventFormat.InPerson, EventFormat.Hybrid, "?eventFormat=Hybrid", "?eventFormat=InPerson", "In person", "Hybrid")]
    [TestCase(EventFormat.Online, EventFormat.Hybrid, "?eventFormat=Hybrid", "?eventFormat=Online", "Online", "Hybrid")]
    public void BuildEventSearchFiltersForTwoEventFormats(EventFormat eventFormat1, EventFormat eventFormat2, string expectedFirst, string expectedSecond,
      string firstValue, string secondValue)
    {
        var request = new GetNetworkEventsRequest
        {
            EventFormat =
            [
                eventFormat1,
                eventFormat2
            ]
        };

        var actual = FilterBuilder.Build(request, () => { return LocationUrl; }, ChecklistLookupEventFormats(), new List<ChecklistLookup>(), new List<ChecklistLookup>());

        var firstItem = actual.First();
        firstItem.Filters.Count.Should().Be(2);
        firstItem.FieldName.Should().Be("Event format");
        firstItem.FieldOrder.Should().Be(1);

        var filter = firstItem.Filters.First();
        filter.ClearFilterLink.Should().Be(LocationUrl + expectedFirst);
        filter.Order.Should().Be(1);
        filter.Value.Should().Be(firstValue);

        var filterSecond = firstItem.Filters.Skip(1).First();
        filterSecond.ClearFilterLink.Should().Be(LocationUrl + expectedSecond);
        filterSecond.Order.Should().Be(2);
        filterSecond.Value.Should().Be(secondValue);
    }

    [TestCase("?eventFormat=Online&eventFormat=Hybrid", "?eventFormat=InPerson&eventFormat=Hybrid", "?eventFormat=InPerson&eventFormat=Online", "In person", "Online", "Hybrid")]
    public void BuildEventSearchFiltersForThreeEventFormats(string expectedFirst, string expectedSecond, string expectedThird, string firstValue, string secondValue, string thirdValue)
    {
        var request = new GetNetworkEventsRequest
        {
            EventFormat =
            [
                EventFormat.InPerson,
                EventFormat.Online,
                EventFormat.Hybrid
            ]
        };

        var actual = FilterBuilder.Build(request, () => { return LocationUrl; }, ChecklistLookupEventFormats(), new List<ChecklistLookup>(), new List<ChecklistLookup>());

        var firstItem = actual.First();
        firstItem.Filters.Count.Should().Be(3);
        firstItem.FieldName.Should().Be("Event format");
        firstItem.FieldOrder.Should().Be(1);

        var filterFirst = firstItem.Filters.First();
        filterFirst.ClearFilterLink.Should().Be(LocationUrl + expectedFirst);
        filterFirst.Order.Should().Be(1);
        filterFirst.Value.Should().Be(firstValue);

        var filterSecond = firstItem.Filters.Skip(1).First();
        filterSecond.ClearFilterLink.Should().Be(LocationUrl + expectedSecond);
        filterSecond.Order.Should().Be(2);
        filterSecond.Value.Should().Be(secondValue);

        var filterThird = firstItem.Filters.Skip(2).First();
        filterThird.ClearFilterLink.Should().Be(LocationUrl + expectedThird);
        filterThird.Order.Should().Be(3);
        filterThird.Value.Should().Be(thirdValue);
    }

    [TestCase(null, "", 0)]
    [TestCase(1, "Event type", 1)]
    [TestCase(2, "Event type", 1)]
    [TestCase(3, "Event type", 1)]
    public void BuildEventSearchFiltersForEventTypes(int? calendarId, string fieldName, int expectedNumberOfFilters)
    {
        var parameterName = "calendarId";
        var request = new GetNetworkEventsRequest { CalendarId = [] };
        var eventTypesLookup = new List<ChecklistLookup>();

        var eventFilters = new GetNetworkEventsRequest
        {
            CalendarId = []
        };

        if (calendarId != null)
        {
            eventTypesLookup.Add(new ChecklistLookup(parameterName, calendarId.Value.ToString()));
            eventFilters.CalendarId.Add(calendarId.Value);
            request.CalendarId.Add(calendarId.Value);
        }

        var actual = FilterBuilder.Build(request, () => { return LocationUrl; }, new List<ChecklistLookup>(), eventTypesLookup, new List<ChecklistLookup>());

        if (expectedNumberOfFilters == 0)
        {
            actual.Count.Should().Be(0);
            return;
        }

        var firstItem = actual.First();
        firstItem.Filters.Count.Should().Be(expectedNumberOfFilters);
        firstItem.FieldName.Should().Be(fieldName);
        firstItem.FieldOrder.Should().Be(1);

        var filter = firstItem.Filters.First();
        filter.ClearFilterLink.Should().Be(LocationUrl);
        filter.Order.Should().Be(1);
        filter.Value.Should().Be(parameterName);
    }

    [TestCase(1, 2, "?calendarId=2", "?calendarId=1")]
    [TestCase(1, 3, "?calendarId=3", "?calendarId=1")]
    [TestCase(2, 3, "?calendarId=3", "?calendarId=2")]
    public void BuildEventSearchFiltersForTwoEventTypes(int calendarId1, int calendarId2,
      string expectedFirst, string expectedSecond)
    {
        var parameterName = "calendarId";
        var request = new GetNetworkEventsRequest { CalendarId = [] };
        var eventTypesLookup = new List<ChecklistLookup>();

        var eventFilters = new GetNetworkEventsRequest
        {
            CalendarId = []
        };

        eventTypesLookup.Add(new ChecklistLookup(parameterName, calendarId1.ToString()));
        eventFilters.CalendarId.Add(calendarId1);
        request.CalendarId.Add(calendarId1);

        eventTypesLookup.Add(new ChecklistLookup(parameterName, calendarId2.ToString()));
        eventFilters.CalendarId.Add(calendarId2);
        request.CalendarId.Add(calendarId2);

        var actual = FilterBuilder.Build(request, () => { return LocationUrl; }, new List<ChecklistLookup>(), eventTypesLookup, new List<ChecklistLookup>());

        var firstItem = actual.First();
        firstItem.Filters.Count.Should().Be(2);
        firstItem.FieldName.Should().Be("Event type");
        firstItem.FieldOrder.Should().Be(1);

        var filter = firstItem.Filters.First();
        filter.ClearFilterLink.Should().Be(LocationUrl + expectedFirst);
        filter.Order.Should().Be(1);
        filter.Value.Should().Be(parameterName);

        var filterSecond = firstItem.Filters.Skip(1).First();
        filterSecond.ClearFilterLink.Should().Be(LocationUrl + expectedSecond);
        filterSecond.Order.Should().Be(2);
        filterSecond.Value.Should().Be(parameterName);
    }

    [TestCase(1, 2, 3, "?calendarId=2&calendarId=3", "?calendarId=1&calendarId=3", "?calendarId=1&calendarId=2")]
    public void BuildEventSearchFiltersForThreeEventTypes(int calendarId1, int calendarId2, int calendarId3, string expectedFirst, string expectedSecond, string expectedThird)
    {
        var parameterName = "calendarId";
        var request = new GetNetworkEventsRequest { CalendarId = [] };
        var eventTypesLookup = new List<ChecklistLookup>();

        var eventFilters = new GetNetworkEventsRequest
        {
            CalendarId = []
        };

        eventTypesLookup.Add(new ChecklistLookup(parameterName, calendarId1.ToString()));
        eventFilters.CalendarId.Add(calendarId1);
        request.CalendarId.Add(calendarId1);

        eventTypesLookup.Add(new ChecklistLookup(parameterName, calendarId2.ToString()));
        eventFilters.CalendarId.Add(calendarId2);
        request.CalendarId.Add(calendarId2);

        eventTypesLookup.Add(new ChecklistLookup(parameterName, calendarId3.ToString()));
        eventFilters.CalendarId.Add(calendarId3);
        request.CalendarId.Add(calendarId3);

        var actual = FilterBuilder.Build(request, () => { return LocationUrl; }, new List<ChecklistLookup>(), eventTypesLookup, new List<ChecklistLookup>());

        var firstItem = actual.First();
        firstItem.Filters.Count.Should().Be(3);
        firstItem.FieldName.Should().Be("Event type");
        firstItem.FieldOrder.Should().Be(1);

        var filter = firstItem.Filters.First();
        filter.ClearFilterLink.Should().Be(LocationUrl + expectedFirst);
        filter.Order.Should().Be(1);
        filter.Value.Should().Be(parameterName);

        var filterSecond = firstItem.Filters.Skip(1).First();
        filterSecond.ClearFilterLink.Should().Be(LocationUrl + expectedSecond);
        filterSecond.Order.Should().Be(2);
        filterSecond.Value.Should().Be(parameterName);

        var filterThird = firstItem.Filters.Skip(2).First();
        filterThird.ClearFilterLink.Should().Be(LocationUrl + expectedThird);
        filterThird.Order.Should().Be(3);
        filterThird.Value.Should().Be(parameterName);
    }

    [TestCase(null, "", 0)]
    [TestCase(1, "Region", 1)]
    [TestCase(2, "Region", 1)]
    [TestCase(3, "Region", 1)]
    public void BuildEventSearchFiltersForSingleRegions(int? regionId1, string fieldName, int expectedNumberOfFilters)
    {
        var parameterName = "regionId";
        var request = new GetNetworkEventsRequest { RegionId = [] };
        var regionLookups = new List<ChecklistLookup>();

        var eventFilters = new GetNetworkEventsRequest
        {
            RegionId = []
        };

        if (regionId1 != null)
        {
            var lookup = new ChecklistLookup(parameterName, regionId1.Value.ToString(), true);

            regionLookups.Add(lookup);

            eventFilters.RegionId.Add(regionId1.Value);
            request.RegionId.Add(regionId1.Value);
        }

        var actual = FilterBuilder.Build(request, () => { return LocationUrl; }, new List<ChecklistLookup>(), new List<ChecklistLookup>(), regionLookups);

        if (expectedNumberOfFilters == 0)
        {
            actual.Count.Should().Be(0);
            return;
        }

        var firstItem = actual.First();
        firstItem.Filters.Count.Should().Be(expectedNumberOfFilters);
        firstItem.FieldName.Should().Be(fieldName);
        firstItem.FieldOrder.Should().Be(1);

        var filter = firstItem.Filters.First();
        filter.ClearFilterLink.Should().Be(LocationUrl);
        filter.Order.Should().Be(1);
        filter.Value.Should().Be(parameterName);
    }

    [TestCase(1, 2, "?regionId=2", "?regionId=1")]
    [TestCase(1, 3, "?regionId=3", "?regionId=1")]
    [TestCase(2, 3, "?regionId=3", "?regionId=2")]
    public void BuildEventSearchFiltersForTwoRegions(int regionId1, int regionId2,
       string expectedFirst, string expectedSecond)
    {
        var parameterName = "regionId";
        var request
            = new GetNetworkEventsRequest { RegionId = [] };
        var regionLookups = new List<ChecklistLookup>();

        var eventFilters = new GetNetworkEventsRequest
        {
            RegionId = []
        };

        var lookup = new ChecklistLookup(parameterName, regionId1.ToString())
        {
            Checked = "Checked"
        };

        regionLookups.Add(lookup);

        request.RegionId.Add(regionId1);
        request.RegionId.Add(regionId2);
        eventFilters.RegionId.Add(regionId1);
        eventFilters.RegionId.Add(regionId2);

        regionLookups.Add(new ChecklistLookup(parameterName, regionId2.ToString()));

        var actual = FilterBuilder.Build(request, () => { return LocationUrl; }, new List<ChecklistLookup>(), new List<ChecklistLookup>(), regionLookups);

        var firstItem = actual.First();
        firstItem.Filters.Count.Should().Be(2);
        firstItem.FieldName.Should().Be("Region");
        firstItem.FieldOrder.Should().Be(1);

        var filter = firstItem.Filters.First();
        filter.ClearFilterLink.Should().Be(LocationUrl + expectedFirst);
        filter.Order.Should().Be(1);
        filter.Value.Should().Be(parameterName);

        var filterSecond = firstItem.Filters.Skip(1).First();
        filterSecond.ClearFilterLink.Should().Be(LocationUrl + expectedSecond);
        filterSecond.Order.Should().Be(2);
        filterSecond.Value.Should().Be(parameterName);
    }

    [TestCase("?regionId=2&regionId=3", "?regionId=1&regionId=3", "?regionId=1&regionId=2", 3, "checked")]
    [TestCase("?regionId=3", "?regionId=2", "", 2, "")]
    public void BuildEventSearchFiltersForThreeRegionsCheckedAndUnchecked(
       string expectedFirst, string expectedSecond, string expectedThird, int expectedNumberOfFilters, string regionId1Checked)
    {
        var parameterName = "regionId";
        var request = new GetNetworkEventsRequest { RegionId = [] };
        var regionLookups = new List<ChecklistLookup>();

        var eventFilters = new GetNetworkEventsRequest
        {
            RegionId = []
        };


        var lookup = new ChecklistLookup(parameterName, 1.ToString())
        {
            Checked = regionId1Checked
        };

        regionLookups.Add(lookup);

        eventFilters.RegionId.Add(1);

        if (regionId1Checked == "checked")
        {
            request.RegionId.Add(1);
        }

        regionLookups.Add(new ChecklistLookup(parameterName, 2.ToString()));
        eventFilters.RegionId.Add(2);
        request.RegionId.Add(2);

        regionLookups.Add(new ChecklistLookup(parameterName, 3.ToString()));
        eventFilters.RegionId.Add(3);
        request.RegionId.Add(3);

        var actual = FilterBuilder.Build(request, () => { return LocationUrl; }, new List<ChecklistLookup>(), new List<ChecklistLookup>(), regionLookups);

        var firstItem = actual.First();
        firstItem.Filters.Count.Should().Be(expectedNumberOfFilters);
        firstItem.FieldName.Should().Be("Region");
        firstItem.FieldOrder.Should().Be(1);

        var filter = firstItem.Filters.First();
        filter.ClearFilterLink.Should().Be(LocationUrl + expectedFirst);
        filter.Order.Should().Be(1);
        filter.Value.Should().Be(parameterName);

        var filterSecond = firstItem.Filters.Skip(1).First();
        filterSecond.ClearFilterLink.Should().Be(LocationUrl + expectedSecond);
        filterSecond.Order.Should().Be(2);
        filterSecond.Value.Should().Be(parameterName);

        if (firstItem.Filters.Count > 2)
        {
            var filterThird = firstItem.Filters.Skip(2).First();
            filterThird.ClearFilterLink.Should().Be(LocationUrl + expectedThird);
            filterThird.Order.Should().Be(3);
            filterThird.Value.Should().Be(parameterName);
        }
    }


    [TestCase(null, null, null, null, null, null, null, null, LocationUrl)]
    [TestCase("2023-07-12", null, null, null, null, null, null, null, LocationUrl + "?fromDate=2023-07-12")]
    [TestCase(null, "2024-07-13", null, null, null, null, null, null, LocationUrl + "?toDate=2024-07-13")]
    [TestCase("2023-07-12", "2024-07-13", null, null, null, null, null, null, LocationUrl + "?fromDate=2023-07-12&toDate=2024-07-13")]
    [TestCase(null, null, EventFormat.Hybrid, null, null, null, null, null, LocationUrl + "?eventFormat=Hybrid")]
    [TestCase(null, null, EventFormat.InPerson, EventFormat.Online, null, null, null, null, LocationUrl + "?eventFormat=InPerson&eventFormat=Online")]
    [TestCase(null, null, null, null, 1, null, null, null, LocationUrl + "?calendarId=1")]
    [TestCase(null, null, null, null, 1, 2, null, null, LocationUrl + "?calendarId=1&calendarId=2")]
    [TestCase(null, null, null, null, null, null, 1, null, LocationUrl + "?regionId=1")]
    [TestCase(null, null, null, null, null, null, 1, 2, LocationUrl + "?regionId=1&regionId=2")]
    [TestCase("2023-07-12", "2024-07-13", EventFormat.Hybrid, EventFormat.InPerson, 1, 2, 3, 4, LocationUrl + "?fromDate=2023-07-12&toDate=2024-07-13&eventFormat=Hybrid&eventFormat=InPerson&calendarId=1&calendarId=2&regionId=3&regionId=4")]
    public void BuildFullQueryStringForPagination(DateTime? fromDate, DateTime? toDate, EventFormat? eventFormat, EventFormat? eventFormat2, int? calendarId, int? calendarId2, int? regionId, int? regionId2, string expectedUrl)
    {
        var request = new GetNetworkEventsRequest
        {
            FromDate = fromDate ?? null,
            ToDate = toDate ?? null
        };

        if (eventFormat != null)
        {
            var eventFormats = new List<EventFormat> { eventFormat.Value };
            if (eventFormat2 != null) eventFormats.Add(eventFormat2.Value);
            request.EventFormat = eventFormats;
        }

        if (calendarId != null)
        {
            var calendarIds = new List<int> { calendarId.Value };
            if (calendarId2 != null) calendarIds.Add(calendarId2.Value);
            request.CalendarId = calendarIds;
        }

        if (regionId != null)
        {
            var regionIds = new List<int> { regionId.Value };
            if (regionId2 != null) regionIds.Add(regionId2.Value);
            request.RegionId = regionIds;
        }

        var actual = FilterBuilder.BuildFullQueryString(request, () => { return LocationUrl; });

        actual.Should().Be(expectedUrl);
    }

    private static List<ChecklistLookup> ChecklistLookupEventFormats() =>
        new()
        {
            new ChecklistLookup(EventFormat.InPerson.GetDescription(), EventFormat.InPerson.ToString()),
            new ChecklistLookup(EventFormat.Online.GetDescription(), EventFormat.Online.ToString()),
            new (EventFormat.Hybrid.GetDescription(), EventFormat.Hybrid.ToString())
        };

    [Test]
    public void Build_NetworkDirectorySearchFilterForNoFilters_CountIsZero()
    {
        var request = new NetworkDirectoryRequestModel { };


        var sut = FilterBuilder.Build(request, () => { return NetworkDirectoryLocationUrl; }, new List<ChecklistLookup>(), new List<ChecklistLookup>());
        sut.Count.Should().Be(0);
    }

    [TestCase(null, 0)]
    [TestCase("directory", 1)]
    public void Build_SearchFiltersForKeyword_ValueAndExpectedNumberOfFilterAreEqualWithkeywordAndexpectedNumberOfFilters(string? keyword, int expectedNumberOfFilters)
    {
        var request = new NetworkDirectoryRequestModel
        {
            Keyword = keyword
        };

        var sut = FilterBuilder.Build(request, () => { return NetworkDirectoryLocationUrl; }, new List<ChecklistLookup>(), new List<ChecklistLookup>());
        sut.Count.Should().Be(expectedNumberOfFilters);
        if (expectedNumberOfFilters > 0)
        {
            var firstItem = sut.First();
            firstItem.Filters.First().Value.Should().Be(keyword);
        }
    }

    [TestCase(null)]
    [TestCase("directory")]
    public void Build_SearchFiltersForKeyword_FieldAndFilterOrderAreOne(string? keyword)
    {
        var request = new NetworkDirectoryRequestModel
        {
            Keyword = keyword
        };

        var sut = FilterBuilder.Build(request, () => { return NetworkDirectoryLocationUrl; }, new List<ChecklistLookup>(), new List<ChecklistLookup>());

        if (sut.Count > 0)
        {
            var firstItem = sut.First();
            firstItem.FieldOrder.Should().Be(1);
            firstItem.Filters.First().Order.Should().Be(1);
        }
    }

    [TestCase(null, "")]
    [TestCase("directory", "Network directory")]
    public void Build_SearchFiltersForKeyword_FieldNameAndClearFilterLinkAreEqualWithkeywordFieldNameAndNetworkDirectoryLocationUrl(string? keyword, string keywordFieldName)
    {
        var request = new NetworkDirectoryRequestModel
        {
            Keyword = keyword
        };

        var sut = FilterBuilder.Build(request, () => { return NetworkDirectoryLocationUrl; }, new List<ChecklistLookup>(), new List<ChecklistLookup>());

        if (sut.Count > 0)
        {
            var firstItem = sut.First();
            firstItem.FieldName.Should().Be(keywordFieldName);
            firstItem.Filters.First().ClearFilterLink.Should().Be(NetworkDirectoryLocationUrl);
        }
    }

    [TestCase(Role.Apprentice, Role.Employer)]
    [TestCase(Role.Employer, Role.Apprentice)]
    [TestCase(Role.RegionalChair, Role.Employer)]
    [TestCase(Role.RegionalChair, Role.Apprentice)]
    public void Build_SearchFiltersForUserRoleType_FieldAndFilterOrderAreEqual(Role role1, Role role2)
    {
        var request = new NetworkDirectoryRequestModel
        {
            UserRole =
            [
                role1,
                role2
            ]
        };

        var sut = FilterBuilder.Build(request, () => { return NetworkDirectoryLocationUrl; }, new List<ChecklistLookup>(), new List<ChecklistLookup>());
        var firstItem = sut.First();
        firstItem.FieldOrder.Should().Be(1);

        var filter = firstItem.Filters.First();
        filter.Order.Should().Be(1);

        var secondFilter = firstItem.Filters.Skip(1).First();
        secondFilter.Order.Should().Be(2);
    }

    [TestCase(Role.Apprentice, Role.Employer, "?userRole=Employer", "?userRole=Apprentice")]
    [TestCase(Role.Employer, Role.Apprentice, "?userRole=Apprentice", "?userRole=Employer")]
    public void Build_SearchFiltersForUserRoleType_FieldNameAndClearFilterLinkAreEqual(Role role1, Role role2, string queryStringForRole1, string queryStringForRole2)
    {
        var request = new NetworkDirectoryRequestModel
        {
            UserRole =
            [
                role1,
                role2
            ]
        };

        var sut = FilterBuilder.Build(request, () => { return NetworkDirectoryLocationUrl; }, new List<ChecklistLookup>(), new List<ChecklistLookup>());
        var firstItem = sut.First();
        firstItem.FieldName.Should().Be("Role");

        var filter = firstItem.Filters.First();
        filter.ClearFilterLink.Should().Be(NetworkDirectoryLocationUrl + queryStringForRole1);

        var secondFilter = firstItem.Filters.Skip(1).First();
        secondFilter.ClearFilterLink.Should().Be(NetworkDirectoryLocationUrl + queryStringForRole2);
    }

    [TestCase(2, 3)]
    [TestCase(3, 2)]
    [TestCase(1, 5)]
    [TestCase(5, 1)]
    public void Build_SearchForRegions_FieldAndFilterOrderAreEqual(int? regionId1, int? regionId2)
    {
        var parameterName = "regionId";
        var request = new NetworkDirectoryRequestModel { RegionId = [] };
        var regionLookups = new List<ChecklistLookup>();

        var eventFilters = new NetworkDirectoryRequestModel
        {
            RegionId = []
        };

        if (regionId1 != null)
        {
            var lookup = new ChecklistLookup(parameterName, regionId1.Value.ToString(), true);

            regionLookups.Add(lookup);

            eventFilters.RegionId.Add(regionId1.Value);
            request.RegionId.Add(regionId1.Value);
        }

        if (regionId2 != null)
        {
            var lookup = new ChecklistLookup(parameterName, regionId2.Value.ToString(), true);

            regionLookups.Add(lookup);

            eventFilters.RegionId.Add(regionId2.Value);
            request.RegionId.Add(regionId2.Value);
        }

        var sut = FilterBuilder.Build(request, () => { return NetworkDirectoryLocationUrl; }, new List<ChecklistLookup>(), new List<ChecklistLookup>());
        var firstItem = sut.First();
        firstItem.FieldOrder.Should().Be(1);

        var filter = firstItem.Filters.First();
        filter.Order.Should().Be(1);

        var secondFilter = firstItem.Filters.Skip(1).First();
        secondFilter.Order.Should().Be(2);
    }

    [TestCase(3, "?regionId=2", 2, "?regionId=3")]
    [TestCase(2, "?regionId=3", 3, "?regionId=2")]
    public void Build_SearchForRegions_FieldNameAndClearFilterLinkAreEqual(int? regionId1, string queryString1, int? regionId2, string queryString2)
    {
        var parameterName = "regionId";
        var request = new NetworkDirectoryRequestModel { RegionId = [] };
        var regionLookups = new List<ChecklistLookup>();

        var eventFilters = new NetworkDirectoryRequestModel
        {
            RegionId = []
        };

        if (regionId1 != null)
        {
            var lookup = new ChecklistLookup(parameterName, regionId1.Value.ToString(), true);

            regionLookups.Add(lookup);

            eventFilters.RegionId.Add(regionId1.Value);
            request.RegionId.Add(regionId1.Value);
        }

        if (regionId2 != null)
        {
            var lookup = new ChecklistLookup(parameterName, regionId2.Value.ToString(), true);

            regionLookups.Add(lookup);

            eventFilters.RegionId.Add(regionId2.Value);
            request.RegionId.Add(regionId2.Value);
        }

        var sut = FilterBuilder.Build(request, () => { return NetworkDirectoryLocationUrl; }, new List<ChecklistLookup>(), new List<ChecklistLookup>());
        var firstItem = sut.First();
        firstItem.FieldName.Should().Be("Region");

        var filter = firstItem.Filters.First();
        filter.ClearFilterLink.Should().Be(NetworkDirectoryLocationUrl + queryString1);

        var secondFilter = firstItem.Filters.Skip(1).First();
        secondFilter.ClearFilterLink.Should().Be(NetworkDirectoryLocationUrl + queryString2);
    }

    [TestCase(3, 2)]
    [TestCase(2, 3)]
    public void Build_SearchForRegions_ValueAndFilterCountAreEqual(int? regionId1, int? regionId2)
    {
        var parameterName = "regionId";
        var request = new NetworkDirectoryRequestModel { RegionId = [] };
        var regionLookups = new List<ChecklistLookup>();

        var eventFilters = new NetworkDirectoryRequestModel
        {
            RegionId = []
        };

        if (regionId1 != null)
        {
            var lookup = new ChecklistLookup(parameterName, regionId1.Value.ToString(), true);

            regionLookups.Add(lookup);

            eventFilters.RegionId.Add(regionId1.Value);
            request.RegionId.Add(regionId1.Value);
        }

        if (regionId2 != null)
        {
            var lookup = new ChecklistLookup(parameterName, regionId2.Value.ToString(), true);

            regionLookups.Add(lookup);

            eventFilters.RegionId.Add(regionId2.Value);
            request.RegionId.Add(regionId2.Value);
        }

        var sut = FilterBuilder.Build(request, () => { return NetworkDirectoryLocationUrl; }, new List<ChecklistLookup>(), new List<ChecklistLookup>());

        var firstItem = sut.First();
        firstItem.Filters.Count.Should().Be(2);

        var filter = firstItem.Filters.First();
        filter.Value.Should().Be(regionId1.ToString());

        var secondFilter = firstItem.Filters.Skip(1).First();
        secondFilter.Value.Should().Be(regionId2.ToString());
    }

    [TestCase(3, "checked")]
    [TestCase(2, "")]
    public void Build_SearchForRegions_FilterValueAreEqual(int expectedNumberOfFilters, string regionId1Checked)
    {
        var parameterName = "regionId";
        var request = new NetworkDirectoryRequestModel { RegionId = [] };
        var regionLookups = new List<ChecklistLookup>();

        var eventFilters = new NetworkDirectoryRequestModel
        {
            RegionId = []
        };


        var lookup = new ChecklistLookup(parameterName, 1.ToString())
        {
            Checked = regionId1Checked
        };

        regionLookups.Add(lookup);

        eventFilters.RegionId.Add(1);

        if (regionId1Checked == "checked")
        {
            request.RegionId.Add(1);
        }

        regionLookups.Add(new ChecklistLookup(parameterName, 2.ToString()));
        eventFilters.RegionId.Add(2);
        request.RegionId.Add(2);

        regionLookups.Add(new ChecklistLookup(parameterName, 3.ToString()));
        eventFilters.RegionId.Add(3);
        request.RegionId.Add(3);

        var sut = FilterBuilder.Build(request, () => { return NetworkDirectoryLocationUrl; }, new List<ChecklistLookup>(), regionLookups);

        var firstItem = sut.First();
        firstItem.Filters.Count.Should().Be(expectedNumberOfFilters);

        var filter = firstItem.Filters.First();
        filter.Value.Should().Be(parameterName);

        var filterSecond = firstItem.Filters.Skip(1).First();
        filterSecond.Value.Should().Be(parameterName);

        if (firstItem.Filters.Count > 2)
        {
            var filterThird = firstItem.Filters.Skip(2).First();
            filterThird.Value.Should().Be(parameterName);
        }
    }

    [TestCase(null, null, null, null, NetworkDirectoryLocationUrl, null)]
    [TestCase(Role.Employer, null, null, null, NetworkDirectoryLocationUrl + "?userRole=Employer", null)]
    [TestCase(Role.RegionalChair, Role.Apprentice, null, null, NetworkDirectoryLocationUrl + "?userRole=RegionalChair&userRole=Apprentice", null)]
    [TestCase(null, null, 1, null, NetworkDirectoryLocationUrl + "?regionId=1", null)]
    [TestCase(null, null, 1, 2, NetworkDirectoryLocationUrl + "?regionId=1&regionId=2&status=New", MemberMaturityStatus.New)]
    [TestCase(Role.Employer, Role.RegionalChair, 3, 4, NetworkDirectoryLocationUrl + "?userRole=Employer&userRole=RegionalChair&regionId=3&regionId=4&status=Active", MemberMaturityStatus.Active)]
    public void Build_SearchFiltersWithAllInput_QueryStringAndExpectedUrlIsEqual(Role? role, Role? role2, int? regionId, int? regionId2, string expectedUrl, MemberMaturityStatus? memberMaturityStatus)
    {
        var request = new NetworkDirectoryRequestModel
        {
        };
        if (role != null)
        {
            var roles = new List<Role> { role.Value };
            if (role2 != null) roles.Add(role2.Value);
            request.UserRole = roles;
        }

        if (regionId != null)
        {
            var regionIds = new List<int> { regionId.Value };
            if (regionId2 != null) regionIds.Add(regionId2.Value);
            request.RegionId = regionIds;
        }

        if (memberMaturityStatus != null)
        {
            request.Status = [memberMaturityStatus.Value];
        }

        var sut = FilterBuilder.BuildFullQueryString(request, () => { return NetworkDirectoryLocationUrl; });

        sut.Should().Be(expectedUrl);
    }
}