using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Aan.SharedUi.Infrastructure;
using SFA.DAS.Aan.SharedUi.Models;
using SFA.DAS.Aan.SharedUi.Models.AmbassadorProfile;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Requests;
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
    public const string NotificationSentConfirmationViewPath = "~/Views/MemberProfile/NotificationSentConfirmation.cshtml";
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
    private static List<int> addressProfileIds = new List<int>()
    {
        ProfileIds.EmployerAddress1,
        ProfileIds.EmployerAddress2,
        ProfileIds.EmployerTownOrCity,
        ProfileIds.EmployerCounty,
        ProfileIds.EmployerPostcode
    };
    private static List<int> reasonToJoinProfileIds = new List<int>()
    {
        ProfileIds.MeetOtherAmbassadorsAndGrowYourNetwork,
        ProfileIds.ShareYourKnowledgeExperienceAndBestPractice,
        ProfileIds.ProjectManageAndDeliverNetworkEvents,
        ProfileIds.BeARoleModelAndActAsAnInformalMentor,
        ProfileIds.ChampionApprenticeshipDeliveryWithinYourNetworks
    };
    private static List<int> supportProfileIds = new List<int>()
    {
        ProfileIds.BuildingApprenticeshipProfileOfMyOrganisation,
        ProfileIds.IncreasingEngagementWithSchoolsAndColleges,
        ProfileIds.GettingStartedWithApprenticeships,
        ProfileIds.UnderstandingTrainingProvidersAndResourcesOthersAreUsing,
        ProfileIds.UsingTheNetworkToBestBenefitMyOrganisation
    };
    private static List<int> employerAddressProfileIds = new List<int>()
    {
        ProfileIds.EmployerUserEmployerAddress1,
        ProfileIds.EmployerUserEmployerAddress2,
        ProfileIds.EmployerUserEmployerTownOrCity,
        ProfileIds.EmployerUserEmployerCounty,
        ProfileIds.EmployerUserEmployerPostcode
    };

    private readonly IValidator<SubmitConnectionCommand> _validator;
    public MemberProfileController(IOuterApiClient outerApiClient, IValidator<SubmitConnectionCommand> validator)
    {
        _outerApiClient = outerApiClient;
        _validator = validator;
    }

    [HttpGet]
    [Route("{id}", Name = SharedRouteNames.MemberProfile)]
    public async Task<IActionResult> Get([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var memberId = User.GetAanMemberId();
        var memberProfiles = await _outerApiClient.GetMemberProfile(id, User.GetAanMemberId(), true, cancellationToken);
        MemberProfileViewModel model = await MemberProfileMapping(memberProfiles, (id == memberId), cancellationToken);
        return View(MemberProfileViewPath, model);
    }

    [HttpPost]
    [Route("{id}", Name = SharedRouteNames.MemberProfile)]
    public async Task<IActionResult> Post([FromRoute] Guid id, SubmitConnectionCommand command, CancellationToken cancellationToken)
    {
        var result = await _validator.ValidateAsync(command, cancellationToken);

        if (!result.IsValid)
        {
            var memberId = User.GetAanMemberId();
            var memberProfiles = await _outerApiClient.GetMemberProfile(id, User.GetAanMemberId(), true, cancellationToken);
            MemberProfileViewModel model = await MemberProfileMapping(memberProfiles, (id == memberId), cancellationToken);
            result.AddToModelState(ModelState);
            return View(MemberProfileViewPath, model);
        }
        CreateNotificationRequest createNotificationRequest = new CreateNotificationRequest(id, command.ReasonToGetInTouch);
        var response = await _outerApiClient.PostNotification(User.GetAanMemberId(), createNotificationRequest, cancellationToken);

        if (response.ResponseMessage.IsSuccessStatusCode)
        {
            return RedirectToAction("NotificationSentConfirmation");
        }
        throw new InvalidOperationException($"A problem occured while sending notification.");
    }


    [HttpGet]
    [Route("notificationsent-confirmation", Name = SharedRouteNames.NotificationSentConfirmation)]
    public IActionResult NotificationSentConfirmation()
    {
        NotificationSentConfirmationViewModel model = new(Url.RouteUrl(SharedRouteNames.NetworkDirectory)!);
        return View(NotificationSentConfirmationViewPath, model);
    }

    public async Task<MemberProfileViewModel> MemberProfileMapping(GetMemberProfileResponse memberProfiles, bool isLoggedInUserMemberProfile, CancellationToken cancellationToken)
    {
        MemberProfileDetail memberProfileDetail = MemberProfileDetailMapping(memberProfiles);
        MemberProfileMappingModel memberProfileMappingModel;
        GetProfilesResult profilesResult;
        if (memberProfileDetail.UserType == MemberUserType.Apprentice)
        {
            memberProfileMappingModel = new()
            {
                LinkedinProfileId = ProfileIds.LinkedIn,
                JobTitleProfileId = ProfileIds.JobTitle,
                BiographyProfileId = ProfileIds.Biography,
                FirstSectionProfileIds = eventsProfileIds,
                SecondSectionProfileIds = promotionsProfileIds,
                AddressProfileIds = addressProfileIds,
                EmployerNameProfileId = ProfileIds.EmployerName,
                IsLoggedInUserMemberProfile = isLoggedInUserMemberProfile
            };
            profilesResult = await _outerApiClient.GetProfilesByUserType(MemberUserType.Apprentice.ToString(), cancellationToken);
        }
        else
        {
            memberProfileMappingModel = new()
            {
                LinkedinProfileId = ProfileIds.EmployerLinkedIn,
                JobTitleProfileId = ProfileIds.EmployerJobTitle,
                BiographyProfileId = ProfileIds.EmployerBiography,
                FirstSectionProfileIds = reasonToJoinProfileIds,
                SecondSectionProfileIds = supportProfileIds,
                AddressProfileIds = employerAddressProfileIds,
                EmployerNameProfileId = ProfileIds.EmployerUserEmployerName,
                IsLoggedInUserMemberProfile = isLoggedInUserMemberProfile
            };
            profilesResult = await _outerApiClient.GetProfilesByUserType(MemberUserType.Employer.ToString(), cancellationToken);
        }

        return new(memberProfileDetail, profilesResult.Profiles, memberProfileMappingModel);
    }

    public static MemberProfileDetail MemberProfileDetailMapping(GetMemberProfileResponse memberProfiles)
    {
        MemberProfileDetail memberProfileDetail = new MemberProfileDetail();
        memberProfileDetail.FullName = memberProfiles.FullName;
        memberProfileDetail.Email = memberProfiles.Email;
        memberProfileDetail.FirstName = memberProfiles.FirstName;
        memberProfileDetail.LastName = memberProfiles.LastName;
        memberProfileDetail.OrganisationName = memberProfiles.OrganisationName;
        memberProfileDetail.RegionId = memberProfiles.RegionId;
        memberProfileDetail.RegionName = memberProfiles.RegionName;
        memberProfileDetail.UserType = memberProfiles.UserType;
        memberProfileDetail.IsRegionalChair = memberProfiles.IsRegionalChair;
        if (memberProfiles.Apprenticeship != null)
        {
            if (memberProfileDetail.UserType == MemberUserType.Apprentice)
            {
                memberProfileDetail.Sector = memberProfiles.Apprenticeship!.Sector;
                memberProfileDetail.Programmes = memberProfiles.Apprenticeship!.Programme;
                memberProfileDetail.Level = memberProfiles.Apprenticeship!.Level;
            }
            else
            {
                memberProfileDetail.Sectors = memberProfiles.Apprenticeship!.Sectors;
                memberProfileDetail.ActiveApprenticesCount = memberProfiles.Apprenticeship!.ActiveApprenticesCount;
            }
        }
        memberProfileDetail.Profiles = memberProfiles.Profiles;
        return memberProfileDetail;
    }
}