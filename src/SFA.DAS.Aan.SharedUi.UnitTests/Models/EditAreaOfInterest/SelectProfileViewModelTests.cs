using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using SFA.DAS.Aan.SharedUi.Models.EditAreaOfInterest;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Aan.SharedUi.UnitTests.Models.SelectProfileViewModelTests;
public class SelectProfileViewModelTests
{
    private SelectProfileViewModel sut = null!;
    private ProfileViewModel profileViewModel;

    [SetUp]
    public void Setup()
    {
        var fixture = new Fixture();
        profileViewModel = fixture.Create<ProfileViewModel>();
    }

    [Test]
    [MoqInlineAutoData(true, "True")]
    [MoqInlineAutoData(false, "False")]
    [MoqInlineAutoData(false, "test")]
    public void PersonalDetailsViewModelIsSet(bool isSelected, string profileValue)
    {
        profileViewModel.Value = profileValue;
        sut = (ProfileViewModel)profileViewModel;
        using (new AssertionScope())
        {
            sut.Should().NotBeNull();
            sut.Id.Should().Be(profileViewModel.Id);
            sut.Description.Should().Be(profileViewModel.Description);
            sut.Category.Should().Be(profileViewModel.Category);
            sut.Ordering.Should().Be(profileViewModel.Ordering);
            Assert.That(sut.IsSelected, Is.EqualTo(isSelected));
        }
    }
}
