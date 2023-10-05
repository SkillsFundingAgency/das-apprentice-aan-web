﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    [Route("{id}/{public}", Name = SharedRouteNames.MemberProfile)]
    public async Task<IActionResult> Get([FromRoute] Guid id, [FromRoute] bool @public, CancellationToken cancellationToken)
    {
        var memberId = User.GetAanMemberId();
        var memberProfileResponse = await _outerApiClient.GetMemberProfile(memberId, id, @public, cancellationToken);
        if (memberProfileResponse.ResponseMessage.IsSuccessStatusCode)
        {
            return View(MemberProfileViewPath, new MemberProfileViewModel(
                memberProfileResponse.GetContent(), new MemberProfileMappingModel(linkedinProfileId, jobTitleProfileId, biographyProfileId, eventsProfileIds, promotionsProfileIds, addressProfileIds, (id == memberId))));
        }
        throw new InvalidOperationException($"A member profile with ID {id} was not found.");
    }
}