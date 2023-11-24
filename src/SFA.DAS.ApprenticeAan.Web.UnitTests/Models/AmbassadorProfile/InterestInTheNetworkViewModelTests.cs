using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using SFA.DAS.Aan.SharedUi.Constants;
using SFA.DAS.Aan.SharedUi.Models;
using SFA.DAS.Aan.SharedUi.Models.AmbassadorProfile;
using SFA.DAS.ApprenticeAan.Web.Models.AmbassadorProfile;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Models.AmbassadorProfile;
public class InterestInTheNetworkViewModelTests
{
    private InterestInTheNetworkViewModel sut;
    private List<Profile> profiles;
    private IEnumerable<MemberProfile> memberProfiles;
    private string areaOfInterestChangeUrl = Guid.NewGuid().ToString();

    [SetUp]
    public void Setup()
    {
        var fixture = new Fixture();
        memberProfiles = fixture.CreateMany<MemberProfile>(4);
        memberProfiles.ToArray()[0].ProfileId = 1;
        memberProfiles.ToArray()[0].Value = "true";
        memberProfiles.ToArray()[1].ProfileId = 2;
        memberProfiles.ToArray()[1].Value = "true";
        memberProfiles.ToArray()[2].ProfileId = 10;
        memberProfiles.ToArray()[2].Value = "true";
        memberProfiles.ToArray()[3].ProfileId = 11;
        memberProfiles.ToArray()[3].Value = "true";
        profiles = new List<Profile>()
        {
            new Profile { Id = 1, Description = "Networking at events in person", Category = "Events", Ordering = 1 },
            new Profile { Id = 2, Description = "Presenting at events in person", Category = "Events", Ordering = 2 },
            new Profile { Id = 10, Description = "Carrying out and writing up case studies", Category = "Promotions", Ordering = 1 },
            new Profile { Id = 11, Description = "Designing and creating marketing materials to champion the network", Category = "Promotions", Ordering = 2 }
        };
        sut = new InterestInTheNetworkViewModel(memberProfiles, profiles, areaOfInterestChangeUrl);
    }

    [Test]
    public void InterestInTheNetworkViewModelIsSet()
    {
        using (new AssertionScope())
        {
            sut.Should().NotBeNull();
            sut.EventsActivities.Should().HaveCount(2);
            sut.EventsActivities.ToArray()[0].Should().Be("Networking at events in person");
            sut.EventsActivities.ToArray()[1].Should().Be("Presenting at events in person");
            sut.PromotionActivities.Should().HaveCount(2);
            sut.PromotionActivities.ToArray()[0].Should().Be("Carrying out and writing up case studies");
            sut.PromotionActivities.ToArray()[1].Should().Be("Designing and creating marketing materials to champion the network");
            sut.InterestInTheNetworkDisplayed.Should().Be(PreferenceConstants.DisplayValue.DisplayTagName);
            sut.InterestInTheNetworkDisplayClass.Should().Be(PreferenceConstants.DisplayValue.DisplayTagClass);
            sut.AreaOfInterestChangeUrl.Should().Be(areaOfInterestChangeUrl);
        }
    }
}
