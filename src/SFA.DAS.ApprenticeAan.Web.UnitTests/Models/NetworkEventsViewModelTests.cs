using FluentAssertions;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.ApprenticeAan.Web.Models.NetworkEvents;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Models;

[TestFixture]
public class NetworkEventsViewModelTests
{
    [TestCase(true, true)]
    [TestCase(false, false)]
    public void ShowFilterOptions_ReturningExpectedValueFromParameters(bool searchFilterAdded, bool expected)
    {
        var model = new NetworkEventsViewModel()
        {
            SelectedFilters = new List<SelectedFilter>()
        };

        if (searchFilterAdded)
        {
            model.SelectedFilters.Add(new SelectedFilter());
        }

        var actual = model.ShowFilterOptions;
        actual.Should().Be(expected);
    }
}