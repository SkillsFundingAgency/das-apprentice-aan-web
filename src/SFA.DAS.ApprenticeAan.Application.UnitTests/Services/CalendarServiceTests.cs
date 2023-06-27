using FluentAssertions;
using Moq;
using SFA.DAS.ApprenticeAan.Application.Services;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Application.UnitTests.Services;

[TestFixture]
public class CalendarServiceTests
{
    [MoqAutoData]
    public async Task Service_GetCalendars_ReturnsOrderedCalendarsList(
        Mock<IOuterApiClient> outerApiClient)
    {
        var sut = new CalendarService(outerApiClient.Object);

        var result = await sut.GetCalendars();
        result.Should().NotBeNullOrEmpty();
        result.Should().BeInAscendingOrder(x => x.Ordering);
    }
}