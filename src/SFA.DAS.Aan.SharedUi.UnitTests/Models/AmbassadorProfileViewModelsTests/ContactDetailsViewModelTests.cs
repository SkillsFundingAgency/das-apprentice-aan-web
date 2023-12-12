using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using SFA.DAS.Aan.SharedUi.Models.AmbassadorProfile;

namespace SFA.DAS.Aan.SharedUi.UnitTests.Models.AmbassadorProfileViewModelsTests;
public class ContactDetailsViewModelTests
{
    private ContactDetailsViewModel sut;
    private string email;
    private IEnumerable<MemberProfile> memberProfiles;
    private IEnumerable<MemberPreference> memberPreferences;
    private string contactDetailChangeUrl = Guid.NewGuid().ToString();

    [SetUp]
    public void Setup()
    {
        var fixture = new Fixture();
        email = fixture.Create<string>();
        memberProfiles = fixture.CreateMany<MemberProfile>(4);
        memberProfiles.ToArray()[0].ProfileId = 1;
        memberProfiles.ToArray()[0].Value = "true";
        memberProfiles.ToArray()[1].ProfileId = 2;
        memberProfiles.ToArray()[1].Value = "true";
        memberProfiles.ToArray()[2].ProfileId = 10;
        memberProfiles.ToArray()[2].Value = "true";
        memberProfiles.ToArray()[3].ProfileId = 11;
        memberProfiles.ToArray()[3].Value = "true";
        memberPreferences = fixture.CreateMany<MemberPreference>();
        sut = new ContactDetailsViewModel(email, memberProfiles, memberPreferences, contactDetailChangeUrl);
    }

    [Test]
    public void ContactDetailsViewModelIsSet()
    {
        using (new AssertionScope())
        {
            sut.Should().NotBeNull();
            sut.EmailAddress.Should().Be(email);
            sut.LinkedInDisplayClass.Should().NotBeNullOrEmpty();
            sut.LinkedInDisplayValue.Should().NotBeNullOrEmpty();
        }
    }

    [Test]
    public void ContactDetailViewModel_InitializationWithParameterlessConstructor_ReturnsExpectedValue()
    {
        // Act
        ContactDetailsViewModel _sut = new ContactDetailsViewModel();

        // Assert
        using (new AssertionScope())
        {
            Assert.That(_sut, Is.Not.Null);
            Assert.That(_sut.EmailAddress, Is.Null);
            Assert.That(_sut.LinkedIn, Is.Null);
            Assert.That(_sut.LinkedInDisplayClass, Is.Null);
            Assert.That(_sut.LinkedInDisplayValue, Is.Null);
            Assert.That(_sut.ContactDetailChangeUrl, Is.Null);
        }
    }
}
