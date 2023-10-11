using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Aan.SharedUi.Constants;
using SFA.DAS.Aan.SharedUi.Extensions;
using SFA.DAS.Aan.SharedUi.Infrastructure;
using SFA.DAS.Aan.SharedUi.Models;
using SFA.DAS.ApprenticeAan.Domain.Constants;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Extensions;

namespace SFA.DAS.ApprenticeAan.Web.Controllers;

[Authorize]
[Route("member-profile")]
public class MemberProfileController : Controller
{
    private readonly IOuterApiClient _outerApiClient;
    public const string MemberProfileViewPath = "~/Views/MemberProfile/Profile.cshtml";
    private static List<int> eventsProfileIds = new List<int>()
    {
        ProfileDataId.NetworkingAtInPersonEvents,
        ProfileDataId.PresentingAtInPersonEvents,
        ProfileDataId.PresentingAtInHybridEvents,
        ProfileDataId.PresentingAtOnlineEvents,
        ProfileDataId.ProjectManagementOfRegionalAndNationalEvents
    };
    private static List<int> promotionsProfileIds = new List<int>()
    {
        ProfileDataId.CaseStudyPromotion,
        ProfileDataId.MarketingMaterialPromotion,
        ProfileDataId.NetworkPromotion,
        ProfileDataId.EngagingWithStakeholdersPromotion,
        ProfileDataId.SocialMediaChannelsPromotion
    };
    private static int linkedinProfileId = ProfileDataId.LinkedinUrl;
    private static int jobTitleProfileId = ProfileDataId.JobTitle;
    private static int biographyProfileId = ProfileDataId.Biography;
    private static int employerNameProfileId = ProfileDataId.EmployerName;
    private static List<int> addressProfileIds = new List<int>()
    {
        ProfileDataId.AddressLine1,
        ProfileDataId.AddressLine2,
        ProfileDataId.Town,
        ProfileDataId.County,
        ProfileDataId.Postcode
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
        var memberProfileResponse = await _outerApiClient.GetMemberProfile(memberId, id, true, cancellationToken);
        var userTypeProfile = await _outerApiClient.GetProfilesByUserType(Role.Apprentice.GetDescription(), cancellationToken);
        if (memberProfileResponse.ResponseMessage.IsSuccessStatusCode)
        {
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
            return View(MemberProfileViewPath, new MemberProfileViewModel(memberProfileResponse.GetContent(), memberProfileMappingModel, GetUserTypeProfiles(userTypeProfile.Profiles)));
        }
        throw new InvalidOperationException($"A member profile with ID {id} was not found.");
    }

    public static List<UserTypeProfilesModel> GetUserTypeProfiles(List<SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses.Profile> profiles)
    {
        return profiles.Select(item => new UserTypeProfilesModel
        {
            Id = item.Id,
            Description = item.Description,
            Category = item.Category,
            Ordering = item.Ordering,
        }).ToList();
    }
}
