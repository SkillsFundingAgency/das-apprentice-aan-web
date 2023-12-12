using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Pipelines.Sockets.Unofficial.Arenas;
using SFA.DAS.Aan.SharedUi.Models;
using SFA.DAS.Aan.SharedUi.Models.AmbassadorProfile;
using SFA.DAS.ApprenticeAan.Web.Models.AmbassadorProfile;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Models.AmbassadorProfile;
public class AmbassadorProfileViewModelTests
{
    private List<Profile> profiles;
    private AmbassadorProfileViewModel sut;
    private string fullName;
    private string regionName;
    private string email;
    private string memberProfileUrl = Guid.NewGuid().ToString();
    private string personalDetailsChangeUrl = Guid.NewGuid().ToString();
    private string areaOfInterestChangeUrl = Guid.NewGuid().ToString();
    private string contactDetailChangeUrl = Guid.NewGuid().ToString();
    private IEnumerable<MemberProfile> memberProfiles;
    private IEnumerable<MemberPreference> memberPreferences;
    private ApprenticeshipDetailsModel? apprenticeshipDetails;
    private MemberUserType userType;

    [SetUp]
    public void Setup()
    {
        var fixture = new Fixture();
        fullName = fixture.Create<string>();
        regionName = fixture.Create<string>();
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
        apprenticeshipDetails = fixture.Create<ApprenticeshipDetailsModel>();
        userType = MemberUserType.Apprentice;
        profiles = new List<Profile>()
        {
            new Profile { Id = 1, Description = "Networking at events in person", Category = "Events", Ordering = 1 },
            new Profile { Id = 2, Description = "Presenting at events in person", Category = "Events", Ordering = 2 },
            new Profile { Id = 10, Description = "Carrying out and writing up case studies", Category = "Promotions", Ordering = 1 },
            new Profile { Id = 11, Description = "Designing and creating marketing materials to champion the network", Category = "Promotions", Ordering = 2 }
        };
        var personalDetails = new PersonalDetailsModel(fullName, regionName, userType, personalDetailsChangeUrl, areaOfInterestChangeUrl, contactDetailChangeUrl, memberProfileUrl);
        sut = new AmbassadorProfileViewModel(personalDetails, email, memberProfiles, memberPreferences, apprenticeshipDetails, profiles, Mock.Of<IUrlHelper>());
    }

    [Test]
    public void ViewModelsAreSet()
    {
        using (new AssertionScope())
        {
            sut.ApprenticeshipDetails.Should().NotBeNull();
            sut.ContactDetails.Should().NotBeNull();
            sut.InterestInTheNetwork.Should().NotBeNull();
            sut.PersonalDetails.Should().NotBeNull();
            sut.ShowApprenticeshipDetails.GetType().Should().Be(typeof(bool));
            sut.MemberProfileUrl.Should().NotBeNull();
            sut.PersonalDetails.PersonalDetailsChangeUrl.Should().NotBeNull();
        }
    }

    [Test]
    public void PersonalDetailsChangeUrlInViewModelIsSet()
    {
        sut.PersonalDetails.PersonalDetailsChangeUrl.Should().Be(personalDetailsChangeUrl);
    }

    [Test]
    public void ApprenticeshipDetailsViewModelIsSet()
    {
        using (new AssertionScope())
        {
            sut.ApprenticeshipDetails.Should().NotBeNull();
            sut.ApprenticeshipDetails.ApprenticeshipLevel.Should().Be(apprenticeshipDetails?.Level);
            sut.ApprenticeshipDetails.ApprenticeshipProgramme.Should().Be(apprenticeshipDetails?.Programme);
            sut.ApprenticeshipDetails.ApprenticeshipSector.Should().Be(apprenticeshipDetails?.Sector);
        }
    }

    [Test]
    public void ShowApprenticeshipDetailsIsSet()
    {
        var apprenticeshipDetailsViewModel = new ApprenticeshipDetailsViewModel(memberProfiles, apprenticeshipDetails, memberPreferences, Mock.Of<IUrlHelper>());
        if (apprenticeshipDetails == null && apprenticeshipDetailsViewModel.EmployerAddress == null && apprenticeshipDetailsViewModel.EmployerName == null)
        {
            sut.ShowApprenticeshipDetails.Should().BeFalse();
        }
        else
        {
            sut.ShowApprenticeshipDetails.Should().BeTrue();
        }
    }
}
