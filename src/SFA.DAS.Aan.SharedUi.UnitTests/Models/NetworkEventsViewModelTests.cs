using FluentAssertions;
using SFA.DAS.Aan.SharedUi.Models.NetworkEvents;

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
            SelectedFiltersModel = new SelectedFiltersModel()
            {
                SelectedFilters = new List<SelectedFilter>()
            }
        };

        if (searchFilterAdded)
        {
            model.SelectedFiltersModel.SelectedFilters.Add(new SelectedFilter());
        }

        var actual = model.SelectedFiltersModel.ShowFilterOptions;
        actual.Should().Be(expected);
    }
}