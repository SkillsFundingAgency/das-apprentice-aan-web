using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Aan.SharedUi.Infrastructure;
using SFA.DAS.Aan.SharedUi.Models;
using SFA.DAS.Aan.SharedUi.Models.AmbassadorProfile;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses;
using SFA.DAS.ApprenticeAan.Web.Extensions;
using static SFA.DAS.Aan.SharedUi.Constants.ProfileConstants;

namespace SFA.DAS.ApprenticeAan.Web.Controllers;

[Authorize]
[Route("member-profile")]
public class MemberProfileController : Controller
{
    private readonly IOuterApiClient _outerApiClient;
    public const string MemberProfileViewPath = "~/Views/MemberProfile/Profile.cshtml";
    private static List<int> eventsProfileIds = new List<int>()
    {
        ProfileIds.NetworkingAtEventsInPerson,
        ProfileIds.PresentingAtEventsInPerson,
        ProfileIds.PresentingAtHybridEventsOnlineAndInPerson,
        ProfileIds.PresentingAtOnlineEvents,
        ProfileIds.ProjectManagementAndDeliveryOfRegionalEventsOrPlayingARoleInOrganisingNationalEvents
    };
    private static List<int> promotionsProfileIds = new List<int>()
    {
        ProfileIds.CarryingOutAndWritingUpCaseStudies,
        ProfileIds.DesigningAndCreatingMarketingMaterialsToChampionTheNetwork,
        ProfileIds.DistributingCommunicationsToTheNetwork,
        ProfileIds.EngagingWithStakeholdersToWorkOutHowToImproveTheNetwork,
        ProfileIds.PromotingTheNetworkOnSocialMediaChannels
    };
    private static int linkedinProfileId = ProfileIds.LinkedIn;
    private static int jobTitleProfileId = ProfileIds.JobTitle;
    private static int biographyProfileId = ProfileIds.Biography;
    private static int employerNameProfileId = ProfileIds.EmployerName;
    private static List<int> addressProfileIds = new List<int>()
    {
        ProfileIds.EmployerAddress1,
        ProfileIds.EmployerAddress2,
        ProfileIds.EmployerTownOrCity,
        ProfileIds.EmployerCounty,
        ProfileIds.EmployerPostcode
    };

    public MemberProfileController(IOuterApiClient outerApiClient)
    {
        _outerApiClient = outerApiClient;
    }

    [HttpGet]
    [Route("{id}", Name = SharedRouteNames.MemberProfile)]
    public async Task<IActionResult> Get([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var memberId = User.GetAanMemberId();

        var profiles = _outerApiClient.GetProfilesByUserType(MemberUserType.Apprentice.ToString(), cancellationToken);
        var memberProfiles = _outerApiClient.GetMemberProfile(id, User.GetAanMemberId(), true, cancellationToken);
        await Task.WhenAll(profiles, memberProfiles);

        MemberProfileMappingModel memberProfileMappingModel = new()
        {
            LinkedinProfileId = linkedinProfileId,
            JobTitleProfileId = jobTitleProfileId,
            BiographyProfileId = biographyProfileId,
            FirstSectionProfileIds = eventsProfileIds,
            SecondSectionProfileIds = promotionsProfileIds,
            AddressProfileIds = addressProfileIds,
            EmployerNameProfileId = employerNameProfileId,
            IsLoggedInUserMemberProfile = (id == memberId)
        };

        MemberProfileViewModel model = new(MemberProfileDetailMapping(memberProfiles), profiles.Result.Profiles, memberProfileMappingModel);
        return View(MemberProfileViewPath, model);
    }

    public static MemberProfileDetail MemberProfileDetailMapping(Task<GetMemberProfileResponse> memberProfiles)
    {
        MemberProfileDetail memberProfileDetail = new MemberProfileDetail();
        memberProfileDetail.FullName = memberProfiles.Result.FullName;
        memberProfileDetail.Email = memberProfiles.Result.Email;
        memberProfileDetail.FirstName = memberProfiles.Result.FirstName;
        memberProfileDetail.LastName = memberProfiles.Result.LastName;
        memberProfileDetail.OrganisationName = memberProfiles.Result.OrganisationName;
        memberProfileDetail.RegionId = memberProfiles.Result.RegionId;
        memberProfileDetail.RegionName = memberProfiles.Result.RegionName;
        memberProfileDetail.UserType = memberProfiles.Result.UserType;
        memberProfileDetail.IsRegionalChair = memberProfiles.Result.IsRegionalChair;
        if (memberProfiles.Result.Apprenticeship != null)
        {
            memberProfileDetail.Sector = memberProfiles.Result.Apprenticeship!.Sector;
            memberProfileDetail.Programmes = memberProfiles.Result.Apprenticeship!.Programme;
            memberProfileDetail.Level = memberProfiles.Result.Apprenticeship!.Level;
        }
        memberProfileDetail.Profiles = memberProfiles.Result.Profiles;
        return memberProfileDetail;
    }
}