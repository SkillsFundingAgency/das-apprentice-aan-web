using AutoFixture;
using SFA.DAS.Aan.SharedUi.Models.AmbassadorProfile;
using FluentAssertions.Execution;
using FluentAssertions;

namespace SFA.DAS.Aan.SharedUi.UnitTests.Models.AmbassadorProfileViewModelsTests;
public class ContactDetailsViewModelTests
{
    private ContactDetailsViewModel sut;
    private string email;
    private IEnumerable<MemberProfile> memberProfiles;
    private IEnumerable<MemberPreference> memberPreferences;

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
        sut = new ContactDetailsViewModel(email, memberProfiles, memberPreferences);
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
}
