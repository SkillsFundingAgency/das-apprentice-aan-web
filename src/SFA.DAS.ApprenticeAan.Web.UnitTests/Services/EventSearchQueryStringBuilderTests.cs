using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using SFA.DAS.ApprenticeAan.Domain.Constants;
using SFA.DAS.ApprenticeAan.Domain.Extensions;
using SFA.DAS.ApprenticeAan.Domain.Models;
using SFA.DAS.ApprenticeAan.Web.Services;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Services;

[TestFixture]
public class EventSearchQueryStringBuilderTests
{
    [Test]
    public void BuildFilterChoicesForNoFilters()
    {
        var locationUrl = "network-events";
        var mockUrlHelper = new Mock<IUrlHelper>();
        mockUrlHelper
            .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
            .Returns(locationUrl);

        var service = new EventSearchQueryStringBuilder();


        var eventFilters = new EventFilterChoices
        {
            FromDate = null,
            ToDate = null
        };

        var actual = service.BuildEventSearchFilters(eventFilters, mockUrlHelper.Object);
        actual.Count.Should().Be(0);
    }

    [TestCase(null, null, "", "", "", "", 0, 1)]
    [TestCase("2023-05-31", null, "", "", "From date", "", 1, 1)]
    [TestCase(null, "2024-01-01", "", "", "To date", "", 1, 1)]
    [TestCase("2023-05-31", "2024-01-01", "?toDate=2024-01-01", "?fromDate=2023-05-31", "From date", "To date", 2, 1)]
    public void BuildEventSearchFiltersForFromDateAndToDate(DateTime? fromDate, DateTime? toDate, string expectedFirst, string expectedSecond, string fieldName1, string fieldName2, int expectedNumberOfFilters, int fieldOrder1)
    {
        var locationUrl = "network-events";
        var mockUrlHelper = new Mock<IUrlHelper>();
        mockUrlHelper
            .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
            .Returns(locationUrl);

        var service = new EventSearchQueryStringBuilder();

        var eventFilters = new EventFilterChoices
        {
            FromDate = fromDate,
            ToDate = toDate
        };

        var actual = service.BuildEventSearchFilters(eventFilters, mockUrlHelper.Object);
        actual.Count.Should().Be(expectedNumberOfFilters);
        if (expectedNumberOfFilters > 0)
        {
            var firstItem = actual.First();
            firstItem.FieldName.Should().Be(fieldName1);
            firstItem.FieldOrder.Should().Be(fieldOrder1);
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
        }

        if (expectedNumberOfFilters <= 1) return;

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

    [TestCase(null, null, null, "", "", "", "", 0, null, null, null)]
    [TestCase(EventFormat.InPerson, null, null, "", "", "", "Event format", 1, "In person", null, null)]
    [TestCase(null, EventFormat.Online, null, "", "", "", "Event format", 1, "Online", null, null)]
    [TestCase(null, null, EventFormat.Hybrid, "", "", "", "Event format", 1, "Hybrid", null, null)]
    [TestCase(EventFormat.InPerson, EventFormat.Online, null, "?eventFormat=Online", "?eventFormat=InPerson", "", "Event format", 2, "In person", "Online", null)]
    [TestCase(EventFormat.InPerson, null, EventFormat.Hybrid, "?eventFormat=Hybrid", "?eventFormat=InPerson", "", "Event format", 2, "In person", "Hybrid", null)]
    [TestCase(null, EventFormat.Online, EventFormat.Hybrid, "?eventFormat=Hybrid", "?eventFormat=Online", "", "Event format", 2, "Online", "Hybrid", null)]
    [TestCase(EventFormat.InPerson, EventFormat.Online, EventFormat.Hybrid, "?eventFormat=Online&eventFormat=Hybrid", "?eventFormat=InPerson&eventFormat=Hybrid", "?eventFormat=InPerson&eventFormat=Online", "Event format", 3, "In person", "Online", "Hybrid")]
    public void BuildEventSearchFiltersForEventFormats(EventFormat? inPerson, EventFormat? online, EventFormat? hybrid,
        string expectedFirst, string expectedSecond, string expectedThird, string fieldName,
        int expectedNumberOfFilters, string firstValue, string secondValue, string thirdValue)
    {
        var locationUrl = "network-events";
        var mockUrlHelper = new Mock<IUrlHelper>();
        mockUrlHelper
            .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
            .Returns(locationUrl);

        var service = new EventSearchQueryStringBuilder();

        var eventFilters = new EventFilterChoices
        {
            EventFormats = new List<EventFormat>(),
            EventFormatsLookup = new List<ChecklistLookup>
            {
                new(EventFormat.InPerson.GetDescription()!, EventFormat.InPerson.ToString()),
                new(EventFormat.Online.GetDescription()!, EventFormat.Online.ToString()),
                new(EventFormat.Hybrid.GetDescription()!, EventFormat.Hybrid.ToString())
            }
        };

        if (inPerson != null)
            eventFilters.EventFormats.Add(inPerson.Value);
        if (online != null)
            eventFilters.EventFormats.Add(online.Value);
        if (hybrid != null)
            eventFilters.EventFormats.Add(hybrid.Value);

        var actual = service.BuildEventSearchFilters(eventFilters, mockUrlHelper.Object);
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
            filter.ClearFilterLink.Should().Be(locationUrl + expectedFirst);
            filter.Order.Should().Be(1);
            filter.Value.Should().Be(firstValue);
        }

        if (firstItem.Filters.Count > 1)
        {
            var filter = firstItem.Filters.Skip(1).First();
            filter.ClearFilterLink.Should().Be(locationUrl + expectedSecond);
            filter.Order.Should().Be(2);
            filter.Value.Should().Be(secondValue);
        }

        if (firstItem.Filters.Count > 2)
        {
            var filter = firstItem.Filters.Skip(2).First();
            filter.ClearFilterLink.Should().Be(locationUrl + expectedThird);
            filter.Order.Should().Be(3);
            filter.Value.Should().Be(thirdValue);
        }
    }

    [TestCase(null, null, null, "", "", "", "", 0)]
    [TestCase(1, null, null, "", "", "", "Event type", 1)]
    [TestCase(null, 1, null, "", "", "", "Event type", 1)]
    [TestCase(null, null, 1, "", "", "", "Event type", 1)]
    [TestCase(1, 2, null, "?calendarId=2", "?calendarId=1", "", "Event type", 2)]
    [TestCase(1, null, 3, "?calendarId=3", "?calendarId=1", "", "Event type", 2)]
    [TestCase(null, 2, 3, "?calendarId=3", "?calendarId=2", "", "Event type", 2)]
    [TestCase(1, 2, 3, "?calendarId=2&calendarId=3", "?calendarId=1&calendarId=3", "?calendarId=1&calendarId=2", "Event type", 3)]
    public void BuildEventSearchFiltersForEventTypes(int? calendarId1, int? calendarId2, int? calendarId3,
        string expectedFirst, string expectedSecond, string expectedThird, string fieldName,
        int expectedNumberOfFilters)
    {
        var parameterName = "calendarId";
        var eventFilterChoices = new EventFilterChoices { CalendarIds = new List<int>() };
        var eventTypesLookup = new List<ChecklistLookup>();

        var eventFilters = new EventFilterChoices
        {
            CalendarIds = new List<int>()
        };

        if (calendarId1 != null)
        {
            eventTypesLookup.Add(new ChecklistLookup(parameterName, calendarId1.Value.ToString()));
            eventFilters.CalendarIds.Add(calendarId1.Value);
            eventFilterChoices.CalendarIds.Add(calendarId1.Value);
        }

        if (calendarId2 != null)
        {
            eventTypesLookup.Add(new ChecklistLookup(parameterName, calendarId2.Value.ToString()));
            eventFilters.CalendarIds.Add(calendarId2.Value);
            eventFilterChoices.CalendarIds.Add(calendarId2.Value);
        }

        if (calendarId3 != null)
        {
            eventTypesLookup.Add(new ChecklistLookup(parameterName, calendarId3.Value.ToString()));
            eventFilters.CalendarIds.Add(calendarId3.Value);
            eventFilterChoices.CalendarIds.Add(calendarId3.Value);
        }

        var locationUrl = "network-events";
        var mockUrlHelper = new Mock<IUrlHelper>();
        mockUrlHelper
            .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
            .Returns(locationUrl);

        var service = new EventSearchQueryStringBuilder();
        eventFilterChoices.EventTypesLookup = eventTypesLookup;
        ;
        var actual = service.BuildEventSearchFilters(eventFilterChoices, mockUrlHelper.Object);

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
            filter.ClearFilterLink.Should().Be(locationUrl + expectedFirst);
            filter.Order.Should().Be(1);
            filter.Value.Should().Be(parameterName);
        }

        if (firstItem.Filters.Count > 1)
        {
            var filter = firstItem.Filters.Skip(1).First();
            filter.ClearFilterLink.Should().Be(locationUrl + expectedSecond);
            filter.Order.Should().Be(2);
            filter.Value.Should().Be(parameterName);
        }

        if (firstItem.Filters.Count > 2)
        {
            var filter = firstItem.Filters.Skip(2).First();
            filter.ClearFilterLink.Should().Be(locationUrl + expectedThird);
            filter.Order.Should().Be(3);
            filter.Value.Should().Be(parameterName);
        }
    }
}
