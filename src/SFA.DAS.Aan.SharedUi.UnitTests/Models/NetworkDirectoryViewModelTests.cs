using FluentAssertions;
using SFA.DAS.Aan.SharedUi.Constants;
using SFA.DAS.Aan.SharedUi.Models.NetworkDirectory;
using SFA.DAS.Aan.SharedUi.Models.NetworkEvents;
using SFA.DAS.Aan.SharedUi.OuterApi.Responses;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Models;

[TestFixture]
public class NetworkDirectoryViewModelTests
{
    [TestCase(true, true)]
    [TestCase(false, false)]
    public void ShowFilterOptions_ReturningExpectedValueFromParameters(bool searchFilterAdded, bool expected)
    {
        var model = new NetworkDirectoryViewModel()
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

    [Test]
    [MoqAutoData]
    public void MembersViewModel_RegionalChairFalse(NetworkDirectorySummary networkDirectorySummary)
    {
        networkDirectorySummary.IsRegionalChair = false;
        var membersViewModel = (MembersViewModel)networkDirectorySummary;

        Assert.Multiple(() =>
        {
            Assert.That(membersViewModel, Is.Not.Null);
            Assert.That(membersViewModel.MemberId, Is.EqualTo(networkDirectorySummary.MemberId));
            Assert.That(membersViewModel.RegionId, Is.EqualTo(networkDirectorySummary.RegionId));
            Assert.That(membersViewModel.RegionName, Is.EqualTo(networkDirectorySummary.RegionName));
            Assert.That(membersViewModel.IsRegionalChair, Is.EqualTo(networkDirectorySummary.IsRegionalChair));
            Assert.That(membersViewModel.FullName, Is.EqualTo(networkDirectorySummary.FullName));
            Assert.That(membersViewModel.UserType, Is.EqualTo(networkDirectorySummary.UserType));
            Assert.That(membersViewModel.JoinedDate, Is.EqualTo(networkDirectorySummary.JoinedDate));
        });
    }

    [Test]
    [MoqAutoData]
    public void MembersViewModel_RegionalChairTrue(NetworkDirectorySummary networkDirectorySummary)
    {
        networkDirectorySummary.IsRegionalChair = true;
        var membersViewModel = (MembersViewModel)networkDirectorySummary;

        Assert.Multiple(() =>
        {
            Assert.That(membersViewModel, Is.Not.Null);
            Assert.That(membersViewModel.MemberId, Is.EqualTo(networkDirectorySummary.MemberId));
            Assert.That(membersViewModel.RegionId, Is.EqualTo(networkDirectorySummary.RegionId));
            Assert.That(membersViewModel.RegionName, Is.EqualTo(networkDirectorySummary.RegionName));
            Assert.That(membersViewModel.IsRegionalChair, Is.EqualTo(networkDirectorySummary.IsRegionalChair));
            Assert.That(membersViewModel.FullName, Is.EqualTo(networkDirectorySummary.FullName));
            Assert.That(membersViewModel.UserType, Is.EqualTo(Role.IsRegionalChair));
            Assert.That(membersViewModel.JoinedDate, Is.EqualTo(networkDirectorySummary.JoinedDate));
        });
    }

    [Test]
    [MoqAutoData]
    public void MembersViewModel_RegionIdNull(NetworkDirectorySummary networkDirectorySummary)
    {
        networkDirectorySummary.RegionId = null;
        var membersViewModel = (MembersViewModel)networkDirectorySummary;

        Assert.Multiple(() =>
        {
            Assert.That(membersViewModel, Is.Not.Null);
            Assert.That(membersViewModel.MemberId, Is.EqualTo(networkDirectorySummary.MemberId));
            Assert.That(membersViewModel.RegionId, Is.EqualTo(networkDirectorySummary.RegionId));
            Assert.That(membersViewModel.RegionName, Is.EqualTo("Multi-regional"));
            Assert.That(membersViewModel.IsRegionalChair, Is.EqualTo(networkDirectorySummary.IsRegionalChair));
            Assert.That(membersViewModel.FullName, Is.EqualTo(networkDirectorySummary.FullName));
            Assert.That(membersViewModel.UserType, Is.EqualTo(Role.IsRegionalChair));
            Assert.That(membersViewModel.JoinedDate, Is.EqualTo(networkDirectorySummary.JoinedDate));
        });
    }

    [Test]
    [MoqAutoData]
    public void MembersViewModel_RegionIdNotNull(NetworkDirectorySummary networkDirectorySummary)
    {
        networkDirectorySummary.RegionId = 1;
        var membersViewModel = (MembersViewModel)networkDirectorySummary;

        Assert.Multiple(() =>
        {
            Assert.That(membersViewModel, Is.Not.Null);
            Assert.That(membersViewModel.MemberId, Is.EqualTo(networkDirectorySummary.MemberId));
            Assert.That(membersViewModel.RegionId, Is.EqualTo(networkDirectorySummary.RegionId));
            Assert.That(membersViewModel.RegionName, Is.EqualTo(networkDirectorySummary.RegionName));
            Assert.That(membersViewModel.IsRegionalChair, Is.EqualTo(networkDirectorySummary.IsRegionalChair));
            Assert.That(membersViewModel.FullName, Is.EqualTo(networkDirectorySummary.FullName));
            Assert.That(membersViewModel.UserType, Is.EqualTo(Role.IsRegionalChair));
            Assert.That(membersViewModel.JoinedDate, Is.EqualTo(networkDirectorySummary.JoinedDate));
        });
    }
}