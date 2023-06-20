using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Domain.Models;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses;
using SFA.DAS.ApprenticeAan.Web.Controllers;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.ApprenticePortal.Authentication.TestHelpers;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Models;

[TestFixture]
public class NetworkEventsViewModelTests
{
    [Test]
    [MoqAutoData]
    public void NetworkEventsViewModel_From_GetCalendarEventsQueryResult(GetCalendarEventsQueryResult result, IUrlHelper helper)
    {
        var sut = new NetworkEventsViewModel(result, helper);

        Assert.Multiple(() =>
        {
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Page, Is.EqualTo(result.Page));
            Assert.That(sut.PageSize, Is.EqualTo(result.PageSize));
            Assert.That(sut.TotalPages, Is.EqualTo(result.TotalPages));
            Assert.That(sut.TotalCount, Is.EqualTo(result.TotalCount));
            Assert.That(sut.CalendarEvents, Is.EquivalentTo(result.CalendarEvents));
        });
    }

    [TestCase(true, true)]
    [TestCase(false, false)]
    public void ShowFilterOptions_ReturningExpectedValueFromParameters(bool searchFilterAdded, bool expected)
    {
        var model = new NetworkEventsViewModel(null)
        {
            SearchFilters = new List<SelectedFilter>()
        };

        if (searchFilterAdded)
        {
            model.SearchFilters.Add(new SelectedFilter());
        }

        var actual = model.ShowFilterOptions;
        actual.Should().Be(expected);
    }
}