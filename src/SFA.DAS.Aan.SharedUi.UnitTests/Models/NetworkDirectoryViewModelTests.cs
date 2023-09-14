using FluentAssertions;
using SFA.DAS.Aan.SharedUi.Models.NetworkDirectory;
using SFA.DAS.Aan.SharedUi.Models.NetworkEvents;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Models;

[TestFixture]
public class NetworkDirectoryViewModelTests
{
    [TestCase(true, true)]
    [TestCase(false, false)]
    public void ShowFilterOptions_ReturningExpectedValueFromParameters(bool searchFilterAdded, bool expected)
    {
        var sut = new NetworkDirectoryViewModel()
        {
            SelectedFiltersModel = new SelectedFiltersModel()
            {
                SelectedFilters = new List<SelectedFilter>()
            }
        };

        if (searchFilterAdded)
        {
            sut.SelectedFiltersModel.SelectedFilters.Add(new SelectedFilter());
        }

        var actual = sut.SelectedFiltersModel.ShowFilterOptions;
        actual.Should().Be(expected);
    }
}