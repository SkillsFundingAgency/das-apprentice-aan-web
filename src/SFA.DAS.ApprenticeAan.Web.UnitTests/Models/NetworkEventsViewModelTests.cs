using SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Models;

[TestFixture]
public class NetworkEventsViewModelTests
{
    [Test]
    [MoqAutoData]
    public void NetworkEventsViewModel_From_GetCalendarEventsQueryResult(GetCalendarEventsQueryResult result)
    {
        var sut = (NetworkEventsViewModel)result;

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
}
