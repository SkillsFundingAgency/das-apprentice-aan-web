using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using SFA.DAS.Aan.SharedUi.Models;
using SFA.DAS.Aan.SharedUi.Models.EditAreaOfInterest;

namespace SFA.DAS.Aan.SharedUi.UnitTests.Models.ProfileViewModelsTests;
public class ProfileViewModelTests
{
    private ProfileViewModel sut;
    private Profile profile;

    [SetUp]
    public void Setup()
    {
        var fixture = new Fixture();
        profile = fixture.Create<Profile>();
        sut = (ProfileViewModel)profile;
    }

    [Test]
    public void ProfileViewModelIsSet()
    {
        using (new AssertionScope())
        {
            sut.Should().NotBeNull();
            sut.Id.Should().Be(profile.Id);
            sut.Description.Should().Be(profile.Description);
            sut.Category.Should().Be(profile.Category);
            sut.Ordering.Should().Be(profile.Ordering);
        }
    }
}
