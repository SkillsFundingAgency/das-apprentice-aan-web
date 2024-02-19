using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Aan.SharedUi.Constants;
using SFA.DAS.Aan.SharedUi.Infrastructure;
using SFA.DAS.Aan.SharedUi.Models;
using SFA.DAS.Aan.SharedUi.Models.AmbassadorProfile;
using SFA.DAS.Aan.SharedUi.Services;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Requests;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses;
using SFA.DAS.ApprenticeAan.Web.Extensions;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using static SFA.DAS.Aan.SharedUi.Constants.ProfileConstants;

namespace SFA.DAS.ApprenticeAan.Web.Controllers;

[Authorize]
[Route("edit-personal-information", Name = SharedRouteNames.EditPersonalInformation)]
public class EditPersonalInformationController(IOuterApiClient apiClient, IValidator<SubmitPersonalDetailModel> validator, ISessionService sessionService) : Controller
{
    private readonly IOuterApiClient _apiClient = apiClient;
    private readonly ISessionService _sessionService = sessionService;
    private readonly IValidator<SubmitPersonalDetailModel> _validator = validator;
    public const string ChangePersonalDetailViewPath = "~/Views/EditPersonalInformation/EditPersonalInformation.cshtml";

    private async Task<EditPersonalInformationViewModel> BuildMemberProfileModel(CancellationToken cancellationToken)
    {
        var memberProfiles = await _apiClient.GetMemberProfile(_sessionService.GetMemberId(), _sessionService.GetMemberId(), false, cancellationToken);
        var regionTask = await _apiClient.GetRegions();
        EditPersonalInformationViewModel memberProfile = EditPersonalInformationViewModelMapping(memberProfiles.RegionId ?? 0, memberProfiles.Profiles, memberProfiles.Preferences, memberProfiles.UserType, memberProfiles.OrganisationName);
        memberProfile.Regions = Region.RegionToRegionViewModelMapping(regionTask.Regions);
        memberProfile.YourAmbassadorProfileUrl = Url.RouteUrl(SharedRouteNames.YourAmbassadorProfile)!;
        return memberProfile;
    }

    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        return View(ChangePersonalDetailViewPath, await BuildMemberProfileModel(cancellationToken));
    }

    [HttpPost]
    public async Task<IActionResult> Post(SubmitPersonalDetailModel submitPersonalDetailModel, CancellationToken cancellationToken)
    {
        var result = await _validator.ValidateAsync(submitPersonalDetailModel, cancellationToken);

        if (!result.IsValid)
        {
            result.AddToModelState(ModelState);
            return View(ChangePersonalDetailViewPath, await BuildMemberProfileModel(cancellationToken));
        }
        UpdateMemberProfileAndPreferencesRequest updateMemberProfileAndPreferencesRequest = new();
        updateMemberProfileAndPreferencesRequest.PatchMemberRequest.RegionId = submitPersonalDetailModel.RegionId;
        updateMemberProfileAndPreferencesRequest.PatchMemberRequest.OrganisationName = submitPersonalDetailModel.OrganisationName;
        List<UpdatePreferenceModel> updatePreferenceModels =
        [
            new UpdatePreferenceModel() { PreferenceId = PreferenceConstants.PreferenceIds.Biography, Value = submitPersonalDetailModel.ShowBiography && !string.IsNullOrEmpty(submitPersonalDetailModel.Biography) },
            new UpdatePreferenceModel() { PreferenceId = PreferenceConstants.PreferenceIds.JobTitle, Value = submitPersonalDetailModel.ShowJobTitle },
        ];

        updateMemberProfileAndPreferencesRequest.UpdateMemberProfileRequest.MemberPreferences = updatePreferenceModels;

        List<UpdateProfileModel> updateProfileModels =
        [
            new UpdateProfileModel() { MemberProfileId = ProfileIds.Biography, Value = submitPersonalDetailModel.Biography?.Trim() },
            new UpdateProfileModel() { MemberProfileId = ProfileIds.JobTitle, Value = submitPersonalDetailModel.JobTitle?.Trim() },
        ];

        updateMemberProfileAndPreferencesRequest.UpdateMemberProfileRequest.MemberProfiles = updateProfileModels;

        await _apiClient.UpdateMemberProfileAndPreferences(_sessionService.GetMemberId(), updateMemberProfileAndPreferencesRequest, cancellationToken);
        TempData[TempDataKeys.YourAmbassadorProfileSuccessMessage] = true;
        return RedirectToRoute(SharedRouteNames.YourAmbassadorProfile);
    }

    public static EditPersonalInformationViewModel EditPersonalInformationViewModelMapping(int regionId, IEnumerable<MemberProfile> memberProfiles, IEnumerable<MemberPreference> memberPreferences, MemberUserType userType, string? organisationName)
    {
        EditPersonalInformationViewModel memberProfile = new()
        {
            RegionId = regionId,
            ShowBiography = MapProfilesAndPreferencesService.GetPreferenceValue(PreferenceConstants.PreferenceIds.Biography, memberPreferences),
            ShowJobTitle = MapProfilesAndPreferencesService.GetPreferenceValue(PreferenceConstants.PreferenceIds.JobTitle, memberPreferences),
            Biography = MapProfilesAndPreferencesService.GetProfileValue(ProfileIds.Biography, memberProfiles),
            JobTitle = MapProfilesAndPreferencesService.GetProfileValue(ProfileIds.JobTitle, memberProfiles),
            UserType = userType,
            OrganisationName = organisationName ?? string.Empty
        };

        return memberProfile;
    }
}