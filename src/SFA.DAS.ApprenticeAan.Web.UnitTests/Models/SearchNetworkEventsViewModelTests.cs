using SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Models;

[TestFixture]
public class SearchNetworkEventsViewModelTests
{
    [Test]
    [MoqAutoData]
    public void SearchNetworkEventsViewModel_From_GetCalendarEventsQueryResult(GetCalendarEventsQueryResult result)
    {
        var viewModel = (NetworkEventsViewModel)result;

        Assert.Multiple(() =>
        {
            Assert.That(viewModel, Is.Not.Null);
            Assert.That(viewModel.Page, Is.EqualTo(result.Page));
            Assert.That(viewModel.PageSize, Is.EqualTo(result.PageSize));
            Assert.That(viewModel.TotalPages, Is.EqualTo(result.TotalPages));
            Assert.That(viewModel.TotalCount, Is.EqualTo(result.TotalCount));
            Assert.That(viewModel.CalendarEvents, Is.EquivalentTo(result.CalendarEvents));
        });
    }
}
