﻿using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using SFA.DAS.ApprenticeAan.Domain.Constants;
using SFA.DAS.ApprenticeAan.Domain.Extensions;
using SFA.DAS.ApprenticeAan.Web.Models.NetworkEvents;
using SFA.DAS.ApprenticeAan.Web.Services;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Services;

[TestFixture]
public class FilterBuilderTests
{
    [Test]
    public void BuildFilterChoicesForNoFilters()
    {
        var locationUrl = "network-events";
        var mockUrlHelper = new Mock<IUrlHelper>();
        mockUrlHelper
            .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
            .Returns(locationUrl);

        var request = new GetNetworkEventsRequest
        {
            FromDate = null,
            ToDate = null
        };


        var actual = FilterBuilder.Build(request, mockUrlHelper.Object, new List<ChecklistLookup>(), new List<ChecklistLookup>(), new List<ChecklistLookup>());
        actual.Count.Should().Be(0);
    }

    [TestCase(null, "", 0)]
    [TestCase("2023-05-31", "From date", 1)]
    public void BuildEventSearchFiltersForFromDate(DateTime? fromDate, string fieldName1, int expectedNumberOfFilters)
    {
        var locationUrl = "network-events";
        var mockUrlHelper = new Mock<IUrlHelper>();
        mockUrlHelper
            .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
            .Returns(locationUrl);

        var request = new GetNetworkEventsRequest
        {
            FromDate = fromDate
        };

        var actual = FilterBuilder.Build(request, mockUrlHelper.Object, new List<ChecklistLookup>(), new List<ChecklistLookup>(), new List<ChecklistLookup>());
        actual.Count.Should().Be(expectedNumberOfFilters);
        if (expectedNumberOfFilters > 0)
        {
            var firstItem = actual.First();
            firstItem.FieldName.Should().Be(fieldName1);
            firstItem.FieldOrder.Should().Be(1);
            firstItem.Filters.First().ClearFilterLink.Should().Be(locationUrl);
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
        var locationUrl = "network-events";
        var mockUrlHelper = new Mock<IUrlHelper>();
        mockUrlHelper
            .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
            .Returns(locationUrl);

        var request = new GetNetworkEventsRequest
        {
            ToDate = toDate
        };

        var actual = FilterBuilder.Build(request, mockUrlHelper.Object, new List<ChecklistLookup>(), new List<ChecklistLookup>(), new List<ChecklistLookup>());

        actual.Count.Should().Be(expectedNumberOfFilters);
        if (expectedNumberOfFilters > 0)
        {
            var firstItem = actual.First();
            firstItem.FieldName.Should().Be(fieldName1);
            firstItem.FieldOrder.Should().Be(1);
            firstItem.Filters.First().ClearFilterLink.Should().Be(locationUrl);
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
        var locationUrl = "network-events";
        var mockUrlHelper = new Mock<IUrlHelper>();
        mockUrlHelper
            .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
            .Returns(locationUrl);


        var request = new GetNetworkEventsRequest
        {
            FromDate = fromDate,
            ToDate = toDate
        };

        var actual = FilterBuilder.Build(request, mockUrlHelper.Object, new List<ChecklistLookup>(), new List<ChecklistLookup>(), new List<ChecklistLookup>());

        actual.Count.Should().Be(2);

        var firstItem = actual.First();
        firstItem.FieldName.Should().Be(fieldName1);
        firstItem.FieldOrder.Should().Be(1);
        firstItem.Filters.First().ClearFilterLink.Should().Be(locationUrl + expectedFirst);
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
        secondItem.Filters.First().ClearFilterLink.Should().Be(locationUrl + expectedSecond);
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
         string fieldName,
        int expectedNumberOfFilters, string firstValue)
    {
        var locationUrl = "network-events";
        var mockUrlHelper = new Mock<IUrlHelper>();
        mockUrlHelper
            .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
            .Returns(locationUrl);

        var request = new GetNetworkEventsRequest
        {
            EventFormat = new List<EventFormat>(),
        };

        if (inPerson != null)
            request.EventFormat.Add(inPerson.Value);
        if (online != null)
            request.EventFormat.Add(online.Value);
        if (hybrid != null)
            request.EventFormat.Add(hybrid.Value);


        var actual = FilterBuilder.Build(request, mockUrlHelper.Object, ChecklistLookupEventFormats(), new List<ChecklistLookup>(), new List<ChecklistLookup>());

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
            filter.ClearFilterLink.Should().Be(locationUrl);
            filter.Order.Should().Be(1);
            filter.Value.Should().Be(firstValue);
        }
    }

    [TestCase(EventFormat.InPerson, EventFormat.Online, "?eventFormat=Online", "?eventFormat=InPerson", "In person", "Online")]
    [TestCase(EventFormat.InPerson, EventFormat.Hybrid, "?eventFormat=Hybrid", "?eventFormat=InPerson", "In person", "Hybrid")]
    [TestCase(EventFormat.Online, EventFormat.Hybrid, "?eventFormat=Hybrid", "?eventFormat=Online", "Online", "Hybrid")]
    public void BuildEventSearchFiltersForTwoEventFormats(EventFormat eventFormat1, EventFormat eventFormat2,
        string expectedFirst, string expectedSecond,
         string firstValue, string secondValue)
    {
        var locationUrl = "network-events";
        var mockUrlHelper = new Mock<IUrlHelper>();
        mockUrlHelper
            .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
            .Returns(locationUrl);

        var request = new GetNetworkEventsRequest
        {
            EventFormat = new List<EventFormat>
            {
                eventFormat1,
                eventFormat2
            }
        };

        var actual = FilterBuilder.Build(request, mockUrlHelper.Object, ChecklistLookupEventFormats(), new List<ChecklistLookup>(), new List<ChecklistLookup>());

        var firstItem = actual.First();
        firstItem.Filters.Count.Should().Be(2);
        firstItem.FieldName.Should().Be("Event format");
        firstItem.FieldOrder.Should().Be(1);

        var filter = firstItem.Filters.First();
        filter.ClearFilterLink.Should().Be(locationUrl + expectedFirst);
        filter.Order.Should().Be(1);
        filter.Value.Should().Be(firstValue);


        var filterSecond = firstItem.Filters.Skip(1).First();
        filterSecond.ClearFilterLink.Should().Be(locationUrl + expectedSecond);
        filterSecond.Order.Should().Be(2);
        filterSecond.Value.Should().Be(secondValue);
    }

    [TestCase("?eventFormat=Online&eventFormat=Hybrid", "?eventFormat=InPerson&eventFormat=Hybrid", "?eventFormat=InPerson&eventFormat=Online", "In person", "Online", "Hybrid")]
    public void BuildEventSearchFiltersForThreeEventFormats(string expectedFirst, string expectedSecond, string expectedThird, string firstValue, string secondValue, string thirdValue)
    {
        var locationUrl = "network-events";
        var mockUrlHelper = new Mock<IUrlHelper>();
        mockUrlHelper
            .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
            .Returns(locationUrl);

        var request = new GetNetworkEventsRequest
        {
            EventFormat = new List<EventFormat>
            {
                EventFormat.InPerson,
                EventFormat.Online,
                EventFormat.Hybrid
            }
        };

        var actual = FilterBuilder.Build(request, mockUrlHelper.Object, ChecklistLookupEventFormats(), new List<ChecklistLookup>(), new List<ChecklistLookup>());

        var firstItem = actual.First();
        firstItem.Filters.Count.Should().Be(3);
        firstItem.FieldName.Should().Be("Event format");
        firstItem.FieldOrder.Should().Be(1);

        var filterFirst = firstItem.Filters.First();
        filterFirst.ClearFilterLink.Should().Be(locationUrl + expectedFirst);
        filterFirst.Order.Should().Be(1);
        filterFirst.Value.Should().Be(firstValue);

        var filterSecond = firstItem.Filters.Skip(1).First();
        filterSecond.ClearFilterLink.Should().Be(locationUrl + expectedSecond);
        filterSecond.Order.Should().Be(2);
        filterSecond.Value.Should().Be(secondValue);

        var filterThird = firstItem.Filters.Skip(2).First();
        filterThird.ClearFilterLink.Should().Be(locationUrl + expectedThird);
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
        var request = new GetNetworkEventsRequest { CalendarId = new List<int>() };
        var eventTypesLookup = new List<ChecklistLookup>();

        var eventFilters = new GetNetworkEventsRequest
        {
            CalendarId = new List<int>()
        };

        if (calendarId != null)
        {
            eventTypesLookup.Add(new ChecklistLookup(parameterName, calendarId.Value.ToString()));
            eventFilters.CalendarId.Add(calendarId.Value);
            request.CalendarId.Add(calendarId.Value);
        }

        var locationUrl = "network-events";
        var mockUrlHelper = new Mock<IUrlHelper>();
        mockUrlHelper
            .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
            .Returns(locationUrl);

        var actual = FilterBuilder.Build(request, mockUrlHelper.Object, new List<ChecklistLookup>(), eventTypesLookup, new List<ChecklistLookup>());

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
        filter.ClearFilterLink.Should().Be(locationUrl);
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
        var request = new GetNetworkEventsRequest { CalendarId = new List<int>() };
        var eventTypesLookup = new List<ChecklistLookup>();

        var eventFilters = new GetNetworkEventsRequest
        {
            CalendarId = new List<int>()
        };

        eventTypesLookup.Add(new ChecklistLookup(parameterName, calendarId1.ToString()));
        eventFilters.CalendarId.Add(calendarId1);
        request.CalendarId.Add(calendarId1);

        eventTypesLookup.Add(new ChecklistLookup(parameterName, calendarId2.ToString()));
        eventFilters.CalendarId.Add(calendarId2);
        request.CalendarId.Add(calendarId2);

        var locationUrl = "network-events";
        var mockUrlHelper = new Mock<IUrlHelper>();
        mockUrlHelper
            .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
            .Returns(locationUrl);

        var actual = FilterBuilder.Build(request, mockUrlHelper.Object, new List<ChecklistLookup>(), eventTypesLookup, new List<ChecklistLookup>());

        var firstItem = actual.First();
        firstItem.Filters.Count.Should().Be(2);
        firstItem.FieldName.Should().Be("Event type");
        firstItem.FieldOrder.Should().Be(1);

        var filter = firstItem.Filters.First();
        filter.ClearFilterLink.Should().Be(locationUrl + expectedFirst);
        filter.Order.Should().Be(1);
        filter.Value.Should().Be(parameterName);


        var filterSecond = firstItem.Filters.Skip(1).First();
        filterSecond.ClearFilterLink.Should().Be(locationUrl + expectedSecond);
        filterSecond.Order.Should().Be(2);
        filterSecond.Value.Should().Be(parameterName);
    }

    [TestCase(1, 2, 3, "?calendarId=2&calendarId=3", "?calendarId=1&calendarId=3", "?calendarId=1&calendarId=2")]
    public void BuildEventSearchFiltersForThreeEventTypes(int calendarId1, int calendarId2, int calendarId3, string expectedFirst, string expectedSecond, string expectedThird)
    {
        var parameterName = "calendarId";
        var request = new GetNetworkEventsRequest { CalendarId = new List<int>() };
        var eventTypesLookup = new List<ChecklistLookup>();

        var eventFilters = new GetNetworkEventsRequest
        {
            CalendarId = new List<int>()
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

        var locationUrl = "network-events";
        var mockUrlHelper = new Mock<IUrlHelper>();
        mockUrlHelper
            .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
            .Returns(locationUrl);

        var actual = FilterBuilder.Build(request, mockUrlHelper.Object, new List<ChecklistLookup>(), eventTypesLookup, new List<ChecklistLookup>());

        var firstItem = actual.First();
        firstItem.Filters.Count.Should().Be(3);
        firstItem.FieldName.Should().Be("Event type");
        firstItem.FieldOrder.Should().Be(1);

        var filter = firstItem.Filters.First();
        filter.ClearFilterLink.Should().Be(locationUrl + expectedFirst);
        filter.Order.Should().Be(1);
        filter.Value.Should().Be(parameterName);

        var filterSecond = firstItem.Filters.Skip(1).First();
        filterSecond.ClearFilterLink.Should().Be(locationUrl + expectedSecond);
        filterSecond.Order.Should().Be(2);
        filterSecond.Value.Should().Be(parameterName);

        var filterThird = firstItem.Filters.Skip(2).First();
        filterThird.ClearFilterLink.Should().Be(locationUrl + expectedThird);
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
        var request = new GetNetworkEventsRequest { RegionId = new List<int>() };
        var regionLookups = new List<ChecklistLookup>();

        var eventFilters = new GetNetworkEventsRequest
        {
            RegionId = new List<int>()
        };

        if (regionId1 != null)
        {
            var lookup = new ChecklistLookup(parameterName, regionId1.Value.ToString(), true);

            regionLookups.Add(lookup);

            eventFilters.RegionId.Add(regionId1.Value);
            request.RegionId.Add(regionId1.Value);
        }

        var locationUrl = "network-events";
        var mockUrlHelper = new Mock<IUrlHelper>();
        mockUrlHelper
            .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
            .Returns(locationUrl);

        var actual = FilterBuilder.Build(request, mockUrlHelper.Object, new List<ChecklistLookup>(), new List<ChecklistLookup>(), regionLookups);

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
        filter.ClearFilterLink.Should().Be(locationUrl);
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
            = new GetNetworkEventsRequest { RegionId = new List<int>() };
        var regionLookups = new List<ChecklistLookup>();

        var eventFilters = new GetNetworkEventsRequest
        {
            RegionId = new List<int>()
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

        var locationUrl = "network-events";
        var mockUrlHelper = new Mock<IUrlHelper>();
        mockUrlHelper
            .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
            .Returns(locationUrl);

        var actual = FilterBuilder.Build(request, mockUrlHelper.Object, new List<ChecklistLookup>(), new List<ChecklistLookup>(), regionLookups);

        var firstItem = actual.First();
        firstItem.Filters.Count.Should().Be(2);
        firstItem.FieldName.Should().Be("Region");
        firstItem.FieldOrder.Should().Be(1);

        var filter = firstItem.Filters.First();
        filter.ClearFilterLink.Should().Be(locationUrl + expectedFirst);
        filter.Order.Should().Be(1);
        filter.Value.Should().Be(parameterName);

        var filterSecond = firstItem.Filters.Skip(1).First();
        filterSecond.ClearFilterLink.Should().Be(locationUrl + expectedSecond);
        filterSecond.Order.Should().Be(2);
        filterSecond.Value.Should().Be(parameterName);
    }

    [TestCase("?regionId=2&regionId=3", "?regionId=1&regionId=3", "?regionId=1&regionId=2", 3, "checked")]
    [TestCase("?regionId=3", "?regionId=2", "", 2, "")]
    public void BuildEventSearchFiltersForThreeRegionsCheckedAndUnchecked(
       string expectedFirst, string expectedSecond, string expectedThird, int expectedNumberOfFilters, string regionId1Checked)
    {
        var parameterName = "regionId";
        var request = new GetNetworkEventsRequest { RegionId = new List<int>() };
        var regionLookups = new List<ChecklistLookup>();

        var eventFilters = new GetNetworkEventsRequest
        {
            RegionId = new List<int>()
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


        var locationUrl = "network-events";
        var mockUrlHelper = new Mock<IUrlHelper>();
        mockUrlHelper
            .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
            .Returns(locationUrl);

        var actual = FilterBuilder.Build(request, mockUrlHelper.Object, new List<ChecklistLookup>(), new List<ChecklistLookup>(), regionLookups);

        var firstItem = actual.First();
        firstItem.Filters.Count.Should().Be(expectedNumberOfFilters);
        firstItem.FieldName.Should().Be("Region");
        firstItem.FieldOrder.Should().Be(1);

        var filter = firstItem.Filters.First();
        filter.ClearFilterLink.Should().Be(locationUrl + expectedFirst);
        filter.Order.Should().Be(1);
        filter.Value.Should().Be(parameterName);

        var filterSecond = firstItem.Filters.Skip(1).First();
        filterSecond.ClearFilterLink.Should().Be(locationUrl + expectedSecond);
        filterSecond.Order.Should().Be(2);
        filterSecond.Value.Should().Be(parameterName);

        if (firstItem.Filters.Count > 2)
        {
            var filterThird = firstItem.Filters.Skip(2).First();
            filterThird.ClearFilterLink.Should().Be(locationUrl + expectedThird);
            filterThird.Order.Should().Be(3);
            filterThird.Value.Should().Be(parameterName);
        }
    }


    private static List<ChecklistLookup> ChecklistLookupEventFormats() =>
        new()
        {
            new ChecklistLookup(EventFormat.InPerson.GetDescription()!, EventFormat.InPerson.ToString()),
            new ChecklistLookup(EventFormat.Online.GetDescription()!, EventFormat.Online.ToString()),
            new (EventFormat.Hybrid.GetDescription()!, EventFormat.Hybrid.ToString())
        };

}