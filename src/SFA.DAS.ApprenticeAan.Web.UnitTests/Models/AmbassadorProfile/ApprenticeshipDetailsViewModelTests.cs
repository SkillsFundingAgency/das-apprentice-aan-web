using AutoFixture;
using SFA.DAS.Aan.SharedUi.Models.AmbassadorProfile;
using SFA.DAS.ApprenticeAan.Web.Models.AmbassadorProfile;
using FluentAssertions.Execution;
using FluentAssertions;
using SFA.DAS.Aan.SharedUi.Constants;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Models.AmbassadorProfile;
public class ApprenticeshipDetailsViewModelTests
{
    private ApprenticeshipDetailsViewModel sut;
    private IEnumerable<MemberProfile> memberProfiles;
    private ApprenticeshipDetailsModel? apprenticeshipDetails;
    private IEnumerable<MemberPreference> memberPreferences;

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
        memberPreferences = fixture.CreateMany<MemberPreference>();
        memberPreferences.ToArray()[0].PreferenceId = PreferenceConstants.PreferenceIds.Apprenticeship;
        apprenticeshipDetails = fixture.Create<ApprenticeshipDetailsModel>();
        sut = new ApprenticeshipDetailsViewModel(memberProfiles, apprenticeshipDetails, memberPreferences);
    }

    [Test]
    public void ViewModelIsSet()
    {
        using (new AssertionScope())
        {
            sut.Should().NotBeNull();
            sut.EmployerName.Should().Be(memberProfiles.FirstOrDefault(x => x.ProfileId == ProfileConstants.ProfileIds.EmployerName)?.Value);
            if (memberProfiles.FirstOrDefault(x => x.ProfileId == ProfileConstants.ProfileIds.EmployerAddress1)?.Value != null)
            {
                sut.EmployerAddress.Should().Contain(memberProfiles.FirstOrDefault(x => x.ProfileId == ProfileConstants.ProfileIds.EmployerAddress1)?.Value);
            }
            else
            {
                sut.EmployerAddress.Should().BeNullOrEmpty();
            }
            sut.ApprenticeshipSector.Should().Be(apprenticeshipDetails?.Sector);
            sut.ApprenticeshipProgramme.Should().Be(apprenticeshipDetails?.Programmes);
            sut.ApprenticeshipLevel.Should().Be(apprenticeshipDetails?.Level);
        }
    }

    [Test]
    public void ApprenticeshipDetailsDisplayValuesAreSet()
    {
        using (new AssertionScope())
        {
            if ((bool)memberPreferences.FirstOrDefault(x => x.PreferenceId == PreferenceConstants.PreferenceIds.Apprenticeship)?.Value!)
            {
                sut.ApprenticeshipDetailsDisplayClass.Should().Be("govuk-tag");
                sut.ApprenticeshipDetailsDisplayValue.Should().Be("Displayed");
            }
            else
            {
                sut.ApprenticeshipDetailsDisplayClass.Should().Be("govuk-tag--blue");
                sut.ApprenticeshipDetailsDisplayValue.Should().Be("Hidden");
            }
        }
    }
}
