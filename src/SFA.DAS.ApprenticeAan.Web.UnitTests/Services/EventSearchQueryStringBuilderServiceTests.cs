
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using SFA.DAS.ApprenticeAan.Domain.Models;
using SFA.DAS.ApprenticeAan.Web.Services;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Services;

[TestFixture]
public class EventSearchQueryStringBuilderServiceTests
{
    [TestCase(null, null, "", "", "", "", 0, 1)]
    [TestCase("2023-05-31", null, "", "", "From date", "", 1, 1)]
    [TestCase(null, "2024-01-01", "", "", "To date", "", 1, 1)]
    [TestCase("2023-05-31", "2024-01-01", "?toDate=2024-01-01", "?fromDate=2023-05-31", "From date", "To date", 2, 1)]
    public void BuildEventSearchFilters(DateTime? fromDate, DateTime? toDate, string expectedFirst, string expectedSecond, string fieldName1, string fieldName2, int expectedNumberOfFilters, int fieldOrder1)
    {
        var locationUrl = "network-events";
        var mockUrlHelper = new Mock<IUrlHelper>();
        mockUrlHelper
            .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
            .Returns(locationUrl);

        var service = new EventSearchQueryStringBuilder();

        var eventFilters = new EventFilters
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
}
