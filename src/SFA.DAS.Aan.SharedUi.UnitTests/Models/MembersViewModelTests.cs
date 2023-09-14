using FluentAssertions;
using SFA.DAS.Aan.SharedUi.Constants;
using SFA.DAS.Aan.SharedUi.Models.NetworkDirectory;
using SFA.DAS.Aan.SharedUi.OuterApi.Responses;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Models;

[TestFixture]
public class MembersViewModelTests
{
    [Test]
    [MoqAutoData]
    public void MembersViewModel_RegionalChairFalse(MemberSummary networkDirectorySummary)
    {
        networkDirectorySummary.IsRegionalChair = false;
        var sut = (MembersViewModel)networkDirectorySummary;

        Assert.Multiple(() =>
        {
            Assert.That(sut.IsRegionalChair, Is.EqualTo(networkDirectorySummary.IsRegionalChair));
            Assert.That(sut.UserRole, Is.EqualTo(networkDirectorySummary.UserType));
        });
    }

    [Test]
    [MoqAutoData]
    public void MembersViewModel_RegionalChairTrue(MemberSummary networkDirectorySummary)
    {
        networkDirectorySummary.IsRegionalChair = true;
        var sut = (MembersViewModel)networkDirectorySummary;

        Assert.Multiple(() =>
        {
            Assert.That(sut.IsRegionalChair, Is.EqualTo(networkDirectorySummary.IsRegionalChair));
            Assert.That(sut.UserRole, Is.EqualTo(Role.RegionalChair));
        });
    }

    [Test]
    [MoqAutoData]
    public void MembersViewModel_RegionIdNull(MemberSummary networkDirectorySummary)
    {
        networkDirectorySummary.RegionId = null;
        var sut = (MembersViewModel)networkDirectorySummary;

        Assert.Multiple(() =>
        {
            Assert.That(sut.RegionId, Is.EqualTo(networkDirectorySummary.RegionId));
            Assert.That(sut.RegionName, Is.EqualTo("Multi-regional"));
        });
    }

    [Test]
    [MoqAutoData]
    public void MembersViewModel_RegionIdNotNull(MemberSummary networkDirectorySummary)
    {
        networkDirectorySummary.RegionId = 1;
        var sut = (MembersViewModel)networkDirectorySummary;

        Assert.Multiple(() =>
        {
            Assert.That(sut.RegionId, Is.EqualTo(networkDirectorySummary.RegionId));
            Assert.That(sut.RegionName, Is.EqualTo(networkDirectorySummary.RegionName));
        });
    }


    [Test, MoqAutoData]
    public void Operator_GivenMemberSummary_ReturnsRequest(MemberSummary model)
    {
        MembersViewModel sut = model;
        sut.Should().BeEquivalentTo(model, options => options.ExcludingMissingMembers());
    }
}